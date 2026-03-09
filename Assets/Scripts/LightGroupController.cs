using UnityEngine;

public class LightGroupController : MonoBehaviour
{
    [Header("Spotlights to control")]
    public Transform[] spotlights;

    [Header("Wave settings")]
    public float speed = 1f;          // Speed of the wave
    public float amplitude = 60f;     // Max rotation angle on each axis
    public float phaseOffset = 0.5f;  // Offset between each light

    void Update()
    {
        if (spotlights == null || spotlights.Length == 0)
            return;

        float time = Time.time * speed;

        for (int i = 0; i < spotlights.Length; i++)
        {
            if (spotlights[i] == null) continue;

            // Calculate phase offset for each light
            float phase = i * phaseOffset;

            // Sinusoidal rotation values
            float xRot = Mathf.Sin(time + phase) * amplitude;
            float yRot = Mathf.Sin(time + phase + Mathf.PI / 2) * amplitude; // Slightly different phase for Y

            // Apply rotation
            spotlights[i].localRotation = Quaternion.Euler(xRot, yRot, 0f);
        }
    }
}