using UnityEngine;

public class SetVelocity : MonoBehaviour {
    public Vector3 velocity;
    public bool SlowDown = true;

    void Update() {
        transform.position += velocity * Time.deltaTime;
        if (SlowDown) {
            velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 5f);
        }
    }
}
