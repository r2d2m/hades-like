using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Destructible : MonoBehaviour
{
    // Physics and logical
    public float maxHP;
    public float currentHP;
    public int collisionDamage = 0;
    protected Rigidbody2D rigidBody;
    
    // Estetic
    protected SpriteRenderer spriteRenderer;
    protected float shakeTime = 0.2f;
    protected float currentShakeTime = 0.0f;
    protected float shakeMagnitude = 0.05f;
    protected Vector3 shakeDirection; 
    
    // External
    public GameObject floor;
    public GameObject fracturedObject;
    public GameObject loot;
    public Sprite[] crackedSprites;


    void Awake() {
        floor = GameObject.FindGameObjectWithTag("Floor");
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        shakeDirection = new Vector3(1,0,0);
    }

    // Decrease HP and initiate shake-timer.
    public void TakeDamage(float damage) {
        if (damage > 0) {
            currentHP -= damage;
            currentShakeTime = shakeTime;
            
            if (crackedSprites != null && crackedSprites.Length > 0 && currentHP > 0) {
                UpdateSprite();
            }
        }
    }

    // Check if HP above 0.
    protected bool BreakCheck() {
        if (currentHP <= 0) {
            Break();
            return true;
        }
        return false;
    }

    // Destroys game object and instantiates a fractured version, with force and torque applied to its fragments.
    // Also instantiates loot item if available.
    public void Break() {
        GameObject fracturedInstance = Instantiate(fracturedObject,transform.position,Quaternion.identity);
        EdgeCollider2D[] edgeColliders = fracturedInstance.GetComponentsInChildren<EdgeCollider2D>();
        
        float forceMagnitude = 100f;
        float torqueDirection = 1;
        float toqrueMagnitude = 100f;

        if (edgeColliders.Length > 0) {
            foreach (var collider in edgeColliders) {
                
                Vector2 forceDirection = GetAverage(collider.points).normalized;
                torqueDirection = torqueDirection*-1;
                
                collider.attachedRigidbody.AddForce(forceDirection * forceMagnitude);
                collider.attachedRigidbody.AddTorque(torqueDirection * toqrueMagnitude);
            }
        }
        
        Destroy(gameObject);

        float xsize = transform.localScale.x;
        float ysize = transform.localScale.y;
        floor.GetComponent<Grid>().UpdateArea(transform.position,xsize,ysize);

        if (loot != null) {
            Instantiate(loot,transform.position,Quaternion.identity);
        }
            

    }

    // Return average x and y-values from input Vector2 array.
    private Vector2 GetAverage(Vector2[] points) {
        Vector2 avg = new Vector2(0,0);
        foreach (Vector2 v in points) {
            avg.x += v.x;
            avg.y += v.y;
        }
        avg.x = avg.x/points.Length;
        avg.y = avg.y/points.Length;
        return avg;
    }

    // Updates sprite when damage is taken.
    private void UpdateSprite() {
        int spriteIndex = Convert.ToInt32((maxHP - currentHP)/(maxHP-1) * crackedSprites.Length)-1;

        if (spriteIndex >= 0) {
            Sprite nextSprite = crackedSprites[spriteIndex];
            spriteRenderer.sprite = nextSprite;
        }

    }

    // Update shake-cooldown and move object if timer is set.
    protected void updateCooldowns() {

        if (currentShakeTime > 0) {
            transform.Translate(shakeDirection * shakeMagnitude);
            shakeDirection *= -1;
            currentShakeTime -= Time.deltaTime;
        } else {
            currentShakeTime = 0;

            if (shakeDirection.x < 0) {
                transform.Translate(shakeDirection * shakeMagnitude);
                shakeDirection *= -1;
            }
        }
    }
}
