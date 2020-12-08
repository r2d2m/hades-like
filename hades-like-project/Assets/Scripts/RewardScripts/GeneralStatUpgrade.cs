using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStatUpgrade : Reward {
    public float primaryDamage = 0;
    public float primaryMult = 0;
    public float altDamage = 0;
    public float altMult = 0;
    public float primaryCooldown = 0;
    public float altCooldown = 0;
    public int maxHealthMod = 0;

    // Update is called once per frame
    public override void grabReward(GameObject player) {
        PlayerMain playerMain = player.GetComponent<PlayerMain>();
        playerMain.modifyPlayerDamage(primaryDamage, primaryMult, altDamage, altMult);
        playerMain.modifyPlayerHealth(maxHealthMod);
        playerMain.modifyCooldowns(primaryCooldown, altCooldown);
        base.grabReward(player);
    }
}
