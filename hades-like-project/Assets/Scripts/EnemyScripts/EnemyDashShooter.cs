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

    public GameObject[] hands;
    float shootRange = 7.5f;
    public GameObject bullet;

    bool playerToRight = true;

    // Start is called before the first frame update
    void Start() {
        collisionDamage = 1;
        maxHP = 7;
        currentHP = maxHP;
        doDash = false;
        currentDashCD = dashCD;
        shootCD = 1.0f;
        rewardSouls = 10;
        currentShootCD = shootCD;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        handChecks();   
        cooldowns();
        updateCooldowns();
        deathCheck();
    }

    void handChecks() {
        if (player.transform.position.x < transform.position.x && playerToRight == true) {
            playerToRight = false;
            Vector3 temp = hands[0].transform.position;
            hands[0].transform.position = hands[1].transform.position;
            hands[1].transform.position = temp;
            foreach (GameObject hand in hands) {
                hand.transform.localScale = new Vector3(hand.transform.localScale.x, -1.3f, 1);
            }
        } else if (player.transform.position.x >= transform.position.x && playerToRight == false) {
            playerToRight = true;
            Vector3 temp = hands[0].transform.position;
            hands[0].transform.position = hands[1].transform.position;
            hands[1].transform.position = temp;
            foreach (GameObject hand in hands) {
                hand.transform.localScale = new Vector3(hand.transform.localScale.x, 1.3f, 1);
            }
        }

        foreach (GameObject hand in hands) {
            hand.transform.right = (Vector2)(player.transform.position - hand.transform.position);
        }
    }

    void cooldowns() {
        if (currentDashCD <= 0) {
            doDash = true;
            currentDashCD = dashCD + Random.Range(-0.2f, 0.2f);
        } else {
            currentDashCD -= Time.deltaTime;
        }

        if (currentShootCD <= 0 && (transform.position - player.transform.position).magnitude < shootRange) {
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
            newBullet.GetComponent<EnemyBullet>().setMovementVector(bulletVec, 400);
        } else {
            print("Missing bullet prefab!");
        }
    }

    public override void die() {
        foreach (GameObject hand in hands) {
            hand.GetComponentInChildren<Animator>().SetTrigger("Die");
        }
        base.die();
    }

    private void FixedUpdate() {
        if (doDash) {
            /*
            if ((transform.position - player.transform.position).magnitude < shootRange) {
                movementVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
                movementVector = movementVector.normalized;

            } else {
                movementVector = player.transform.position - transform.position;
                movementVector = movementVector.normalized;
            }
            */

            movementVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            movementVector = movementVector.normalized;

            rigidBody.AddForce(movementVector * dashStrength);
            doDash = false;
        }
    }

}
