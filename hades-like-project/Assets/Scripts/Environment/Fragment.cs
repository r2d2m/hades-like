using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    protected float aliveTime = 1.0f;
    protected float fadeTime = 0.5f;
    protected float currentTime;
    protected SpriteRenderer spriteRenderer;
    protected Color fadeVector = new Color(1.0f,1.0f,1.0f,1.0f);

    void Start() {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        currentTime = 0.0f;
    }

    // Decrease alpha-value if aliveTime has passed. If aliveTime + fadeTime has passed, destroy object.
    void FixedUpdate() {
        currentTime += Time.deltaTime;
        
        if (currentTime > aliveTime && currentTime < aliveTime + fadeTime) {
            float fadePercent = Time.deltaTime/fadeTime;
            spriteRenderer.color -= fadePercent * fadeVector;
        } 
        else if (currentTime >= aliveTime + fadeTime) {
            Destroy(gameObject);
        }
    }
}
