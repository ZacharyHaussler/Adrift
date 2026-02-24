//using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {

    //Physics
    private Rigidbody rb;

    //Mouse Control
    public float XSensitivity = 100f;
    public float YSensitivity = 100f;
    private float XRotation;
    private float YRotation;

    //Player Movement
    private float x = 0.0f;
    private float y = 0.0f;
    private float z = 0.0f;
    public float JumpForce = 15f;
    public float JetpackForce = 12f;

    //Gamestates
    private bool OnWall = false;
    private bool GrappleThrown = false;
    private bool GrappleConnected = false;

    //Grapple
    public GameObject GrapplePrefab;
    public GameObject GrappleLinePrefab;
    private GameObject Grapple;
    private GameObject GrappleLine;
    public float GrappleLaunchSpeed = 30f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Currently responsible for locking and hiding the cursur
    void Start() {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called every frame
    // Currently responsible for jumping off walls, spawning grapple, and deleting grapple
    void Update() {

        // Detects when player jumps off a wall and applies force accordingly
        if (OnWall && Input.GetKeyDown(KeyCode.Space)) {
            OnWall = false;
            rb.AddForce(JumpForce * transform.forward, ForceMode.Impulse);
        }

        // Spawns grapple at player position and gives it speed. Also spawns the line between player and grapple
        if (Input.GetMouseButtonDown(0)) {
            GrappleThrown = true;
            Grapple = Instantiate(GrapplePrefab, transform.position + 0.6f * transform.forward, Quaternion.Euler(transform.forward));
            Grapple.GetComponent<Rigidbody>().linearVelocity = rb.linearVelocity + GrappleLaunchSpeed * transform.forward;
            Grapple.transform.rotation = Quaternion.LookRotation(transform.forward);
            Grapple.transform.rotation *= Quaternion.Euler(90,0,0);
            Grapple.GetComponent<GrappleProjectileScript>().player = gameObject;

            GrappleLine = Instantiate(GrappleLinePrefab, Vector3.zero, Quaternion.identity);
            GrappleLine.GetComponent<GrappleHandler>().player = transform;
            GrappleLine.GetComponent<GrappleHandler>().grapple = Grapple.transform;
        }

        // Deletes the grapple and grapple line when player lets go of grapple button
        if (Input.GetMouseButtonUp(0)) {
            GrappleThrown = false;
            GrappleConnected = false;

            Destroy(Grapple);
            Destroy(GrappleLine);
        }

    }

    // LateUpdate is called every frame after Update
    // Currently responsible for detecting mouse movement and applying it to the player
    void LateUpdate() {

        XRotation = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * YSensitivity * -1;
        YRotation = Input.GetAxisRaw("Mouse X") * Time.deltaTime * XSensitivity;

        transform.rotation *= Quaternion.Euler(XRotation, YRotation, 0);

    }

    // FixedUpdate is called 50 times per second, andis primarily used for continuous physics calculations
    // Currently responsible for player jetpack movement and force applied due to grapple
    void FixedUpdate(){

        // Detects jectpack movement and applies it to player if applicable
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        z = (Input.GetKey(KeyCode.Space) ? 1f : 0f) - (Input.GetKey(KeyCode.LeftShift) ? 1f : 0f);
        Vector3 JetpackForceV =
            x * transform.right +
            y * transform.up +
            z * transform.forward;
        if (!OnWall) {
            rb.AddForce(JetpackForce * JetpackForceV.normalized, ForceMode.Force);
        }

        // Applies grapple force to player if grapple is connected to a surface
        if (GrappleConnected) {
            rb.AddForce(15f*(Grapple.transform.position - transform.position).normalized, ForceMode.Force);
            OnWall = false;
        }
    }

    // OnCollisionEnter is called when the player collides with any other collider
    // Currently responsible for stopping the player when they hit a wall and allowing player to jump off a wall
    // Will need to be modified when other possible collisions are added, eg. bullets and other players
    void OnCollisionEnter(Collision collision) {
        OnWall = true;
        rb.linearVelocity = new Vector3(0,0,0);
    }

    // Called by the grapple when the grapple lands
    public void GrappleLanded() {
        GrappleConnected = true;
    }

    
}
