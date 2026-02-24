using UnityEngine;

public class GrappleHandler : MonoBehaviour
{
    public Transform player;
    public Transform grapple;
    public LineRenderer line;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        line.SetPosition(0, player.position);
        line.SetPosition(1, grapple.position);
        // later add in more to give cool grapple effect
    }
}
