using System.Collections;
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

    protected bool deathCheck() {
        if (currentHP <= 0) {
            die();
            return true;
        }
        return false;
    }

    GameObject getPlayer() {
        return player;
    }

    // 
    public void takeDamage(float damage) {
        currentHP -= damage;
    }

    public void heal() {

    }

    public int getCollisionDamage() {
        return collisionDamage;
    }

    public void die() {
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
