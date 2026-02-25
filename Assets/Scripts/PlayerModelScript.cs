using UnityEngine;

public class PlayerModelScript : MonoBehaviour {

    public GameObject Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        transform.position = Player.transform.position;
        if (!Player.GetComponent<PlayerScript>().IsOnWall()) {
            transform.rotation = Player.transform.rotation * Quaternion.Euler(90,0,0);
        }
    }

    public void LandedOnWall(Collision collision) {
        transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal, Vector3.up) * Quaternion.Euler(90,0,0);
    }
}
