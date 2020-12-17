using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashShooter : Enemy {

    public float dashCD = 0.8f;
    float currentDashCD;
    public float dashStrength = 2000;

    public float shootCD;
    public float currentShootCD;
    bool doDash;

    public GameObject bullet;

    // Start is called before the first frame update
    void Start() {
        collisionDamage = 1;
        maxHP = 3;
        currentHP = maxHP;
        doDash = false;
        currentDashCD = dashCD;
        shootCD = 1.0f;
        currentShootCD = shootCD;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        cooldowns();
        deathCheck();
    }

    void cooldowns() {
        if (currentDashCD <= 0) {
            doDash = true;
            currentDashCD = dashCD + Random.Range(-0.2f, 0.2f);
        } else {
            currentDashCD -= Time.deltaTime;
        }

        if(currentShootCD <= 0){
            shoot();
           currentShootCD = shootCD + Random.Range(-0.2f, 0.2f);
        } else {
            currentShootCD -= Time.deltaTime;
        }
    }

    void shoot() {
        if (bullet != null) {
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 0));
            Vector3 bulletVec = (player.transform.position - transform.position).normalized;
            newBullet.GetComponent<EnemyBullet>().setMovementVector(bulletVec);
        } else {
            print("Missing bullet prefab!");
        }
    }

    private void FixedUpdate() {
        if (doDash) {
            movementVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            movementVector = movementVector.normalized;
            if (Random.Range(0, 100) < 75) {
                rigidBody.AddForce(movementVector * dashStrength);
            } else {
                rigidBody.AddForce(-movementVector * dashStrength);
            }
            doDash = false;
        }
    }

}
