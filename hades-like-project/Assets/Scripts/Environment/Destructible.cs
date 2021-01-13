using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    // Physics and logical
    public float maxHP;
    public float currentHP;
    public int collisionDamage = 0;
    protected Rigidbody2D rigidBody;
    
    // Estetic
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;
    protected float colorTime = 0.07f;
    protected float currentColorTime = 0.0f;
    
    // External
    public GameObject floor;

    void Awake() {
        floor = GameObject.FindGameObjectWithTag("Floor");
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(float damage) {
        if (damage > 0) {
            currentHP -= damage;
            currentColorTime = colorTime;
            spriteRenderer.color = new Color(1.0f, 0.0f, 0.0f);
            
        }
    }

    protected bool BreakCheck() {
        if (currentHP <= 0) {
            Break();
            return true;
        }
        return false;
    }

    public void Break() {
        Destroy(gameObject);
        float xsize = transform.localScale.x;
        float ysize = transform.localScale.y;
        floor.GetComponent<Grid>().UpdateArea(transform.position,xsize,ysize);
        
    }

    protected void updateCooldowns() {
        if (currentColorTime > 0) {
            currentColorTime -= Time.deltaTime;
        } else {
            currentColorTime = 0;
            spriteRenderer.color = originalColor;
        }
    }
}
