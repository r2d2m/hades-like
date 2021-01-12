using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBeamMain : Spell {
    // Start is called before the first frame update
    public GameObject waveBeamSub;

    private void Awake() {
        cooldownTime = 0.3f;
    }

    void Start() {
        transform.position = gunGameObject.transform.position;
        setRotationTowardsVector(getMouseDeltaVector());
        Destroy(gameObject, 0.1f);

        for (int i = 0; i < 2; i++) {
            GameObject newWave = Instantiate(waveBeamSub, transform.position, transform.rotation);
            newWave.transform.parent = transform.parent;
            WaveBeamSub spellScript = newWave.GetComponent<WaveBeamSub>();
            spellScript.mouseVector = getMouseVector();
            spellScript.damage = 1.5f * damageMultiplier;
            spellScript.waveDirection = (i == 0) ? 1 : -1;
            spellScript.setPlayerStats(damageMultiplier, rangeMultiplier, cooldownMultiplier);
        }
    }

}
