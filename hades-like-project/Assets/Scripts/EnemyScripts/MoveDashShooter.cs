using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDashShooter : EnemyPathfinder {
    // Start is called before the first frame update
    float dashSpeed;
    float maxDashCooldown = 2.0f;
    float currentDashCooldown;
    float bulletForce = 620.0f;
    public GameObject projectilePrefab;
    int nrOfProjectiles = 5;

    void Start() {
        //setChaseRange(5);
        dashSpeed = 5000;
        collisionDamage = 1;
        maxHP = 20;
        currentHP = maxHP;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        movementStr = 50;
        target = player.transform;
        followingPath = true;
        rewardSouls = 50;
        StartUpdatePath();
    }

    IEnumerator shootAtPlayer() {
        for (int i = 0; i < nrOfProjectiles; i++) {
            GameObject newProj = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, 0));
            newProj.GetComponent<EnemyBullet>().setMovementVector((Vector2)(player.transform.position - transform.position).normalized, bulletForce);
            newProj.GetComponent<EnemyBullet>().setBulletLifeTime(1.3f);
            newProj.transform.localScale = newProj.transform.localScale * 1.3f;
            yield return new WaitForSeconds(0.15f);
        }
    }

    // Update is called once per frame
    void Update() {
        updateCooldowns();
        deathCheck();
        chaseRangeCheck();
        currentDashCooldown -= Time.deltaTime;
        if (currentDashCooldown <= 0) {
            float randomFloat = Random.Range(0.0f, 1.0f);
            if (randomFloat < 0.33f) {
                movementVector = Random.insideUnitCircle.normalized;
            } else if (randomFloat < 0.66f) {
                movementVector = (Vector2)(player.transform.position - transform.position).normalized * 1.3f;
            } else {
                IEnumerator shootFunc = shootAtPlayer();
                StartCoroutine(shootFunc);
                movementVector = new Vector3(0, 0, 0);
            }
            rigidBody.AddForce(movementVector * dashSpeed);
            currentDashCooldown = maxDashCooldown + Random.Range(-0.5f, 0.5f);
        }
    }

    private void FixedUpdate() {
        //        print(currentState);
    }
}