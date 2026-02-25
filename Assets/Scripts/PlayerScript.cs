//using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.InputSystem;
using TMPro;
using System;
using Unity.Mathematics;


public class PlayerScript : MonoBehaviour {

    //Physics
    private Rigidbody rb;

    //General Gameplay
    public float PlayerHealth = 100f; //hp of the player as a percent (0-100)
    public float RegenDelay = 2f; //after RegenDelay seconds of not taking damage, the player will begin regenerating health
    public float RegenSpeed = 50f; //health regenerated per second when the player is regenerating health
    public float FuelDepletionRate = 40f; //fuel depleted per second when using the jetpack
    public float RefuelDelay = 0.5f; //after RefuelDelay seconds of not using fuel, the player will begin to regenerate fuel 
    public float RefuelSpeed = 50f; //fuel regenerated per second when the player is regenerating fuel
    public float Fuel = 100f; //jetpack fuel the player has as a percent (0-100)
    private float HealTimeStamp = 0f; //Time.time value of the last time the player took damage. Used w/ RegenDelay to determine when the player begins regenerating hp
    private float FuelTimeStamp = 0f; //Time.time value of the last time the jetpack was used. Used w/ RefuelDelay to determine when the player begins regenerating fuel

    //Mouse Control
    public float XSensitivity = 100f; //mouse sensitivity in the horizontal direction
    public float YSensitivity = 100f; //mouse sensitivity in the vertical direction
    private float XRotation; //degrees the player rotates about the local x axis every frame. Calculated with YSensitivity
    private float YRotation; //degrees the player rotates about the local y axis every frame. Calculated with XSensitivity

    //Player Movement
    private float x = 0.0f; //variables that store the player's intention with respect to jetpack movement
    private float y = 0.0f; // ^^
    private float z = 0.0f; // ^^
    public float JumpForce = 15f; //force exerted on the player when jumping off a wall (using ForceMode.Impulse)
    public float JetpackForce = 12f; //force exerted on the player every physics frame when using the jetpack (using ForceMode.Force)
    public float DangerSpeed = 25f; //minimum speed at which the player will take damage upon hitting a wall
    public float DeathSpeed = 50f; //speed at which the player will take 100% damage upon hitting a wall

    //Gamestates
    private bool OnWall = false; //stores if the player is on a wall. When true, disables jetpack movement and enables jumping
    private bool GrappleConnected = false; //stores if the grapple has latched onto a surface. When true, force due to the grapple is applied to the player

    //Grapple
    public GameObject GrapplePrefab; //prefab for the grapple projectile object
    public GameObject GrappleLinePrefab; //prefab for the line that connects the grapple projectile and the player
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    private GameObject? Grapple = null; //object that stores the instance of each grapple. Is null when no grapple is active
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    private GameObject GrappleLine; //object that stores the instance of the line that connects the grapple projectile and the player
    public float GrappleLaunchSpeed = 60f; //base speed of the grapple projectile when it is spawned in
    public float GrappleMaxLength = 75f; //distance between grapple projectile and player at which the grapple is automatically despawned

