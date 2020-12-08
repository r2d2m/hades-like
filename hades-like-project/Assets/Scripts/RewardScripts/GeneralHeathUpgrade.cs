using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralHealthUpgrade : Reward {
    public float primaryDamage = 1;
    public float primaryMult = 1;
    public float secondaryDamage = 1;
    public float secondaryMult = 1;

    // Update is called once per frame
    public override void grabReward(GameObject player) {
        player.GetComponent<PlayerMain>().modifyPlayerDamage(primaryDamage, primaryMult, secondaryDamage, secondaryMult);
        base.grabReward(player);
    }
}
