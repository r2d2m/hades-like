using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstNeedlesMain : Spell {
    // Start is called before the first frame update
    public GameObject burstNeedlesSub;

    float shootForce = 700;
    int nrOfNeedles = 25;
    float offsetAngle = 180;

    private void Awake() {
        cooldownTime = 6f;
        isBasicAttack = false;
    }

    void Start() {
        transform.position = gunGameObject.transform.position;
        setRotationTowardsVector(getMouseDeltaVector());

        Vector3 currentDir = Quaternion.Euler(0, 0, -offsetAngle) * getMouseDeltaVector().normalized;

        for (int i = 0; i < nrOfNeedles; i++) {
            GameObject newNeedle = Instantiate(burstNeedlesSub, transform.position, transform.rotation);
            newNeedle.transform.parent = transform.parent;
            newNeedle.GetComponent<Rigidbody2D>().AddForce(currentDir * shootForce);
            BurstNeedlesSub spellScript = newNeedle.GetComponent<BurstNeedlesSub>();
            spellScript.moveVec = currentDir;
            spellScript.damage = 0.5f * damageMultiplier;

            spellScript.setPlayerStats(damageMultiplier, rangeMultiplier, cooldownMultiplier, speedMultiplier, lifeTimeMultiplier, forceMultipler);
            currentDir = Quaternion.Euler(0, 0, offsetAngle * 2 / nrOfNeedles) * currentDir;
        }
        Destroy(gameObject);
    }

}
