using UnityEngine;

public class DroneRandomFly : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 2f;
    public float directionChangeInterval = 3f;

    [Header("Boundary Settings")]
    public float sphereRadius = 100f;
    public Vector3 sphereCenter = Vector3.zero;

    private Vector3 targetDirection;
    private float directionTimer;

    void Start()
    {
        Vector3 randomDirection = Random.insideUnitSphere * sphereRadius;
        randomDirection += transform.position;
        transform.position = randomDirection;
        PickNewDirection();
        directionTimer = directionChangeInterval;
    }

    void Update()
    {
        directionTimer -= Time.deltaTime;

        if (directionTimer <= 0f)
        {
            PickNewDirection();
            directionTimer = directionChangeInterval;
        }

        StayInsideSphere();

        RotateTowardsDirection();
        MoveForward();
    }

    void PickNewDirection()
    {
        targetDirection = Random.onUnitSphere;
    }

    void RotateTowardsDirection()
    {
        if (targetDirection == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void MoveForward()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void StayInsideSphere()
    {
        Vector3 offset = transform.position - sphereCenter;
        float distanceFromCenter = offset.magnitude;

        if (distanceFromCenter > sphereRadius)
        {
            // Force direction back toward center
            targetDirection = (sphereCenter - transform.position).normalized;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(sphereCenter, sphereRadius);
    }
}