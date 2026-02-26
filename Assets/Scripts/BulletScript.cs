using UnityEngine;

public class BulletScript : MonoBehaviour {

    private GameObject Owner;
    private float Damage;

    public void SetValues(GameObject Own, float Dmg, float Speed) {
        Owner = Own;
        Damage = Dmg;
        gameObject.GetComponent<Rigidbody>().linearVelocity = Speed * Owner.transform.forward + Vector3.Dot(Owner.GetComponent<Rigidbody>().linearVelocity, Owner.transform.forward) * Owner.transform.forward;
        transform.rotation = Quaternion.LookRotation(Owner.transform.forward) * Quaternion.Euler(90,0,0);

    }
    
    
    // Update is called once per frame
    void Update() {
        if (transform.position.magnitude > 500) {
            Destroy(gameObject);
        }
        
    }

    void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
}
