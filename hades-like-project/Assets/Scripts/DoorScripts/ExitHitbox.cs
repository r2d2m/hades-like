using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHitbox : MonoBehaviour {
    DoorScript parentScript;
    // Start is called before the first frame update
    void Start() {
        parentScript = transform.parent.GetComponent<DoorScript>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            parentScript.playerExit();
        }
    }
}
