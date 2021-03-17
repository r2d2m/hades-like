using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpell3 : Spell {
    // Start is called before the first frame update

    private void Awake() {
        cooldownTime = 0.4f;
        isBasicAttack = true;
    }

    void Start() {
        damage = 1 * damageMultiplier;
        transform.position = playerPos;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        GetComponent<Rigidbody2D>().AddTorque(20);
        GetComponent<Rigidbody2D>().AddForce(getMouseVector() * 1000 * speedMultiplier);
        Destroy(gameObject, 0.7f * lifeTimeMultiplier);
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
