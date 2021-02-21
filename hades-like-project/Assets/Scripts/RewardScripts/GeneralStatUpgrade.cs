using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStatUpgrade : Reward {
    public float damageMultiplier = 0;
    public float cooldownMultiplier = 0;
    public float rangeMultiplier = 0;
    public float spellSpeedMultiplier = 0;
    public float spellForceMultipler = 0;
    public float spellLifeTimeMultiplier = 0;
    public float movementSpeedMultiplier = 0;
    public int maxHealthMod = 0;


    // Update is called once per frame
    public override void grabReward(GameObject player) {
        PlayerMain playerMain = player.GetComponent<PlayerMain>();
        playerMain.modifyPlayerDamageMultiplier(damageMultiplier);
        playerMain.modifyPlayerHealth(maxHealthMod);
        playerMain.modifyCooldown(cooldownMultiplier);
        playerMain.modifyPlayerSpeed(movementSpeedMultiplier);
        playerMain.modifySpellForce(spellForceMultipler);
        playerMain.modifySpellSpeed(spellSpeedMultiplier);
        playerMain.modifyLifeTime(spellLifeTimeMultiplier);

        base.grabReward(player);
    }
}
