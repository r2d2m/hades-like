using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Reward : MonoBehaviour {
    public AudioClip pickupSound;
    public ItemInfo info;
    public string infoText;

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
        Destroy(gameObject);
    }

    void OnMouseEnter() {
        info.OpenWindow(transform.position.x,transform.position.y,infoText);
    }

    void OnMouseExit() {
        info.CloseWindow();
    }

}
