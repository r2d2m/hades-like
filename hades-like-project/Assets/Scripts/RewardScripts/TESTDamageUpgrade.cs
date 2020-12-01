using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTDamageUpgrade : Reward {

    // Update is called once per frame
    public override void grabReward(GameObject player) {
        player.GetComponent<PlayerMain>().modifyPlayerDamage(1, 1, 1, 1);
        base.grabReward(player);
    }
}
