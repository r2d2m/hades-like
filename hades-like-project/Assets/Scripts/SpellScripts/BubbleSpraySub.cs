using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpraySub : Spell {
    // Start is called before the first frame update
    float maxScale;
    public GameObject bubblePop;

    void Start() {
          destroySelfWithDelay(Random.Range(4f, 5f) * rangeMultiplier);
    
        transform.localScale = Vector3.one * 0.2f;
        maxScale = Random.Range(1.2f, 1.8f);
    }

    void Update() {
        if (transform.localScale.x < maxScale) {
            transform.localScale = transform.localScale + Vector3.one * 2.5f * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.transform.tag) {
            case "Enemy":
                other.gameObject.GetComponent<Enemy>().takeDamage(damage);
                destroySelfAndTriggerAnim();
                break;
            case "Destructible":
                other.gameObject.GetComponent<Destructible>().TakeDamage(damage);
                destroySelfAndTriggerAnim();
                break;
        }
    }
}
