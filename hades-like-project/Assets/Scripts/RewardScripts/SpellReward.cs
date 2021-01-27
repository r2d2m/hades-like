using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellReward : Reward {
    GameObject spell;
    // Start is called before the first frame update
    void Start() {
        spell = loadRandomSpell(1);
        GetComponentInChildren<SpriteRenderer>().sprite = spell.GetComponent<Spell>().getSpellIcon();
        GetComponentInChildren<Transform>().localScale = new Vector3(0.7f, 0.7f, 1);
    }

    public GameObject loadRandomSpell(int tier) {
        string spellType = "";
        GameObject[] spellArray;
        switch (tier) {
            case 1:
                spellType = "NormalRooms";
                break;
        }
        spellArray = Resources.LoadAll<GameObject>("Spells");
        return spellArray[Random.Range(0, spellArray.Length)];
    }

    public override void grabReward(GameObject player) {
        PlayerMain playerMain = player.GetComponent<PlayerMain>();
        GameObject newSpell = spell;
        playerMain.pickupSpell(spell);
        base.grabReward(player);
        print("Picked up spell: " + spell);
    }
}
