using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialouge : MonoBehaviour
{
    
    public GameObject dialogueManager;
    public GameObject dialoguePanel;
    public float dialogueDistance = 2;

    private GameObject player;
    private bool inDialogue = false;


    // Panelfade

    private float inTop = -700f;
    private float outTop = -900f;
    private float fadeSpeed = 400f;

    void Awake()
    {   
        
        player = GameObject.FindGameObjectWithTag("Player");
        dialoguePanel = GameObject.FindGameObjectWithTag("DialoguePanel");
        
    }

    //void Start() {
    //    dialoguePanel.SetActive(false);
    //    dialogueManager.SetActive(false);
    //}

    IEnumerator FadeInPanel() {

        RectTransform rt = dialoguePanel.GetComponent<RectTransform>();

        while(rt.offsetMax.y < inTop) {
            rt.offsetMax = new Vector2(rt.offsetMax.x,rt.offsetMax.y + fadeSpeed * Time.deltaTime);
            yield return null;
        }
        dialogueManager.SetActive(true);
    }

    IEnumerator FadeOutPanel() {

        dialogueManager.SetActive(false);

        RectTransform rt = dialoguePanel.GetComponent<RectTransform>();

        while(rt.offsetMax.y > outTop) {

            rt.offsetMax = new Vector2(rt.offsetMax.x,rt.offsetMax.y - fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void Update()
    {

        if ((player.transform.position - transform.position).magnitude < dialogueDistance) {

            if (inDialogue == false) {
                inDialogue = true;
                StopCoroutine(FadeOutPanel());
                StartCoroutine(FadeInPanel());
                //dialogueManager.SetActive(true);
                //dialoguePanel.SetActive(true);
            }

        } else {

            if (inDialogue) {
                inDialogue = false;
                StopCoroutine(FadeInPanel());
                StartCoroutine(FadeOutPanel());
                //dialogueManager.SetActive(false);
                //dialoguePanel.SetActive(false);
            }

        }
        
    }
}
