using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuy : Enemy {
    // Start is called before the first frame update
    void Start() {
        collisionDamage = 1;
        maxHP = 80;
        currentHP = maxHP;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        movementStr = 1000;
    }

    // Update is called once per frame
    void Update() {
        updateCooldowns();
        deathCheck();
    }

    private void FixedUpdate() {
        movementVector = (player.transform.position - transform.position).normalized;
        rigidBody.AddForce(movementVector * movementStr);
    }

}
