using UnityEngine;

public class ArenaManager : MonoBehaviour {
    public float rotationSpeed = 2f;
    public Transform ArenaStands;
    public Transform DirectionalLight;

    void Update() {
        float skyRotation = Time.time * rotationSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", skyRotation);
        ArenaStands.Rotate(Vector3.up + Vector3.right, rotationSpeed * Time.deltaTime);
        DirectionalLight.rotation = Quaternion.Euler(0f, 180f - skyRotation, 0f);
    }
}