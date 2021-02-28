using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialouge : MonoBehaviour
{
    
    public GameObject dialogueManager;
    public GameObject dialogueCanvas;
    public float dialogueDistance = 2;

    private GameObject player;
    private bool inDialogue = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        if ((player.transform.position - transform.position).magnitude < dialogueDistance) {

            if (inDialogue == false) {
                inDialogue = true;
                dialogueManager.SetActive(true);
                dialogueCanvas.SetActive(true);
            }

        } else {

            if (inDialogue) {
                inDialogue = false;
                dialogueManager.SetActive(false);
                dialogueCanvas.SetActive(false);
            }

        }
        
    }
}
