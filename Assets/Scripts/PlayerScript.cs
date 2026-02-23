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
            rigidbody.AddForce(16f * transform.forward, ForceMode.Impulse);
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
    }

    void OnCollisionEnter(Collision collision) {
        OnWall = true;
        rigidbody.linearVelocity = new Vector3(0,0,0);
    }

}
