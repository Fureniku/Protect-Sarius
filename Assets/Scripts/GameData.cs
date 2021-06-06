using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameData : MonoBehaviour {
    
    //This class holds and controls the settings etc of the game.
    //Most controls are called from buttons in the Options panel prefab
    //Most functions should be very self-explanatory. 

    [SerializeField] private Toggle invertControlToggle;
    [SerializeField] private Toggle particleToggle;
    [SerializeField] private Toggle showControlsToggle;
    [SerializeField] private Slider pitchSlider;
    [SerializeField] private Slider yawSlider;
    [SerializeField] private Slider rollSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private GameObject particleField;

    [SerializeField] private AudioMixer mixer;
    
    private GameObject stick;

    void Start() {
        bool inverted = PlayerPrefs.GetInt("invertPitch", 0) != 0;
        bool particles = PlayerPrefs.GetInt("particles", 1) != 0;
        bool showControls = PlayerPrefs.GetInt("showControls", 1) != 0;
        invertControlToggle.isOn = inverted;
        particleToggle.isOn = particles;
        showControlsToggle.isOn = showControls;

        if (particleField != null) {
            particleField.SetActive(particles);
        }
        
        
        Initialize();
    }

    private void Initialize() {
        float pitchVal = PlayerPrefs.GetFloat("pitchSens", 4);
        float yawVal = PlayerPrefs.GetFloat("yawSens", 2);
        float rollVal = PlayerPrefs.GetFloat("rollSens", 4);
        
        bool inverted = PlayerPrefs.GetInt("invertPitch", 0) != 0;
        bool particles = PlayerPrefs.GetInt("particles", 1) != 0;
        bool controls = PlayerPrefs.GetInt("showControls", 1) != 0;

        float musicVol = PlayerPrefs.GetFloat("musicVol", 0.7f);
        float sfxVol = PlayerPrefs.GetFloat("sfxVol", 0.7f);
        
        mixer.SetFloat("MusicVol", Mathf.Log10(musicVol) * 20);
        mixer.SetFloat("SFXVol", Mathf.Log10(sfxVol) * 20);

        pitchSlider.value = pitchVal;
        yawSlider.value = yawVal;
        rollSlider.value = rollVal;

        musicSlider.value = musicVol;
        sfxSlider.value = sfxVol;
        
        invertControlToggle.isOn = inverted;
        particleToggle.isOn = particles;
        showControlsToggle.isOn = controls;
        
        if (particleField != null) {
            particleField.SetActive(particles);
        }
    }

    public void PauseGame() {
        Time.timeScale = 0;
    }

    public void UnpauseGame() {
        Time.timeScale = 1;
    }

    public void SetInvertControls() {
        PlayerPrefs.SetInt("invertPitch", invertControlToggle.isOn ? 1 : 0);
    }

    public void SetParticles() {
        PlayerPrefs.SetInt("particles", particleToggle.isOn ? 1 : 0);
        
        if (particleField != null) {
            particleField.SetActive(particleToggle.isOn);
        }
    }

    public void SetControlsVisible() {
        PlayerPrefs.SetInt("showControls", showControlsToggle.isOn ? 1 : 0);
    }

    public void ResetControls() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("invertPitch", 0);
        PlayerPrefs.SetFloat("pitchSens", 4);
        PlayerPrefs.SetFloat("yawSens", 2);
        PlayerPrefs.SetFloat("rollSens", 4);
        PlayerPrefs.SetInt("quality", 2);
        PlayerPrefs.SetInt("particles", 1);
        Initialize();
    }

    public void SetPitchSens(Slider s) {
        PlayerPrefs.SetFloat("pitchSens", s.value);
    }

    public void SetYawSens(Slider s) {
        PlayerPrefs.SetFloat("yawSens", s.value);
    }

    public void SetRollSens(Slider s) {
        PlayerPrefs.SetFloat("rollSens", s.value);
    }

    public void SetMusicVol(Slider s) {
        mixer.SetFloat("MusicVol", Mathf.Log10(s.value) * 20);
        PlayerPrefs.SetFloat("musicVol", s.value);
    }

    public void SetSFXVol(Slider s) {
        mixer.SetFloat("SFXVol", Mathf.Log10(s.value) * 20);
        PlayerPrefs.SetFloat("sfxVol", s.value);
    }
}
