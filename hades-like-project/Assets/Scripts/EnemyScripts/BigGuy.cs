using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuy : EnemyPathfinder {
    // Start is called before the first frame update
    void Start() {
        collisionDamage = 1;
        maxHP = 80;
        currentHP = maxHP;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        movementStr = 1000;
        target = player.transform;
        followingPath = true;
        StartCoroutine (UpdatePath ());
    }

    // Update is called once per frame
    void Update() {
        updateCooldowns();
        deathCheck();
    }

    private void FixedUpdate() {
        
        if (followingPath) {
            movementVector = GetPathVector(transform.position);
        } else {
            movementVector = (player.transform.position - transform.position).normalized;
        }
        
        rigidBody.AddForce(movementVector * movementStr);
    }

}
