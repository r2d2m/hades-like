using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharger : Enemy {

    float dashCD;
    float currentDashCD;
    float dashForce;
    bool doDash;
    Vector3 originalScale;

    // Start is called before the first frame update
    void Start() {
        collisionDamage = 1;
        maxHP = 10;
        currentHP = maxHP;
        doDash = false;
        dashForce = 5000;
        dashCD = 2f;
        rewardSouls = 10;
        currentDashCD = dashCD;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        originalScale = GetComponentInChildren<SpriteRenderer>().transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        movement();
        updateCooldowns();
        deathCheck();
    }

    void movement() {
        if (currentDashCD <= 0) {
            doDash = true;
            currentDashCD = dashCD + Random.Range(-0.2f, 0.2f);
            GetComponentInChildren<SpriteRenderer>().transform.localScale = originalScale;
        } else {
            currentDashCD -= Time.deltaTime;
            GetComponentInChildren<SpriteRenderer>().transform.localScale += new Vector3(0, 0.004f * Time.deltaTime, 0);
        }
    }

    private void FixedUpdate() {
        if (doDash) {
            movementVector = (player.transform.position - transform.position).normalized;
            rigidBody.AddForce(movementVector * dashForce);
            doDash = false;
        }
    }

}
