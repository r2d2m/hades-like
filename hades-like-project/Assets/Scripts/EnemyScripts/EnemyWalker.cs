using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : Enemy {

    public float walkSpeed;

    // Start is called before the first frame update
    void Start() {
        currentHP = maxHP;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        updateCooldowns();
        deathCheck();
    }

    private void FixedUpdate() {
        movementVector = (player.transform.position - transform.position).normalized;
        rigidBody.AddForce(walkSpeed * movementVector);
    }
}
