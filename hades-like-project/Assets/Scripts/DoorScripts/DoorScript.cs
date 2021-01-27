using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour {
    bool isOpen;
    Color openColor = Color.green;
    Color closedColor = Color.red;
    RoomManager roomManager;
    GameObject roomSelectionUI;
    Animator animator;
    Collider2D[] colliders;
    public AudioClip spawnSound;

    public GameObject exit;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponentInChildren<Animator>();
        roomSelectionUI = GameObject.FindGameObjectWithTag("RoomSelectionUI");
        roomManager = GameObject.FindGameObjectWithTag("Floor").GetComponent<RoomManager>();
        colliders = GetComponents<Collider2D>();
        closeDoor();
    }

    // Update is called once per frame
    void Update() {

    }

    public void openDoor() {
        animator.SetTrigger("WormErupt");
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        setColliders(true);
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>().PlayOneShot(spawnSound);
        isOpen = true;
    }

    public void closeDoor() {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        setColliders(false);
        isOpen = false;
    }

    public void playerExit() {
        if (isOpen) {
            roomManager.doorButtonClicked();
            animator.SetTrigger("PlayerEnter");
        }
    }


    private void setColliders(bool boo) {
        foreach (Collider2D collider in colliders) {
            collider.enabled = boo;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && isOpen) {
            animator.SetTrigger("OpenMouth");
            //roomManager.doorButtonClicked();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player" && isOpen) {
            animator.SetTrigger("CloseMouth");
        }
    }
}
