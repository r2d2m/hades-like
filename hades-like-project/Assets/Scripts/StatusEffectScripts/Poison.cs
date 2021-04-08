using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poison : StatusEffect {

    public float DPS;
    private float waitingTime;
    private Enemy enemyScript;

     void Start() {
        enemyScript = target.GetComponent<Enemy>();
        StartCoroutine(PosionDamage());
    }

    void Update() {
        UpdateDuration();
    }

    IEnumerator PosionDamage() {


        while (elapsedTime < durationTime) {
            enemyScript.currentHP -= Time.deltaTime*DPS;
            yield return null;
        }

        Destroy(gameObject);
            
    }
    
}
