using UnityEngine;

public class BulletScript : MonoBehaviour {

    private GameObject Owner;
    public float Damage;
    public BulletTrail Trail;
    public GameObject TrailPrefab;

    public void SetValues(GameObject Own, float Dmg, float Speed) {
        Owner = Own;
        Damage = Dmg;
        //gameObject.GetComponent<Rigidbody>().linearVelocity = Speed * Owner.transform.forward + Vector3.Dot(Owner.GetComponent<Rigidbody>().linearVelocity, Owner.transform.forward) * Owner.transform.forward;
        
        //transform.rotation = Quaternion.LookRotation(Owner.transform.forward) * Quaternion.Euler(90,0,0);


        gameObject.GetComponent<Rigidbody>().linearVelocity = Speed * transform.forward;
        GameObject TrailGO = Instantiate(TrailPrefab, transform.position + transform.forward * 0.15f, transform.rotation);
        Trail = TrailGO.GetComponent<BulletTrail>();
        Trail.BulletTF = transform;
    }
    
    
    // Update is called once per frame
    void Update() {
        if (transform.position.magnitude > 500) {
            Trail.DestroyTrail();
            Destroy(gameObject);
        }
        
    }

    void OnCollisionEnter(Collision collision) {
        Trail.DestroyTrail();
        Destroy(gameObject);
    }
}
