using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour {
    public AudioClip pickupSound;

    public virtual void grabReward(GameObject player) {
        if (pickupSound != null) {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.7f);
        }
        GameObject.Find("Floor").GetComponent<RoomManager>().rewardWasGrabbed();
    }

    public void despawnSelf() {
        Destroy(gameObject);
    }
}
