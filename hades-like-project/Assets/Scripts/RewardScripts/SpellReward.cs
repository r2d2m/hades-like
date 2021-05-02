using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellReward : Reward {
    GameObject spell;
    // Start is called before the first frame update
    void Start() {
        spell = loadRandomSpell(1);
        GetComponentInChildren<SpriteRenderer>().sprite = spell.GetComponent<Spell>().getSpellIcon();
        GetComponentInChildren<Transform>().localScale = new Vector3(1f, 1f, 1);
    }

    public GameObject loadRandomSpell(int tier) {
        string spellType = "";
        GameObject[] spellArray;
        switch (tier) {
            case 1:
                spellType = "NormalRooms";
                break;
        }

        PlayerMain playerMain = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();
        List<PlayerSpell> playerSpells = playerMain.getEquippedSpells();
        List<string> playerSpellNames = new List<string>();
        for (int i = 0; i < playerSpells.Count; i++) {
            print(playerSpells[i].spellObject.name);
            playerSpellNames.Add(playerSpells[i].spellObject.name);
        }

        spellArray = Resources.LoadAll<GameObject>("Spells");
        GameObject newSpell;

        // TODO this is a very shitty solution
        int tries = 100;
        while (tries > 0) {
            newSpell = spellArray[Random.Range(0, spellArray.Length)];
            if (!playerSpellNames.Contains(newSpell.gameObject.name)) {
                return newSpell;
            }
            tries -= 1;
        }
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
