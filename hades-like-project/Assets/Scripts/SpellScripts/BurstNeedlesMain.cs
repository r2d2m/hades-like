using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstNeedlesMain : Spell {
    // Start is called before the first frame update
    public GameObject burstNeedlesSub;

    float shootForce = 700;
    int nrOfNeedles = 25;
    float offsetAngle = 180;

    public BurstNeedlesMain() {
        manaCost = 55f;
        cooldownTime = 6f;
        isBasicAttack = false;
    }

    //public override float getManaCost() => manaCost * Mathf.Pow(0.9f, playerAgility);
    public override float getCooldownTime() => cooldownTime * Mathf.Pow(0.9f, playerAgility);

    void Start() {
        transform.position = gunGameObject.transform.position;
        setRotationTowardsVector(getMouseDeltaVector());

        Vector3 currentDir = Quaternion.Euler(0, 0, -offsetAngle) * getMouseDeltaVector().normalized;
        nrOfNeedles = nrOfNeedles + (int) (playerAgility / 2);
        for (int i = 0; i < nrOfNeedles; i++) {
            GameObject newNeedle = Instantiate(burstNeedlesSub, transform.position, transform.rotation);
            newNeedle.transform.parent = transform.parent;
            newNeedle.GetComponent<Rigidbody2D>().AddForce(currentDir * shootForce);
            BurstNeedlesSub spellScript = newNeedle.GetComponent<BurstNeedlesSub>();
            spellScript.moveVec = currentDir;
            spellScript.damage = 0.5f * damageMultiplier;

            spellScript.setPlayerStats(damageMultiplier, playerAgility, playerStrength, playerIntelligence);
            currentDir = Quaternion.Euler(0, 0, offsetAngle * 2 / nrOfNeedles) * currentDir;
        }
        Destroy(gameObject);
    }

}
