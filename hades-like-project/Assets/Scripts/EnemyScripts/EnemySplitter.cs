using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySplitter : Enemy {

    public float walkSpeed;
    public GameObject smallSplitter;
    public int numberOfSplits = 2;

    public float splitForce;

    // Start is called before the first frame update
    void Start() {
        currentHP = maxHP;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        deathCheck();
    }


    private void FixedUpdate() {
        movementVector = (player.transform.position - transform.position).normalized;
        rigidBody.AddForce(walkSpeed * movementVector);
    }

    public override void die() {
        // Spawn corpse if we have a prefab
        if (corpsePrefab != null) {
            GameObject corpse = Instantiate(corpsePrefab, transform.position, transform.rotation);
            corpse.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            corpse.transform.parent = transform.parent; // TODO CHANGE THIS!
            //corpse.GetComponent<SpriteRenderer>().color = originalColor;
        }
        
        Vector3 splitVector = new Vector3(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f), 0.0f).normalized;

        for(int i = 0; i < numberOfSplits; i++){
            Vector3 rotatedVector = Quaternion.Euler(0, 0, 360/numberOfSplits * i) * splitVector;
            GameObject newSmallSplitter = Instantiate(smallSplitter, transform.position + rotatedVector * 0.3f, transform.rotation);
            print(rotatedVector);
            newSmallSplitter.GetComponent<Rigidbody2D>().AddForce(splitForce * rotatedVector);
        }

        //floor.GetComponent<RoomManager>().enemyDeath();
        // Add one more enemy to room!
        floor.GetComponent<RoomManager>().addEnemiesIntoRoom(numberOfSplits - 1);
        Destroy(gameObject);
    }

}
