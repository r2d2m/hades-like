using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShotMain : Spell {
    // Start is called before the first frame update
    public GameObject burstNeedlesSub;

    float shootForce = 1000;

    private void Awake() {
        cooldownTime = 0.4f;
        isBasicAttack = false;
    }

    void Start() {
        transform.position = gunGameObject.transform.position;
        setRotationTowardsVector(getMouseDeltaVector());

        Vector3 currentDir = Quaternion.Euler(0, 0, -30) * getMouseDeltaVector().normalized;

        for (int i = 0; i < 10; i++) {
            GameObject newNeedle = Instantiate(burstNeedlesSub, transform.position, transform.rotation);
            newNeedle.transform.parent = transform.parent;
            newNeedle.GetComponent<Rigidbody2D>().AddForce(currentDir * shootForce);
            BurstNeedlesSub spellScript = newNeedle.GetComponent<BurstNeedlesSub>();
            spellScript.moveVec = currentDir;
            spellScript.damage = 0.1f * damageMultiplier;

            spellScript.setPlayerStats(damageMultiplier, playerAgility, playerStrength, playerIntelligence);
            currentDir = Quaternion.Euler(0, 0, 60 / 10) * currentDir;
        }
        Destroy(gameObject);
    }

}
