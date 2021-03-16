using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySplitter : EnemyPathfinder {

    public float walkSpeed;
    public GameObject smallSplitter;
    public int numberOfSplits = 2;

    public GameObject testLoot;
    public float splitForce;

    // Start is called before the first frame update
    void Start() {
        collisionDamage = 1;
        currentHP = maxHP;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        followingPath = true;
        target = player.transform;
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

        rigidBody.AddForce(movementVector * walkSpeed);
    }

    IEnumerator SpawnChildren() {
        yield return new WaitForSeconds(0.7f);
        Vector3 splitVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f).normalized;
        for (int i = 0; i < numberOfSplits; i++) {
            Vector3 rotatedVector = Quaternion.Euler(0, 0, 360 / numberOfSplits * i) * splitVector;
            GameObject newSmallSplitter = Instantiate(smallSplitter, transform.position + rotatedVector * 0.3f, transform.rotation, transform.parent);
            newSmallSplitter.GetComponent<Rigidbody2D>().AddForce(splitForce * rotatedVector);
        }
        floor.GetComponent<RoomManager>().addEnemiesIntoRoom(numberOfSplits - 1);
    }

    public override void die() {
        GetComponentInChildren<Animator>().SetTrigger("Die");
        StopUpdatePath();
        GetComponent<Collider2D>().enabled = false;
        setToOriginalColor();

        IEnumerator spawnChildrenFunc = SpawnChildren();
        StartCoroutine(spawnChildrenFunc);
        GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Corpses";

        if (testLoot) {
            Instantiate(testLoot, transform.position, transform.rotation, transform.parent);
        }

        // Add one more enemy to room
        enabled = false;
        // Destroy(gameObject);
    }

}
