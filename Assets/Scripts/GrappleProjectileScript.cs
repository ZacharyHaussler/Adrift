using UnityEngine;

public class GrappleProjectileScript : MonoBehaviour {

    public GameObject player;
    public Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
    }

    // Called when the grapple hits something. Stops grapple and tells playerScript than the grapple landed
    void OnCollisionEnter(Collision collision) {
        rb.linearVelocity = Vector3.zero;
        transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal, Vector3.up) * Quaternion.Euler(90,0,0);
        player.GetComponent<PlayerScript>().GrappleLanded();
    }
}
