using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;

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
