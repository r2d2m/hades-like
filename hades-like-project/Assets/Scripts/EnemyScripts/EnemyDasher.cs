using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDasher : Enemy {

    float dashCD;
    float currentDashCD;
    float dashForce;
    bool doDash;

    // Start is called before the first frame update
    void Start() {
        collisionDamage = 1;
        maxHP = 3;
        currentHP = maxHP;
        doDash = false;
        dashForce = 4000;
        dashCD = 2f;
        rewardSouls = 10;
        currentDashCD = dashCD;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        movement();
        updateCooldowns();
        deathCheck();
        //gameObject.GetComponentInChildren<Animator>().SetFloat("MoveSpeed", rigidBody.velocity.magnitude);
    }

    void movement() {
        if (currentDashCD <= 0) {
            doDash = true;
            currentDashCD = dashCD + Random.Range(-0.2f, 0.2f);
        } else {
            currentDashCD -= Time.deltaTime;
        }
    }

    private void FixedUpdate() {
        if (doDash) {
            movementVector = (player.transform.position - transform.position).normalized;
            if (Random.Range(0, 100) < 75) {
                rigidBody.AddForce(movementVector * dashForce);
            } else {
                rigidBody.AddForce(-movementVector * dashForce);
            }
            doDash = false;
        }
    }
}
