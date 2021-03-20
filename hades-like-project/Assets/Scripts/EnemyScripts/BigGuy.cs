using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuy : EnemyPathfinder {
    // Start is called before the first frame update

    void Start() {
        setChaseRange(5);

        collisionDamage = 1;
        maxHP = 5;
        currentHP = maxHP;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        movementStr = 1000;
        target = player.transform;
        followingPath = true;
        rewardSouls = 50;
        StartUpdatePath();
    }

    // Update is called once per frame
    void Update() {
        updateCooldowns();
        deathCheck();
        chaseRangeCheck();
    }

    private void FixedUpdate() {
//        print(currentState);
        if (currentState == EnemyStates.CHASING) {
            if (followingPath) {
                movementVector = GetPathVector(transform.position);
            } else {
                movementVector = (player.transform.position - transform.position).normalized;
            }

        }

        rigidBody.AddForce(movementVector * movementStr);
    }
}