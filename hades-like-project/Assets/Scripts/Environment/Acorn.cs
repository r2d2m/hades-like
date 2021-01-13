using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : Destructible
{
    // Start is called before the first frame update
    void Start()
    {
        maxHP = 1;
        currentHP = maxHP;
        
    }

    // Update is called once per frame
    void Update()
    {
        updateCooldowns();
        BreakCheck();
    }
}
