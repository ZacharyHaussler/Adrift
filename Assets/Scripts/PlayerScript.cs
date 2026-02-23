//using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {

    //Physics
    public Rigidbody rigidbody;

    //Mouse Control
    public float XSensitivity = 100f;
    public float YSensitivity = 100f;
    private float XRotation;
    private float YRotation;

    //Player Movement
    private float x = 0.0f;
    private float y = 0.0f;
    private float z = 0.0f;
    private Vector3 ThrustDirection = new Vector3(0f,0f,0f);
    private float AngleForce = 0.0f;

    //Gamestates
    private bool OnWall = false;
    private bool GrappleThrown = false;
    private bool GrappleConnected = false;

    //Grapple
    public GameObject GrapplePrefab;
    private GameObject Grapple;
    private Vector3 GrappleVelocity;


    void Awake() {
        //playerControls = new PlayerControls();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update() {
        if (OnWall && Input.GetKeyDown(KeyCode.Space)) {
            OnWall = false;
            rigidbody.AddForce(15f * transform.forward, ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(0)) {
            GrappleThrown = true;
            Grapple = Instantiate(GrapplePrefab, transform.position + 0.6f*transform.forward, Quaternion.Euler(transform.forward));
            Grapple.GetComponent<Rigidbody>().linearVelocity = rigidbody.linearVelocity + 30f*transform.forward;
            Grapple.transform.rotation = Quaternion.LookRotation(transform.forward);
            GrappleVelocity = Grapple.GetComponent<Rigidbody>().linearVelocity;
        }
        if (Input.GetMouseButtonUp(0)) {
            GrappleThrown = false;
            GrappleConnected = false;
            GrappleVelocity = new Vector3(999,999,999);
            Destroy(Grapple);
        }
        if (!GrappleConnected && GrappleThrown && GrappleVelocity != Grapple.GetComponent<Rigidbody>().linearVelocity) {
            Grapple.GetComponent<Rigidbody>().linearVelocity = new Vector3(0,0,0);
            GrappleConnected = true;
        }
    }

    void LateUpdate() {

        XRotation = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * YSensitivity * -1;
        YRotation = Input.GetAxisRaw("Mouse X") * Time.deltaTime * XSensitivity;

        transform.rotation *= Quaternion.Euler(XRotation, YRotation, 0);

    }

    void FixedUpdate(){

        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        y = (Input.GetKey(KeyCode.Space) ? 1f : 0f) - (Input.GetKey(KeyCode.LeftShift) ? 1f : 0f);
        
        Vector3 JetpackForce =
            x * transform.right +
            y * transform.up +
            z * transform.forward;

        if (!OnWall) {
            rigidbody.AddForce(4f * JetpackForce.normalized, ForceMode.Force);
        }
        if (GrappleConnected) {
            rigidbody.AddForce(15f*(Grapple.transform.position - transform.position).normalized, ForceMode.Force);
            OnWall = false;
        }
    }

    void OnCollisionEnter(Collision collision) {
        OnWall = true;
        rigidbody.linearVelocity = new Vector3(0,0,0);
    }

}
