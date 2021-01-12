using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
    bool isOpen;
    Color openColor = Color.green;
    Color closedColor = Color.red;
    RoomManager roomManager;
    GameObject roomSelectionUI;

    // Start is called before the first frame update
    void Start() {
        roomSelectionUI = GameObject.FindGameObjectWithTag("RoomSelectionUI");
        roomManager = GameObject.FindGameObjectWithTag("Floor").GetComponent<RoomManager>();
        closeDoor();
    }

    // Update is called once per frame
    void Update() {
        if (isOpen) {
            roomSelectionUI.transform.position = transform.position + new Vector3(0, 1f, 0);
        }
    }

    public void openDoor() {
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        roomSelectionUI.transform.position = transform.position + new Vector3(0, 0.8f, 0);
        isOpen = true;
    }

    public void closeDoor() {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        isOpen = false;
        activeUI(false);
    }

    private void activeUI(bool active) {
        foreach (Transform child in roomSelectionUI.transform) {
            child.gameObject.SetActive(active);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && isOpen) {
            activeUI(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player" && isOpen) {
            activeUI(false);
        }
    }
}
