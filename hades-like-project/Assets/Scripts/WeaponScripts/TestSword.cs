using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSword : Melee
{
    void Awake() {
        Destroy(gameObject, baseLifeTime);
    }
}
