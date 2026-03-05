using UnityEngine;

public class GrappleProjectileScript : MonoBehaviour {

    public GameObject player;
    public Rigidbody rb;

    // Called when the grapple hits something. Stops grapple and tells playerScript than the grapple landed
    void OnCollisionEnter(Collision collision) {
        rb.linearVelocity = Vector3.zero;
        Vector3 ContactPoint = collision.gameObject.GetComponent<Collider>().ClosestPoint(collision.contacts[0].point);
        transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal, Vector3.up) * Quaternion.Euler(90,0,0);
        transform.position = ContactPoint;
        player.GetComponent<PlayerScript>().GrappleLanded();
        transform.SetParent(collision.transform);
    }
}
