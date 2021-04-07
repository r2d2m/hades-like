using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSploder : Enemy {
    float shootForce = 800;
    public int nrOfProj;
    public GameObject projectilePrefab;
    int projectileDamage;
    Vector2 movementVec;

    // Start is called before the first frame update
    void Start() {
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
        movementVec = new Vector2(0, 0);
    }

    private void FixedUpdate() {

    }

    public override void die() {
        Vector3 currentDir = new Vector3(0, 1, 0);
        for (int i = 0; i < nrOfProj; i++) {
            GameObject newProj = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, 0));
            newProj.GetComponent<EnemyBullet>().setMovementVector(currentDir.normalized * shootForce);
            newProj.GetComponent<EnemyBullet>().bulletLifeTime = 2;
            currentDir = Quaternion.Euler(0, 0, 360 / nrOfProj) * currentDir;
            newProj.transform.parent = floor.transform;
        }
        base.die();
    }
}
