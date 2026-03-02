using UnityEngine;
using TMPro;

public class TargetBlock : MonoBehaviour {

    public float health = 100f;
    public float mapRadius = 50f;
    public TextMeshPro TextMesh;
    public GameObject ExplosionPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        randomPos();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Bullet") {
            health -= collision.gameObject.GetComponent<BulletScript>().Damage;
            TextMesh.text = health.ToString();
            if (health <= 0) {
                Vector3 bulletDirection = (collision.gameObject.transform.position - transform.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(bulletDirection);
                Instantiate(ExplosionPrefab, transform.position, rotation);
                randomPos();
                health = 100f;
                TextMesh.text = health.ToString();
            }
        }
    }

    private void randomPos() {
        Vector3 randomDirection = Random.insideUnitSphere * mapRadius;
        transform.position = randomDirection;
    }
}
