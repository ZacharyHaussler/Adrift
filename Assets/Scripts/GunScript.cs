using UnityEngine;
using TMPro;

public class GunScript : MonoBehaviour {

    public GameObject BulletPrefab;
    public string GunType;
    public GameObject Player;
    private GameObject Bullet;

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
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButton(0)) {
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
        } else {
            transform.rotation = Player.transform.rotation;
        }
    }

    private void UpdateGunSelection() {
        switch(GunType) {
            case "ProtoGun":
                FireRate = 0.2f;
                ReloadTime = 2f;
                AmmoCapacity = 15;
                BulletDmg = 20;
                BulletSpeed = 100f;
            break;
        }
        Ammo = AmmoCapacity;
        AmmoText.text = Ammo + " / " + AmmoCapacity;
        GunText.text = GunType;
    }

    private void FireBullet() {
        if (Time.time - LastShotTimeStamp >= FireRate) {
            Bullet = Instantiate(BulletPrefab, transform.position + 0.5f * Player.transform.right - 0.4f * Player.transform.right + 1.5f * Player.transform.forward, Quaternion.Euler(Player.transform.forward));
            Bullet.GetComponent<BulletScript>().SetValues(Player, BulletDmg, BulletSpeed);
            


            LastShotTimeStamp = Time.time;
            Ammo -= 1;

            AmmoText.text = Ammo + " / " + AmmoCapacity;
        }
    }

    

}
