using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    public int damage = 1;
    public float movementForce = 200;
    public float pushForce = 5;
    public float bulletLifeTime = 1;

    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, bulletLifeTime);
    }

    // Update is called once per frame
    void Update() {

    }


    public void setMovementVector(Vector3 movementVec) {
        GetComponent<Rigidbody2D>().AddForce(movementForce * movementVec.normalized);
    }

    public int getDamage() {
        return damage;
    }

    public void collideWithPlayer() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "player") {
        }

        switch (other.tag) {
            case "player":
                break;
            case "Enemy":
                break;
            case "playerbullet":
                break;
            default:
                Destroy(gameObject);
                break;

        }
    }
}
