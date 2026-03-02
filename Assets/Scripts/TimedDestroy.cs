using UnityEngine;

public class TimedDestroy : MonoBehaviour {
    public float DestroyTime = 5f;
    void Start(){
        Invoke("DestorySelf", DestroyTime);
    }

    private void DestorySelf() {
        Destroy(gameObject);
    }
}
