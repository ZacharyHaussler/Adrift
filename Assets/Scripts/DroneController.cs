using UnityEngine;

public class DroneController : MonoBehaviour {

    [Header("Spawning")]
    public GameObject DronePrefab;
    public int DroneCount = 6;
    public bool ShouldRespawn = true;
    public float RespawnTime = 5f;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 2f;
    public float directionChangeInterval = 3f;

    [Header("Boundaries")]
    public float sphereRadius = 100f;
    public Vector3 sphereCenter = Vector3.zero;

    void Start() {
        for (int i = 0; i < DroneCount; i++) {
            SpawnDrone();
        }
    }

    public void DroneDestroyed() {
        if (ShouldRespawn) {
            Invoke("SpawnDrone", RespawnTime);
        }
    }

    void SpawnDrone() {
        GameObject newDrone = Instantiate(DronePrefab, transform.position, Quaternion.identity);
        DroneRandomFly drone = newDrone.GetComponent<DroneRandomFly>();
        drone.Controller = this;
        drone.moveSpeed = moveSpeed;
        drone.rotationSpeed = rotationSpeed;
        drone.directionChangeInterval = directionChangeInterval;
        drone.sphereRadius = sphereRadius;
        drone.sphereCenter = sphereCenter;
    }
}