    //UI Control
    public GameObject Healthbar; //connected to the green bar on the UI that displays the player's hp
    public GameObject HealthText; //connected to the text above the green bar on the UI. Displays a label and a numerical value for the player's health
    public GameObject FuelBar; //connected to the orange bar on the UI that displays the player's fuel


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Currently responsible for locking and hiding the cursur
    void Start() {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called every frame
    void Update() {

        // Regenerates fuel enough time has passed since last jetpack use
        if (Time.time - FuelTimeStamp > RefuelDelay && Fuel < 100f) {
            Fuel += RefuelSpeed * Time.deltaTime;
            if (Fuel > 100f) {
                Fuel = 100f;
            }
            UpdateFuelUI();
        }

        // Regenerates health if enough time has passed since last damage taken
        if (Time.time - HealTimeStamp > RegenDelay && PlayerHealth < 100f) {
            PlayerHealth += RegenSpeed * Time.deltaTime;
            if (PlayerHealth > 100f) {
                PlayerHealth = 100f;
            }
            UpdateHeathUI();
        }

        // Detects when player jumps off a wall and applies force accordingly
        if (OnWall && Input.GetKeyDown(KeyCode.Space)) {
            OnWall = false;
            rb.AddForce(JumpForce * transform.forward, ForceMode.Impulse);
        }

        // Spawns grapple at player position and gives it speed. Also spawns the line between player and grapple
        if (Input.GetMouseButtonDown(0)) {
            Grapple = Instantiate(GrapplePrefab, transform.position + 0.6f * transform.forward + 0.5f * transform.up, Quaternion.Euler(transform.forward));
            Grapple.GetComponent<Rigidbody>().linearVelocity = GrappleLaunchSpeed * transform.forward + Vector3.Dot(rb.linearVelocity, transform.forward) * transform.forward;
            Grapple.transform.rotation = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(90,0,0);
            Grapple.GetComponent<GrappleProjectileScript>().player = gameObject;
            

            GrappleLine = Instantiate(GrappleLinePrefab, Vector3.zero, Quaternion.identity);
            GrappleLine.GetComponent<GrappleHandler>().player = transform;
            GrappleLine.GetComponent<GrappleHandler>().grapple = Grapple.transform;
        }

        // Breaks grapple if its max distance is exceded
        if (Grapple != null && Vector3.Distance(Grapple.transform.position, transform.position) > GrappleMaxLength) {
            KillGrapple();
        }

        // Deletes the grapple and grapple line when player lets go of grapple button
        if (Input.GetMouseButtonUp(0)) {
            KillGrapple();
        }

    }

    // LateUpdate is called every frame after Update
    // Currently responsible for detecting mouse movement and applying it to the player
    void LateUpdate() {

        XRotation = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * YSensitivity * -1;
        YRotation = Input.GetAxisRaw("Mouse X") * Time.deltaTime * XSensitivity;

        transform.rotation *= Quaternion.Euler(XRotation, YRotation, 0);

    }

    // FixedUpdate is called 50 times per second, and is primarily used for continuous physics calculations
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
        if (!OnWall && Fuel > 0f && JetpackForceV != Vector3.zero) {
            rb.AddForce(JetpackForce * JetpackForceV.normalized, ForceMode.Force);
            Fuel -= FuelDepletionRate * Time.deltaTime;
            if (Fuel < 0f) {
                Fuel = 0f;
            }
            FuelTimeStamp = Time.time;
            UpdateFuelUI();
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
        
        // Collision damage if player is moving too fast
        if (collision.relativeVelocity.magnitude > DeathSpeed) {
            TakeDamage(100f);
        } else if (collision.relativeVelocity.magnitude > DangerSpeed) {
            TakeDamage(100f * (collision.relativeVelocity.magnitude - DangerSpeed) / (DeathSpeed - DangerSpeed));
        }

        // Causes player to stick to wall
        rb.linearVelocity = new Vector3(0,0,0);
        OnWall = true;

        // Destroys grapple to avoid sketchy nonsense
        if (GrappleConnected) {
            KillGrapple();
        }
    }

    // Called by the grapple when the grapple lands
    public void GrappleLanded() {
        GrappleConnected = true;
    }

    // Destroys the grapple and grapple line
    // CURRENTLY BUGGED IN SOME WAY
    void KillGrapple() {
        if (Grapple != null) {
            GrappleConnected = false;
            Destroy(Grapple);
            Destroy(GrappleLine);
            Grapple = null;
        }
    }

    // Deals damage to the player equal to given float
    public void TakeDamage(float damage) {
        PlayerHealth -= damage;
        HealTimeStamp = Time.time;
        if (PlayerHealth < 0f) {
            PlayerHealth = 0f;
        }
        UpdateHeathUI();
    }

    // Updates the healthbar when damage is taken or healing is recieved 
    private void UpdateHeathUI() {
        Healthbar.GetComponent<RectTransform>().sizeDelta = new Vector2(4.5f * PlayerHealth, 20);
        Healthbar.GetComponent<RectTransform>().anchoredPosition = new Vector2(265f - (450f - 4.5f * PlayerHealth)/2, 40);

        HealthText.GetComponent<TextMeshProUGUI>().text = "Health - " + (math.floor(PlayerHealth*10)/10).ToString();
    }

    // Updates the fuel bar when fuel is lost or gained
    private void UpdateFuelUI() {
        FuelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 6f * Fuel);
        FuelBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(75, 50 - (600f - 6f * Fuel)/2);
    }


    
}
