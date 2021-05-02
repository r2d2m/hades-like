using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    protected int damage = 1;
    protected float movementForce = 200;
    protected float pushForce = 5;
    protected float bulletLifeTime = 1;

    public void setMovementVector(Vector3 movementVec, float moveForce = 1) {
        GetComponent<Rigidbody2D>().AddForce(moveForce * movementVec.normalized);
    }

    public int getDamage() {
        return damage;
    }

    public void collideWithPlayer() {
        Destroy(gameObject);
    }

    public void setDamage(int newDamage) {
        damage = newDamage;
    }

    public void setBulletLifeTime(float newLifeTime) {
        bulletLifeTime = newLifeTime;
    }

    public void setMovementForce(float newForce) {
        movementForce = newForce;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "player") {
        }

        switch (other.tag) {
            case "player":
                break;
            case "Enemy":
                break;
            case "PlayerSpell":
                break;
            default:
                //Destroy(gameObject);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {

        switch (other.transform.tag) {
            case "Unwalkable":
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;

        }
    }
}
