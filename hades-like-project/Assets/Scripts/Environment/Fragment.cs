using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    protected float aliveTime = 3.0f;
    protected float fadeTime = 1.0f;
    protected float currentTime;
    protected SpriteRenderer spriteRenderer;
    protected Color alphaVector;

    void Start() {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        currentTime = 0.0f;
        alphaVector = new Color(0,0,0,1.0f);
    }

    // Decrease alpha-value if aliveTime has passed. If aliveTime + fadeTime has passed, destroy object.
    void Update() {
        currentTime += Time.deltaTime;
        
        if (currentTime > aliveTime && currentTime < aliveTime + fadeTime) {
            float fadePercent = Time.deltaTime/fadeTime;
            spriteRenderer.color -= fadePercent * alphaVector;
        } 
        else if (currentTime >= aliveTime + fadeTime) {
            Destroy(gameObject);
        }
    }
}
