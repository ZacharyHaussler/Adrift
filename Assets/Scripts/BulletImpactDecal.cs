using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BulletImpactDecal : MonoBehaviour {
    public DecalProjector Decal;
    
    void Start() {
        float WH = Random.Range(0.5f, 0.7f);
        Decal.size = new Vector3(WH, WH, WH);
        Vector3 rot = transform.eulerAngles;
        rot.z = Random.Range(0f, 360f);
        transform.eulerAngles = rot;
        Invoke("DestroySelf", 10f);
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
