using UnityEngine;
using UnityEngine.Audio;

public class InitializeSettings : MonoBehaviour {
    
    [SerializeField] private AudioMixer mixer;
    
    void Start() {
        Initialize();
    }
    
    //Set the sound settings.
    private void Initialize() {
        float musicVol = PlayerPrefs.GetFloat("musicVol", 0.7f);
        float sfxVol = PlayerPrefs.GetFloat("sfxVol", 0.7f);
        
        mixer.SetFloat("MusicVol", Mathf.Log10(musicVol) * 20);
        mixer.SetFloat("SFXVol", Mathf.Log10(sfxVol) * 20);
    }
}
