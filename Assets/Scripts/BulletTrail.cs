using UnityEngine;

public class BulletTrail : MonoBehaviour {
    public Transform BulletTF;
    public bool Destroyed = false;
    void Update() {
        if(!Destroyed){
            transform.position = BulletTF.position;
        }
    }
    public void DestroyTrail() {
        Destroyed = true;
        Invoke("DestroySelf", 1f);
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
