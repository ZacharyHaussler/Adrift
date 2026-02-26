using UnityEngine;
using TMPro;

public class GunScript : MonoBehaviour {

    public GameObject BulletPrefab;
    public int GunType = 0;
    private string[] GunNames = {"ProtoGun"};
    public GameObject Player;
    private GameObject Bullet;
    public Camera cam;

    private float ReloadTime;
    private float FireRate;
    private float LastShotTimeStamp = 0.0f;
    private int AmmoCapacity;
    private int Ammo;
    private float BulletDmg;
    private float BulletSpeed;

    private bool Reloading = false;
    private float ReloadTimeStamp = 0.0f;

    //UI
    public TextMeshProUGUI AmmoText;
    public TextMeshProUGUI GunText;
    public GameObject ReloadIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        UpdateGunSelection();
        ReloadIcon.SetActive(false);
        transform.position = new Vector3(.5f, 0.1f, 1f);
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButton(0) && !Reloading) {
            if (Ammo <= 0) {
                if (!Reloading) {
                    Reloading = true;
                    ReloadTimeStamp = Time.time;
                    ReloadIcon.SetActive(true);
                }
            } else {
                FireBullet();
            }

        }

        if (Input.GetKeyDown(KeyCode.R) && Ammo != AmmoCapacity && !Reloading) {
            Reloading = true;
            ReloadTimeStamp = Time.time;

            ReloadIcon.SetActive(true);
        }

        if (Reloading) {
            if (Time.time - ReloadTimeStamp >= ReloadTime) {
                Ammo = AmmoCapacity;
                Reloading = false;
                ReloadIcon.SetActive(false);
                AmmoText.text = Ammo + " / " + AmmoCapacity;
            }
        }

        
        
    }

    void FixedUpdate() {
        

        if (Physics.Raycast(Player.transform.position + 0.5f * Player.transform.up, Player.transform.forward, out RaycastHit hit, 1000f)) {
            transform.LookAt(hit.point, Player.transform.up);
            Debug.Log(hit.distance.ToString());
        } else {
            transform.rotation = Player.transform.rotation;
        }

        if (Input.GetMouseButton(1)) { 
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, -.2f, 1f), 0.2f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 30, 0.2f);
        
        } else  {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.5f, -.4f, 1f), 0.1f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 70, 0.2f);
        }
    }

    private void UpdateGunSelection() {
        switch(GunType) {
            case 0:
                FireRate = 0.2f;
                ReloadTime = 2f;
                AmmoCapacity = 15;
                BulletDmg = 20;
                BulletSpeed = 100f;
                print("case called");
                break;
        }
        Ammo = AmmoCapacity;
        AmmoText.text = Ammo + " / " + AmmoCapacity;
        GunText.text = GunNames[GunType];
    }

    private void FireBullet() {
        if (Time.time - LastShotTimeStamp >= FireRate) {
            Bullet = Instantiate(BulletPrefab, transform.position + 0.5f * Player.transform.right - 0.4f * Player.transform.right + 1.5f * Player.transform.forward, Quaternion.Euler(transform.forward));
            Bullet.GetComponent<BulletScript>().SetValues(Player, BulletDmg, BulletSpeed);
            


            LastShotTimeStamp = Time.time;
            Ammo -= 1;

            AmmoText.text = Ammo + " / " + AmmoCapacity;
        }
    }

    

}
