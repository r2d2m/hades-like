using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpell1 : Spell {

    private void Awake() {
        damage = 2 * damageMultiplier;    
        cooldownTime = 2;
    }
    
    // Start is called before the first frame update
    void Start() {
        transform.localScale = new Vector3(0.1f, 0.1f, 0);
        Destroy(gameObject, 0.3f);
    }

    // Update is called once per frame
    void Update() {
        transform.position = mousePos;
        transform.localScale += new Vector3(0.05f, 0.05f, 0);
    }
}
