using UnityEngine;

public class BulletScript : MonoBehaviour {

    private GameObject Owner;
    public float Damage;
    public BulletTrail Trail;
    public GameObject TrailPrefab;
    public GameObject HitEffectPrefab;
    public GameObject ImpactDecalPrefab;

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
    
    void Update() {
        if (transform.position.magnitude > 500) {
            Trail.DestroyTrail();
            Destroy(gameObject);
        }
        
    }

    void FixedUpdate() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 4f)) {
            if (hit.collider.gameObject.tag == "Bounce Pad") {
                if (hit.normal != hit.collider.transform.forward){
                    Trail.DestroyTrail();
                    Destroy(gameObject);
                } else {
                    BouncePadBounce(hit.collider.transform.forward);
                }   
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        Trail.DestroyTrail();
        //Instantiate(HitEffectPrefab, transform.position + transform.forward * 0.15f, transform.rotation * Quaternion.Euler(180,0,0));
        Instantiate(HitEffectPrefab, transform.position, transform.rotation * Quaternion.Euler(180,0,0));
        Instantiate(ImpactDecalPrefab, transform.position, Quaternion.LookRotation(collision.contacts[0].normal, Vector3.up) * Quaternion.Euler(180,0,0));
        Destroy(gameObject);
    }

    public void BouncePadBounce(Vector3 PadNormal) {
        float angle = Vector3.Angle(gameObject.GetComponent<Rigidbody>().linearVelocity.normalized * -1f, PadNormal);
        Debug.Log(angle);
        gameObject.GetComponent<Rigidbody>().linearVelocity += 2f*Mathf.Cos(angle * Mathf.Deg2Rad)*gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude * PadNormal;
    }
}
