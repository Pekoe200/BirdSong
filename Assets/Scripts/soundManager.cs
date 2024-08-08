using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class soundManager : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] AudioMixer mixer;

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";

    void Awake(){
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume")){
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        if(!PlayerPrefs.HasKey("sfxVolume")){
            PlayerPrefs.SetFloat("sfxVolume", 1);
            Load();
        }
        else{
            Load();
        }
    }

    public void SetMusicVolume(float value){
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
        SaveMusic();
    }

    public void SetSFXVolume(float value){
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
        SaveSFX();
    }

    // Load user audio prefs
    private void Load(){
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    // Save music audio prefs
    private void SaveMusic(){
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }

    // Save SFX audio prefs
    private void SaveSFX(){
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }
}
