using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupMember : EnemyPathfinder {

    public Enemy[] dependencies;
    public Animator animator;
    protected float revivalTimer;
    public float deathTime;
    

    void Start() {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        movementStr = 150;
        target = player.transform;
        initialState();
    }

    void initialState() {
        currentHP = maxHP;
        revivalTimer = 0;
        followingPath = true;
        StartUpdatePath();
    }

    void Update() {
        updateCooldowns();
        if (currentState != EnemyStates.DYING) {
            deathCheck();
        }
        
    }

    private void FixedUpdate() {
        
        if (currentState == EnemyStates.CHASING) {
            movementVector = GetPathVector(transform.position);
            rigidBody.AddForce(movementVector * movementStr);
        } 
        
        
    }

    IEnumerator waitForRevival() {
        while (revivalTimer < deathTime) {
            revivalTimer += Time.deltaTime;
            yield return null;
        }
        
        if (doRevive()) 
            revive();
        else 
            trueDeath();
        
    }

    private bool doRevive() {
        foreach(Enemy friend in dependencies) {
           if (friend != null && friend.currentState != EnemyStates.DYING) {
               return true;
            }
        }
        return false;
    }

    private void revive() {
        StopCoroutine(waitForRevival());
        initialState();
        animator.SetTrigger("Chasing");
        currentState = EnemyStates.CHASING;
    }

    void trueDeath() {
        if (corpsePrefab != null) {
            GameObject corpse = Instantiate(corpsePrefab, transform.position, transform.rotation);
            corpse.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            corpse.transform.parent = transform.parent; 
        }

        Destroy(gameObject);
        
    }

    public override void die() {
        currentState = EnemyStates.DYING;
        animator.SetTrigger("Dying");
        StartCoroutine(waitForRevival());
        

    }
}
