using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/*
Script to be attached to an NPC, allowing them to communicate with their dialogue manager, 
as well as the text display and dialogue panel of the scene.
*/

public class DialougeUIManager : MonoBehaviour
{
    
    public GameObject dialogueManager;
    public float dialogueDistance = 2;
    public GameObject[] answerButtons;
    public MousePointerScript mouseScript;

    private int nButtons = 3;

    private GameObject dialoguePanel;
    private TextMeshProUGUI textDisplay;
    private GameObject player;

    // Bools for keeping track on dialogue and knowing if to break a fader-coroutine.
    
    private bool inDialogue = false;
    private bool fadingIn = false;
    private bool fadingOut = false;

    // Panelfade

    public float inTop = -700f; // May cause problem?
    public float outTop = -900f; // May cause problem?
    public float panelFadeTime = 0.5f;
    private float panelHeight;

    // Textfade
    public float textFadeTime = 0.2f;

    void Awake()
    {   
        // Fetch components
        player = GameObject.FindGameObjectWithTag("Player");
        dialoguePanel = GameObject.FindGameObjectWithTag("DialoguePanel");
        textDisplay = GameObject.FindGameObjectWithTag("DialogueDisplay").GetComponent<TextMeshProUGUI>();
        
        Array.Resize(ref answerButtons,nButtons);
        for (int i = 0; i < nButtons; i++) {
            answerButtons[i] = GameObject.FindGameObjectWithTag("DialogueDisplay").gameObject.transform.GetChild(i).gameObject;
            answerButtons[i].GetComponent<Button>().interactable = false;
        }
        
        mouseScript = GameObject.FindGameObjectWithTag("MousePointer").GetComponent<MousePointerScript>();

        panelHeight = Mathf.Abs(inTop-outTop);
    }

    void Update() {

            // Activates dialogue when player comes within range of object.

            if ((player.transform.position - transform.position).magnitude < dialogueDistance) {

                if (inDialogue == false) {
                    inDialogue = true;
                    fadingOut = false;
                    fadingIn = true;

                    StartCoroutine(FadeInPanel());
                    mouseScript.setMouseHand();
                    player.GetComponent<PlayerMain>().inDialogue = true;
                }

            // Deactivates dialogue when player exits range of object.

            } else {

                if (inDialogue) {
                    inDialogue = false;
                    fadingOut = true;
                    fadingIn = false;

                    StartCoroutine(FadeOutText());
                    mouseScript.setMouseAim();
                    player.GetComponent<PlayerMain>().inDialogue = false;
                }

            }
            
        }

    // IEnumerator for fading in text.

    IEnumerator FadeInText() {
        if (dialogueManager.GetComponentInChildren<Dialogue>().answering) {
            foreach (GameObject btn in answerButtons) {
                btn.GetComponent<Button>().interactable = true;
            }
        }

        while (textDisplay.color.a < 1) {
            if (fadingOut) break;

            Color deltaVector = new Color(0,0,0,Time.deltaTime/textFadeTime) ;
            textDisplay.color += deltaVector;

            if (dialogueManager.GetComponentInChildren<Dialogue>().answering) {
                
                foreach (GameObject btn in answerButtons) {
                    btn.GetComponentInChildren<TextMeshProUGUI>().color += deltaVector;
                }
            }

            yield return null;
            }
        dialogueManager.SetActive(true);
        fadingIn = false;
    }

    // IEnumerator for fading out text. Starts FadeOutPanel() when done.

    IEnumerator FadeOutText() {
        
        dialogueManager.SetActive(true);
        while (textDisplay.color.a > 0) {
            if (fadingIn) break;
            
            Color deltaVector = new Color(0,0,0,Time.deltaTime/textFadeTime) ;
            textDisplay.color -= deltaVector;

            if (dialogueManager.GetComponentInChildren<Dialogue>().answering) {
                foreach (GameObject btn in answerButtons) {
                    btn.GetComponentInChildren<TextMeshProUGUI>().color -= deltaVector;
                }
            }

            yield return null;
            }
        
        foreach (GameObject btn in answerButtons) {
            btn.GetComponent<Button>().interactable = false;
        }
        
        StartCoroutine(FadeOutPanel());
    }

    // IEnumerator for fading in panel. Starts FadeInText() when done.

    IEnumerator FadeInPanel() {

        RectTransform rt = dialoguePanel.GetComponent<RectTransform>();

        while(rt.offsetMax.y < inTop) {
            if (fadingOut) break;

            float deltaTop = Time.deltaTime * panelHeight/panelFadeTime;
            rt.offsetMax += new Vector2(0,deltaTop); 

            yield return null;
        }
        StartCoroutine(FadeInText());
        
    }

    // IEnumerator for fading out panel.
    IEnumerator FadeOutPanel() {
        
        dialogueManager.SetActive(false);

        RectTransform rt = dialoguePanel.GetComponent<RectTransform>();

        while(rt.offsetMax.y > outTop) {
            if (fadingIn) break;

            float deltaTop = Time.deltaTime * panelHeight/panelFadeTime;
            rt.offsetMax  -= new Vector2(0,deltaTop);

            yield return null;
        }
        fadingOut = false;
    }

    

    
}

