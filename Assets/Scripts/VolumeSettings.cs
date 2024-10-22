using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeSettings : MonoBehaviour {
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;

    private void Start() {
    SetMusicVolume();
}
    public void SetMusicVolume() {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume)*20);
    }
}
