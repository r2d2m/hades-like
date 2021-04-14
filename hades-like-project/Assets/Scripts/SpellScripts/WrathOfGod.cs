using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathOfGod : Spell {
    Vector3 originalScale;
    // Start is called before the first frame update

    private void Awake() {
        cooldownTime = 7;
        //originalScale = transform.localScale;
        //transform.localScale = new Vector3(0.1f, 0.1f, 1);
    }

    public WrathOfGod() {
        manaCost = 50f;
        cooldownTime = 7f;
        isBasicAttack = false;
    }

    void Start() {
        damage = 1 * damageMultiplier;
        transform.localScale *= rangeMultiplier;
        transform.position = playerGameObject.transform.position;
        Destroy(gameObject, 0.3f);
    }

    private void Update() {
        transform.position = playerGameObject.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Unwalkable")) {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.transform.tag) {
            case "Enemy":
                other.gameObject.GetComponent<Rigidbody2D>().AddForce((other.transform.position - transform.position).normalized * 8000);
                other.gameObject.GetComponent<Enemy>().takeDamage(damage);
                break;
            case "EnemyBullet":
                other.gameObject.GetComponent<Rigidbody2D>().velocity *= -2;
                break;
        }
    }
}
