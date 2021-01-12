using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon {
    public float hitboxTime;
    Vector2 deltaVector; 

    public void setDeltaVector(Vector2 deltaVector){
        this.deltaVector = deltaVector;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Collide with enemies
        // TODO: Damage multiple types of enemies!
        if (other.transform.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy>().takeDamage(weaponDamage);
            other.gameObject.GetComponent<Rigidbody2D>().velocity = deltaVector * enemyPushForce;
        }
    }
}
