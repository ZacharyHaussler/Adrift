using UnityEngine;
//using UnityEngine.InputSystem;



public class PlayerScript : MonoBehaviour {

    public Rigidbody rigidbody;

    //private PlayerControls playerControls;
    private Vector3 ThrustDirection = new Vector3(0f,0f,0f);
    private float AngleForce = 0.0f;


    void Awake() {
        //playerControls = new PlayerControls();
        

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        ThrustDirection = new Vector3(0f,0f,0f);
        ThrustDirection.x += (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        ThrustDirection.z += (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        ThrustDirection.y += (Input.GetKey(KeyCode.Space) ? 1 : 0) - (Input.GetKey(KeyCode.LeftShift) ? 1 : 0);
        Debug.Log(ThrustDirection);
        if (ThrustDirection != new Vector3(0f,0f,0f)) {
            rigidbody.AddForce(2*ThrustDirection/ThrustDirection.magnitude, ForceMode.Force);
        }

    }

    


}
