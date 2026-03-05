using UnityEngine;

public class TimedDestroy : MonoBehaviour {

    public float DestroyTime = 5f;
    
    void Start(){
        Invoke("DestroySelf", DestroyTime);
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
