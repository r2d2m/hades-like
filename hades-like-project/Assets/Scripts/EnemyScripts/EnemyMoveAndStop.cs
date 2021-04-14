using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAndStop : Enemy {

    float walkCD;
    float currentWalkCD;
    float walkForce;
    float walkDuration;
    float currentWalkDuration;
    bool doWalk;

    // Start is called before the first frame update
    void Start() {
        collisionDamage = 1;
        maxHP = 3;
        currentHP = maxHP;
        doWalk = false;
        walkForce = 150f;
        walkCD = 0.3f;
        walkDuration = 2f;
        rewardSouls = 10;
        currentWalkCD = walkCD;
        currentWalkDuration = walkDuration;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        movement();
        updateCooldowns();
        deathCheck();
        gameObject.GetComponentInChildren<Animator>().SetFloat("MoveSpeed", rigidBody.velocity.magnitude);
    }

    void movement() {
        if (doWalk) {
            currentWalkCD -= Time.deltaTime;
            if (currentWalkCD <= 0) {
                doWalk = false;
                currentWalkDuration = walkDuration;
            }
        } else {
            currentWalkDuration -= Time.deltaTime;
            if (currentWalkDuration <= 0) {
                doWalk = true;
                if (Random.Range(0.0f, 1.0f) > 0.5f) {
                    movementVector = Random.insideUnitCircle.normalized;
                }else{
                    movementVector = (player.transform.position - transform.position).normalized;
                }
                currentWalkCD = walkCD;
            }
        }
    }

    private void FixedUpdate() {
        if (doWalk) {
            rigidBody.AddForce(movementVector * 400);
        }
    }
}
