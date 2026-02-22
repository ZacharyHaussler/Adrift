using Unity.Mathematics;
using UnityEngine;
//using UnityEngine.InputSystem;



public class PlayerScript : MonoBehaviour {

    public Rigidbody rigidbody;
    public float XSensitivity;
    public float YSensitivity;

    private float XRotation;
    private float YRotation;
    
    


    
    private Vector3 ThrustDirection = new Vector3(0f,0f,0f);
    private float AngleForce = 0.0f;


    void Awake() {
        //playerControls = new PlayerControls();
        

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void LateUpdate() {
        

        XRotation = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * YSensitivity * -1;
        YRotation = Input.GetAxisRaw("Mouse X") * Time.deltaTime * XSensitivity;
        
        

        transform.rotation *= Quaternion.Euler(XRotation, YRotation, 0);

    }
    
    
    // FixedUpdate is called 50 times per second
    void FixedUpdate() {
        ThrustDirection = new Vector3(0f,0f,0f);
        ThrustDirection.x += (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        ThrustDirection.z += (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        ThrustDirection.y += (Input.GetKey(KeyCode.Space) ? 1 : 0) - (Input.GetKey(KeyCode.LeftShift) ? 1 : 0);
        
        


        if (ThrustDirection != new Vector3(0f,0f,0f)) {
            rigidbody.AddForce(4f*ThrustDirection.x*(new Vector3(0f,0f,0f)+transform.right), ForceMode.Force);
            rigidbody.AddForce(4f*ThrustDirection.y*(new Vector3(0f,0f,0f)+transform.up), ForceMode.Force);
            rigidbody.AddForce(4f*ThrustDirection.z*(new Vector3(0f,0f,0f)+transform.forward), ForceMode.Force);
        }

    }

    


}
