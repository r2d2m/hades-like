using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpell1 : Spell {

    public TestSpell1() {
        manaCost = 55f;
        cooldownTime = 10;
        isBasicAttack = false;
    }

    // Start is called before the first frame update
    void Start() {
        damage = 2 * damageMultiplier;
        transform.localScale = new Vector3(0.1f, 0.1f, 0);
        transform.position = mousePos;

        Destroy(gameObject, 0.3f * rangeMultiplier);
    }

    // Update is called once per frame
    void Update() {
        transform.localScale += new Vector3(0.05f, 0.05f, 0);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.transform.tag) {
            case "Enemy":
                other.gameObject.GetComponent<Enemy>().takeDamage(damage);
                break;
        }
    }
}
