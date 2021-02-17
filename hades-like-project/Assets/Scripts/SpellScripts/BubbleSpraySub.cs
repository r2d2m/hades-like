using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpraySub : Spell {
    // Start is called before the first frame update
    float maxScale;

    void Start() {
        Destroy(gameObject, Random.Range(0.7f, 2f) * rangeMultiplier);
        transform.localScale = Vector3.one * 0.2f;
        maxScale = Random.Range(0.8f, 1.2f);
    }

    void Update() {
        if (transform.localScale.x < maxScale) {
            transform.localScale = transform.localScale + Vector3.one * 0.025f;
        }
    }


    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.transform.tag) {
            case "Enemy":
                other.gameObject.GetComponent<Enemy>().takeDamage(damage);
                Destroy(gameObject);
                break;
            case "Destructible":
                other.gameObject.GetComponent<Destructible>().TakeDamage(damage);
                Destroy(gameObject);
                break;
        }
    }
}
