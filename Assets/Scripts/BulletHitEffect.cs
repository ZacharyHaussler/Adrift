using UnityEngine;

public class BulletHitEffect : MonoBehaviour {
    
    void Start() {
        Invoke("DestroySelf", 2f);
        
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
