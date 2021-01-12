using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpell3 : Spell {
    // Start is called before the first frame update

    private void Awake() {
        damage = 1 * damageMultiplier;
        cooldownTime = 0.4f;
    }

    void Start() {
        transform.position = gunGameObject.transform.position;
        GetComponent<Rigidbody2D>().AddForce(getMouseVector() * 1000 * rangeMultiplier);
        setRotationTowardsVector(getMouseDeltaVector());
        Destroy(gameObject, 0.7f);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.transform.tag) {
            case "Enemy":
                other.gameObject.GetComponent<Enemy>().takeDamage(damage);
                Destroy(gameObject);
                break;
        }
    }
}
