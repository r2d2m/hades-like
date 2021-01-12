using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStatUpgrade : Reward {
    public float damageMultiplier = 0;
    public float cooldownMultiplier = 0;
    public int maxHealthMod = 0;

    // Update is called once per frame
    public override void grabReward(GameObject player) {
        PlayerMain playerMain = player.GetComponent<PlayerMain>();
        playerMain.modifyPlayerDamageMultiplier(damageMultiplier);
        playerMain.modifyPlayerHealth(maxHealthMod);
        playerMain.modifyCooldown(cooldownMultiplier);
        base.grabReward(player);
    }
}
