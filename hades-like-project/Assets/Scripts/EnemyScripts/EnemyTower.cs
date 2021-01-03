using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTower : Enemy {

    public float shootCD;
    public float currentShootCD;
    public float projectileForce = 1;
    public float range = 5;

    public GameObject bullet;

    // Start is called before the first frame update
    void Start() {
        collisionDamage = 0;
        maxHP = 3;
        currentHP = maxHP;
        currentShootCD = shootCD;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {

        Vector3 deltaVec = player.transform.position - transform.position;
        if(currentShootCD <= 0 && deltaVec.magnitude < range){
            shoot(deltaVec);
            currentShootCD = shootCD;
        } else {
            currentShootCD -= Time.deltaTime;
        }

        deathCheck();
    }


    void shoot(Vector3 deltaVector) {
        if (bullet != null) {
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 0));
            Vector3 bulletVec = (deltaVector).normalized;
            newBullet.GetComponent<EnemyBullet>().setMovementVector(projectileForce * bulletVec);
        } else {
            print("Missing bullet prefab!");
        }
    }

    private void FixedUpdate() {

    }
}
