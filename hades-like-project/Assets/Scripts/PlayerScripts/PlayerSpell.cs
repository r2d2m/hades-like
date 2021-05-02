using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpell {
    public GameObject spellObject;
    public float cooldown;
    public float currentCooldown;
    public float manaCost;

    public PlayerSpell() {
        spellObject = null;
        cooldown = 1;
        currentCooldown = 0;
        manaCost = 10;
    }

    public PlayerSpell(GameObject spellObject, float cooldown, float manaCost) {
        this.spellObject = spellObject;
        this.cooldown = cooldown;
        this.currentCooldown = 0;
        this.manaCost = manaCost;
    }

    public GameObject getSpellObject(){
        return spellObject;
    }

    public float getCooldownTime(){
        return cooldown;
    }

    public float getCurrentCooldown(){
        return currentCooldown;
    }

    public float getManaCost(){
        return manaCost;
    }

    public Spell getSpellScript(){
        return spellObject.GetComponent<Spell>();
    }
}
