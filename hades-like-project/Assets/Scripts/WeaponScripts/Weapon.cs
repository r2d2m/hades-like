using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType { RANGED, MELEE };

public class Weapon : MonoBehaviour {

    public GameObject explodeEffect;
    protected float weaponDamage = 0;
    public float baseDamage = 1.0f; // different base damage for different weapons
    public float baseLifeTime = 1.0f;
    protected float lifeTime = 1.0f;
    public float enemyPushForce = 1.0f;
    public float baseCD = 1.0f;

    public AudioClip[] attackSounds;
    public WeaponType currentType;

    public void setDamage(float playerFlatDamageBonus, float playerDamageMultiplier) {
        weaponDamage = playerDamageMultiplier * baseDamage + playerFlatDamageBonus;
    }

    public WeaponType getWeaponType() {
        return currentType;
    }

    public float getBaseCoolDown(){
        return baseCD;
    }

    private void OnDestroy() {
        // Create particle effects
        if (explodeEffect != null) {
            Instantiate(explodeEffect, transform.position, transform.rotation);
        }
    }

    public AudioClip getAttackSound() {
        int randomIndex = Random.Range(0, attackSounds.Length - 1);
        return attackSounds[randomIndex];
    }
}
