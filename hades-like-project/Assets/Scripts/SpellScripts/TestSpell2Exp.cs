using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpell2Exp : Spell {

    void Start() {
        transform.localScale = new Vector3(0.1f * rangeMultiplier, 0.1f * rangeMultiplier, 0);
        Destroy(gameObject, 0.3f);
    }

    // Update is called once per frame
    void Update() {
        transform.localScale += new Vector3(0.05f * rangeMultiplier, 0.05f * rangeMultiplier, 0);
    }

}
