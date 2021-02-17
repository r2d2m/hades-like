using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;

    public MousePointerScript mouseScript;

    private void Start() {
        mouseScript.setMouseHand();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
           
            if (paused) {
                resume();
            } else {
                pause();
            }
        }
        
    }

    public void resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void quit() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
