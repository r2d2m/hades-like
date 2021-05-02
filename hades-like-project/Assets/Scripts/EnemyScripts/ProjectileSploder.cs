using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSploder : Enemy {
    float shootForce = 400;
    public int nrOfProj;
    public GameObject projectilePrefab;
    int projectileDamage;
    Vector2 movementVec;
    float maxMovementTimer = 2.5f;
    float currentMovementTimer;
    float movementForce = 150;

    // Start is called before the first frame update
    void Start() {
        currentMovementTimer = maxMovementTimer;
        collisionDamage = 1;
        projectileDamage = 1;
        maxHP = 3;
        currentHP = maxHP;
        rewardSouls = 10;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        movementVec = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update() {
        movement();
        updateCooldowns();
        deathCheck();
    }

    void movement() {
        currentMovementTimer -= Time.deltaTime;
        if (currentMovementTimer <= 0) {
            movementVec = Random.insideUnitCircle.normalized;
            currentMovementTimer = maxMovementTimer + Random.Range(-0.5f, 0.5f);
        }
    }

    private void FixedUpdate() {
        rigidBody.AddForce(movementForce * movementVec);
    }

    public override void die() {
        Vector3 currentDir = new Vector3(0, 1, 0);
        for (int i = 0; i < nrOfProj; i++) {
            GameObject newProj = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, 0));
            newProj.GetComponent<EnemyBullet>().setMovementForce(shootForce);
            newProj.GetComponent<EnemyBullet>().setMovementVector(currentDir.normalized, shootForce);
            newProj.GetComponent<EnemyBullet>().setBulletLifeTime(2);
            currentDir = Quaternion.Euler(0, 0, 360 / nrOfProj) * currentDir;
            newProj.transform.parent = floor.transform;
        }
        base.die();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Vector2 collisionPoint = other.contacts[0].point;
        Vector2 normal = ((Vector2)transform.position - collisionPoint).normalized;
        movementVec = Vector2.Reflect(movementVec, normal);
    }
}
