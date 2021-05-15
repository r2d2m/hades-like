using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Reward : MonoBehaviour {
    public AudioClip pickupSound;
    public ItemInfo info;
    public string itemName = "Item Name";
    public string infoText = "This is a placeholder text";

    void Awake() {
        info = GameObject.Find("PlayerUI").GetComponent<ItemInfo>();
    }

    public virtual void grabReward(GameObject player) {
        if (pickupSound != null) {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.7f);
        }
        GameObject.Find("Floor").GetComponent<RoomManager>().rewardWasGrabbed();
    }

    public void despawnSelf() {
        info.CloseWindow();
        Destroy(gameObject);
    }

    void OnMouseEnter() {
        print("Enter");
        info.OpenWindow(transform.position.x, transform.position.y, itemName, infoText);
    }

    void OnMouseExit() {
        print("Exit");
        info.CloseWindow();
    }

}
