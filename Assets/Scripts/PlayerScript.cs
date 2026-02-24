//using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {

    //Physics
    public Rigidbody rigidbody;
    public float JetpackForce = 12f;

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
    public GameObject GrappleLinePrefab;
    private GameObject Grapple;
    private GameObject GrappleLine;
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

            GrappleLine = Instantiate(GrappleLinePrefab, Vector3.zero, Quaternion.identity);
            GrappleLine.GetComponent<GrappleHandler>().player = transform;
            GrappleLine.GetComponent<GrappleHandler>().grapple = Grapple.transform;
        }

        if (Input.GetMouseButtonUp(0)) {
            GrappleThrown = false;
            GrappleConnected = false;

            Destroy(Grapple);
            Destroy(GrappleLine);
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
        y = Input.GetAxisRaw("Vertical");
        z = (Input.GetKey(KeyCode.Space) ? 1f : 0f) - (Input.GetKey(KeyCode.LeftShift) ? 1f : 0f);
        
        Vector3 JetpackForceV =
            x * transform.right +
            y * transform.up +
            z * transform.forward;

        if (!OnWall) {
            rigidbody.AddForce(JetpackForce * JetpackForceV.normalized, ForceMode.Force);
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

    // void OnCollisionExit(Collision collision) {
    //     OnWall = false;
    // }
}
