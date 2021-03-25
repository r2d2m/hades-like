using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType { AOE, PROJECTILE, MELEE, SHIELD };

public class Spell : MonoBehaviour {

    public GameObject playerGameObject;
    public GameObject gunGameObject;
    public GameObject mouseGameObject;
    public GameObject mainCameraObject;

    public Vector3 playerPos;
    public Vector3 mousePos;
    public float cooldownTime;
    public float damage;
    public float testCooldown = 1.0f;

    public bool isBasicAttack;

    protected float damageMultiplier = 1.0f;
    protected float rangeMultiplier = 1.0f;
    protected float cooldownMultiplier = 1.0f;
    protected float speedMultiplier = 1.0f;
    protected float lifeTimeMultiplier = 1.0f;
    protected float forceMultipler = 1.0f;
    protected float manaCost = 50.0f;
    protected float playerAgility = 0;
    protected float playerStrength = 0;
    protected float playerIntelligence = 0;

    public Sprite spellIcon;

    public void setPlayerPos(Vector3 playerPos) {
        this.playerPos = playerPos;
    }

    public void setSpellIcon(Sprite spellIcon) {
        this.spellIcon = spellIcon;
    }

    public Sprite getSpellIcon() {
        if (spellIcon != null) {
            return spellIcon;
        } else {
            print("SpellIcon is missing!!!");
            return null;
        }
    }

    public void setMousePos(Vector3 mousePos) {
        this.mousePos = new Vector3(mousePos.x, mousePos.y, 0);
    }

    public bool getIsBasicAttack() {
        return isBasicAttack;
    }

    public void setPlayerGameObject(GameObject playerGameObject) {
        this.playerGameObject = playerGameObject;
    }

    public void setGunGameObject(GameObject gunGameObject) {
        this.gunGameObject = gunGameObject;
    }

    public void setMouseGameObject(GameObject mouseGameObject) {
        this.mouseGameObject = mouseGameObject;
    }

    public void setMainCameraObject(GameObject mainCameraObject) {
        this.mainCameraObject = mainCameraObject;
    }

    public float getDamage() {
        return damage;
    }

    public float getManaCost(){
        return manaCost;
    }

    public Vector3 getMouseVector() {
        return (mousePos - playerPos).normalized;
    }

    public void setPlayerStats(float damageMultiplier, float playerAgility, float playerStrength, float playerIntelligence) {
        this.damageMultiplier = damageMultiplier;
        this.playerAgility = playerAgility;
        this.playerStrength = playerStrength;
        this.playerIntelligence = playerIntelligence;
    }

    public virtual float getCooldownTime() {
        return cooldownTime;
    }

    protected Vector3 getMouseDeltaVector() {
        return (mousePos - playerPos);
    }

    protected void setRotationTowardsVector(Vector3 deltaVector) {
        float rotationAngle = Mathf.Atan2(deltaVector.y, deltaVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }

    protected void destroySelfWithDelay(float delay) {
        if (GetComponentInChildren<Animator>() != null) {
            IEnumerator destroyRutine = waitAndDestroy(delay);
            StartCoroutine(destroyRutine);
        } else {
            Destroy(gameObject, delay);
        }
    }

    protected IEnumerator waitAndDestroy(float delay) {
        yield return new WaitForSeconds(delay);
        destroySelfAndTriggerAnim();
    }

    protected void destroySelfAndTriggerAnim() {
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("Destroy");
        if (anim != null) {
            anim.SetTrigger("Destroy");
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
    }
}
