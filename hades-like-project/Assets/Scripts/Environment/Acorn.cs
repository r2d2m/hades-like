using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : Destructible
{
    

    void Start() {
        maxHP = 5;
        currentHP = maxHP;
        
    }

    // Update is called once per frame
    void Update()
    {
        updateCooldowns();
        BreakCheck();
    }
}
