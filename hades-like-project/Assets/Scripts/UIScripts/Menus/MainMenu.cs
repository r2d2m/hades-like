using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Canvas canvas;
    public AudioSource audio;
    public float fadeoutTime = 1.0f;
    
    float fadeoutTimer = 0.0f;
    float deltaAlpha;
    float deltaVolume;
    float initialVolume;


    public void PlayGame() { 
        StartCoroutine(FadeOut());       
    }

    IEnumerator FadeOut() {
        initialVolume = audio.GetComponent<AudioSource>().volume;

        while ( fadeoutTimer < fadeoutTime) {
            fadeoutTimer += Time.deltaTime;
            
            deltaAlpha = Time.deltaTime/fadeoutTime;
            deltaVolume = initialVolume*Time.deltaTime/fadeoutTime;
            
            audio.GetComponent<AudioSource>().volume -= deltaVolume;
            canvas.GetComponent<CanvasGroup>().alpha -= deltaAlpha;
            
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("Quiting Game");
        Application.Quit();
    }


}
