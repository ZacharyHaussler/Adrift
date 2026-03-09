using UnityEngine;

public class RockSpawner : MonoBehaviour {

    [Header("Rock Settings")]
    public GameObject[] rockPrefabs;
    public int rockCount = 50;

    [Header("Spawn Area")]
    public float spawnRadius = 100f;

    [Header("Scale Range")]
    public float minScale = 1f;
    public float maxScale = 4.0f;

    void Start() {
        SpawnRocks();
    }

    void SpawnRocks() {

        if (rockPrefabs == null || rockPrefabs.Length == 0) return;

        for (int i = 0; i < rockCount; i++) {
            // Random position inside a sphere
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            Vector3 spawnPosition = transform.position + randomOffset;

            // Pick random rock prefab
            GameObject prefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];

            // Random rotation
            Quaternion rotation = Random.rotation;

            // Instantiate rock
            GameObject rock = Instantiate(prefab, spawnPosition, rotation, transform);

            // Random scale
            float scale = Random.Range(minScale, maxScale);
            rock.transform.localScale = Vector3.one * scale;
        }
    }
}