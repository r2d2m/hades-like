using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;

    // Initialize sliders to current volumes.
    void Awake() {
        float volume;

        audioMixer.GetFloat("masterVolume",out volume);
        masterSlider.value = Math.Max(volume,-20f);

        audioMixer.GetFloat("musicVolume",out volume);
        musicSlider.value = Math.Max(volume,-20f);

        audioMixer.GetFloat("effectsVolume",out volume);
        effectsSlider.value = Math.Max(volume,-20f);
    }

    public void SetMasterVolume(float volume) {
        
        if (volume < -19) 
            volume = -80;

        audioMixer.SetFloat("masterVolume",volume);
    }
    
    public void SetMusicVolume(float volume) {
        
        if (volume < -19) 
            volume = -80;

        audioMixer.SetFloat("musicVolume",volume);
    }

    public void SetEffectsVolume(float volume) {
        
        if (volume < -19) 
            volume = -80;

        audioMixer.SetFloat("effectsVolume",volume);
    }

    public void SetQuality(int qualityIndex) {
        Debug.Log(qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }
}
