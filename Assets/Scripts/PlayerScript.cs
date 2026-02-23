using Unity.Mathematics;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {

    public Rigidbody rigidbody;
    public float XSensitivity = 100f;
    public float YSensitivity = 100f;

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

    void FixedUpdate(){

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        float y = (Input.GetKey(KeyCode.Space) ? 1f : 0f) - (Input.GetKey(KeyCode.LeftShift) ? 1f : 0f);

        Vector3 worldForce =
            x * transform.right +
            y * transform.up +
            z * transform.forward;

        rigidbody.AddForce(4f * worldForce.normalized, ForceMode.Force);
    }

}
