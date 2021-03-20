using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySpell : Spell {
    Vector3 originalScale;
    // Start is called before the first frame update

    private void Awake() {
        cooldownTime = 7;
        originalScale = transform.localScale;
        transform.localScale = new Vector3(0.1f, 0.1f, 1);
    }

    void Start() {
        damage = 0;
        transform.position = gunGameObject.transform.position;
        GetComponent<Rigidbody2D>().AddForce(getMouseVector() * 300);
        Destroy(gameObject, 2f * rangeMultiplier);
    }

    // Update is called once per frame
    void Update() {
        if (transform.localScale.x < originalScale.x) {
            transform.localScale = new Vector3(transform.localScale.x * 1.1f, transform.localScale.y * 1.1f, transform.localScale.z);
        } else {
            transform.localScale = originalScale;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        switch (other.transform.tag) {
            case "Enemy":
                print("ENTER");
                other.gameObject.GetComponent<Rigidbody2D>().AddForce((transform.position - other.transform.position) * 1000);
                break;
        }
    }
}
