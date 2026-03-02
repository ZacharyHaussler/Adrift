using UnityEngine;
using System.Collections;

public class ArenaTexureController : MonoBehaviour {
    public Texture2D[] textures;
    public float minFadeTime = 0.5f;
    public float maxFadeTime = 2.5f;
    public float emissionStrength = 2f;

    private Material mat;
    private int currentIndex = 0;

    void Start()
    {
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(Mathf.Abs(transform.position.y * 0.5f), 0f, 0f);
        mat = GetComponent<Renderer>().material;
        mat.SetFloat("_EmissionStrength", emissionStrength);

        if (textures.Length < 2)
        {
            Debug.LogError("Need at least 2 textures!");
            return;
        }

        mat.SetTexture("_TexA", textures[currentIndex]);
        StartCoroutine(TextureLoop());
    }

    IEnumerator TextureLoop()
    {
        while (true)
        {
            int nextIndex;

            // Ensure different texture
            do
            {
                nextIndex = Random.Range(0, textures.Length);
            }
            while (nextIndex == currentIndex);

            mat.SetTexture("_TexB", textures[nextIndex]);

            float fadeTime = Random.Range(minFadeTime, maxFadeTime);
            float timer = 0f;

            while (timer < fadeTime)
            {
                float blend = timer / fadeTime;
                mat.SetFloat("_Blend", blend);
                timer += Time.deltaTime;
                yield return null;
            }

            mat.SetFloat("_Blend", 1f);

            // Swap roles
            mat.SetTexture("_TexA", textures[nextIndex]);
            mat.SetFloat("_Blend", 0f);

            currentIndex = nextIndex;

            // Optional pause between swaps
            yield return new WaitForSeconds(Random.Range(0.2f, 1f));
        }
    }
}