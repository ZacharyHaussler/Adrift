using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasScript : MonoBehaviour {

    public Slider HSensSlider;
    public Slider VSensSlider;
    public PlayerScript player;
    public GameObject InGameSettingsMenu;
    public TextMeshProUGUI HText;
    public TextMeshProUGUI VText;

    void Start() {
        HSensSlider.value = PlayerPrefs.GetFloat("XSensitivity", 500f);
        VSensSlider.value = PlayerPrefs.GetFloat("YSensitivity", 500f);
        HText.text = Mathf.Round((HSensSlider.value - 100f) / 1900f * 100f).ToString() + "%";
        VText.text = Mathf.Round((VSensSlider.value - 100f) / 1900f * 100f).ToString() + "%";
    }

    public void SetXSensitivity() {
        PlayerPrefs.SetFloat("XSensitivity", HSensSlider.value);
        HText.text = Mathf.Round((HSensSlider.value - 100f) / 1900f * 100f).ToString() + "%";
        player.XSensitivity = HSensSlider.value;
    }
    public void SetYSensitivity() {
        PlayerPrefs.SetFloat("YSensitivity", VSensSlider.value);
        VText.text = Mathf.Round((VSensSlider.value - 100f) / 1900f * 100f).ToString() + "%";
        player.YSensitivity = VSensSlider.value;
    }
    public void ShowInGameSettings() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InGameSettingsMenu.SetActive(true);
        PlayerScript.InGameSettingsActive = true;
    }
    public void HideInGameSettings() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InGameSettingsMenu.SetActive(false);
        PlayerScript.InGameSettingsActive = false;
    }
}
