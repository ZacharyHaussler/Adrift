using UnityEngine;

public class SetVelocity : MonoBehaviour {
    public Vector3 velocity;

    void Update() {
        transform.position += velocity * Time.deltaTime;
    }
}
