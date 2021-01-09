﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Parent class for all enemies, should include things that all enemies share!
*/
public class Enemy : MonoBehaviour {
    public GameObject corpsePrefab;
    public GameObject floor;

    public float maxHP;
    public float currentHP;
    protected GameObject player;
    public int collisionDamage = 0;
    protected Vector3 movementVector;
    protected Rigidbody2D rigidBody;
    protected float movementStr;
    protected Color originalColor;
    protected SpriteRenderer spriteRenderer;

    protected float colorTime = 0.07f;
    protected float currentColorTime = 0.0f;

    // Start is called before the first frame update
    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        floor = GameObject.FindGameObjectWithTag("Floor");
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // Check if enemy is dead
    protected bool deathCheck() {
        if (currentHP <= 0) {
            die();
            return true;
        }
        return false;
    }

    // Get the player gameobject
    GameObject getPlayer() {
        return player;
    }

    // Update things as current color etc
    protected void updateCooldowns() {
        if (currentColorTime > 0) {
            currentColorTime -= Time.deltaTime;
        } else {
            currentColorTime = 0;
            spriteRenderer.color = originalColor;
        }
    }

    // Take damage and set color
    public void takeDamage(float damage) {
        currentHP -= damage;
        currentColorTime = colorTime;
        spriteRenderer.color = new Color(1.0f, 0.0f, 0.0f);
    }

    private void setToOriginalColor() {
        spriteRenderer.color = originalColor;
    }

    public void heal() {

    }

    public int getCollisionDamage() {
        return collisionDamage;
    }

    // Things to do and set during deah
    public virtual void die() {
        // Spawn corpse if we have a prefab
        if (corpsePrefab != null) {
            GameObject corpse = Instantiate(corpsePrefab, transform.position, transform.rotation);
            corpse.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            corpse.transform.parent = transform.parent; // TODO CHANGE THIS!
            //corpse.GetComponent<SpriteRenderer>().color = originalColor;
        }

        floor.GetComponent<RoomManager>().enemyDeath();
        Destroy(gameObject);
    }
}
