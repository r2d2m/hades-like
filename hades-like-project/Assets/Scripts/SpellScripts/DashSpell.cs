using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSpell : Spell {

    public DashSpell() {
        manaCost = 55f;
        cooldownTime = 2;
        isBasicAttack = false;
    }
    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, 0.1f);
    }

    // Update is called once per frame
    void Update() {
        playerGameObject.GetComponent<Rigidbody2D>().velocity =  getMouseDeltaVector().normalized * (1000 + playerAgility * 50);
    }
}
