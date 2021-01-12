using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour {
    public AudioClip pickupSound;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public virtual void grabReward(GameObject player) {
        if (pickupSound != null) {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.7f);
        }
        despawnSelf();
    }

    public void despawnSelf() {
        Destroy(gameObject);
    }
}
