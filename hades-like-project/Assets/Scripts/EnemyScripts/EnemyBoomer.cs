using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomer : EnemyPathfinder {

    public float walkSpeed;
    public GameObject poisonField;
    public float poisonLifeTime = 5.0f;

    // Start is called before the first frame update
    void Start() {
        collisionDamage = 1;
        currentHP = maxHP;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        followingPath = true;
        target = player.transform;
        movementStr = 200;
        rewardSouls = 30;
        StartUpdatePath();
    }

    // Update is called once per frame
    void Update() {
        updateCooldowns();
        deathCheck();
    }

    private void FixedUpdate() {
        if (followingPath) {
            movementVector = GetPathVector(transform.position);
        } else {
            movementVector = (player.transform.position - transform.position).normalized;
        }

        rigidBody.AddForce(movementVector * movementStr);
    }

    public override void die() {
        // Spawn corpse if we have a prefab
        if (corpsePrefab != null) {
            GameObject corpse = Instantiate(corpsePrefab, transform.position, transform.rotation);
            corpse.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            corpse.transform.parent = transform.parent; // TODO CHANGE THIS!
            //corpse.GetComponent<SpriteRenderer>().color = originalColor;
        }

        GameObject poison = Instantiate(poisonField, transform.position, transform.rotation, transform.parent);
        poison.GetComponent<PoisonField>().setValues(1, 5.0f);
        StopUpdatePath();
        floor.GetComponent<RoomManager>().enemyDeath(transform.position, rewardSouls);

        Destroy(gameObject);
    }

}
