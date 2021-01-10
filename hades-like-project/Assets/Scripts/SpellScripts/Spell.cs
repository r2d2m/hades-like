using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType{AOE, PROJECTILE, MELEE, SHIELD};

public class Spell : MonoBehaviour {

    public GameObject playerGameObject;
    public GameObject gunGameObject;
    public GameObject mouseGameObject;

    public Vector3 playerPos;
    public Vector3 mousePos;
    public float cooldownTime;
    public float damage;

    protected float damageMultiplier = 1;
    protected float rangeMultiplier = 1;
    protected float cooldownMultiplier = 1;

    public void setPlayerPos(Vector3 playerPos) {
        this.playerPos = playerPos;
    }

    public void setMousePos(Vector3 mousePos) {
        this.mousePos = new Vector3(mousePos.x, mousePos.y, 0);
    }

    public void setPlayerGameObject(GameObject playerGameObject){
        this.playerGameObject = playerGameObject;
    }

    public void setGunGameObject(GameObject gunGameObject){
        this.gunGameObject = gunGameObject;
    }

    public void setMouseGameObject(GameObject mouseGameObject){
        this.mouseGameObject = mouseGameObject;
    }
    
    public float getDamage(){
        return damage;
    }

    public Vector3 getMouseVector(){
        return (mousePos - playerPos).normalized;
    }

    public void setPlayerStats(float damageMultiplier, float rangeMultiplier, float cooldownMultiplier){
        this.damageMultiplier = damageMultiplier;
        this.rangeMultiplier = rangeMultiplier;
        this.cooldownMultiplier = cooldownMultiplier;
    }

    public virtual float getCooldownTime() {
        return cooldownTime;
    }
}
