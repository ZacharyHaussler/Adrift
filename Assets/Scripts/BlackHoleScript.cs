using UnityEngine;

public class BlackHoleScript : MonoBehaviour
{
    
    public float ForceOnPlayer = 300f;
    public float ForceOnBullet = 30f;
    public float Radius = 10f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        transform.localScale = new Vector3(Radius/10f, Radius/10f, Radius/10f);
    }

    

    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Bullet") {
            float distance = Vector3.Distance(transform.position, other.transform.position);
            other.gameObject.GetComponentInParent<Rigidbody>().AddForce(ForceOnBullet/distance/distance * (transform.position - other.transform.position), ForceMode.Force);
        } else if (other.gameObject.tag == "Player") {
            float distance = Vector3.Distance(transform.position, other.transform.position);
            other.gameObject.GetComponent<Rigidbody>().AddForce(ForceOnPlayer/distance/distance * (transform.position - other.transform.position), ForceMode.Force);
            other.gameObject.GetComponent<PlayerScript>().UpdateSpeedUI();
        }
    }
}
