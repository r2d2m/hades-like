using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Parent class for all enemies, should include things that all enemies share!
*/
public class Enemy : MonoBehaviour {
    public GameObject corpsePrefab;
    public GameObject floor;
    public GameObject dirBloodSplatter;

    public float maxHP;
    public float currentHP;
    protected int rewardSouls = 10;

    protected GameObject player;
    public int collisionDamage = 0;
    protected Vector3 movementVector;
    protected Rigidbody2D rigidBody;
    protected float movementStr;
    protected Color originalColor;
    protected SpriteRenderer spriteRenderer;

    public enum EnemyStates { IDLE, CHASING, DYING, DEAD };
    public EnemyStates currentState = EnemyStates.CHASING;
    protected float colorTime = 0.07f;
    protected float currentColorTime = 0.0f;
    public float chaseRange = 0;

    AudioSource enemyAudioSource;

    public AudioClip idleSound;
    public AudioClip damagedSound;
    public AudioClip deathSound;


    // Start is called before the first frame update
    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        floor = GameObject.FindGameObjectWithTag("Floor");
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        enemyAudioSource = GetComponent<AudioSource>();
    }

    // Check if enemy is dead
    protected bool deathCheck() {
        if (currentHP <= 0) {
            die();
            return true;
        }
        return false;
    }

    public void setChaseRange(float range) {
        chaseRange = range;
        currentState = EnemyStates.IDLE;
    }

    public void chaseRangeCheck() {
        if ((player.transform.position - transform.position).magnitude < chaseRange) {
            currentState = EnemyStates.CHASING;
        }
    }

    // Get the player gameobject
    GameObject getPlayer() {
        return player;
    }

    public void checkStatusEffects() {

    }

    // Update things as current color etc
    protected void updateCooldowns() {
        if (currentColorTime > 0) {
            currentColorTime -= Time.deltaTime;
        } else {
            currentColorTime = 0;
            spriteRenderer.color = originalColor;
        }
    }

    public void takeDirectionalDamage(float damage, Vector3 damageSourcePos) {
        takeDamage(damage);
        spawnDirectionalBlood(damageSourcePos);
    }

    // Take damage and set color
    public void takeDamage(float damage) {
        if (damage > 0) {
            if (damagedSound != null) {
                enemyAudioSource.PlayOneShot(damagedSound);
            }
            currentHP -= damage;
            currentColorTime = colorTime;
            spriteRenderer.color = new Color(1.0f, 0.0f, 0.0f);
        }
    }

    public void spawnDirectionalBlood(Vector3 damageSourcePos) {
        if (dirBloodSplatter != null) {
            Vector3 deltaVec = damageSourcePos - transform.position;
            Quaternion rot = Quaternion.LookRotation(deltaVec);
            rot = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, -90);
            Instantiate(dirBloodSplatter, damageSourcePos - deltaVec.normalized * 0.4f, rot, floor.transform);
        } else {
            print("Missing bloodsplatter");
        }
    }

    protected void setToOriginalColor() {
        spriteRenderer.color = originalColor;
    }

    public void heal() {

    }

    public int getCollisionDamage() {
        return collisionDamage;
    }

    public void disableEnemy() {
        enabled = false;
    }

    public void enableEnemy() {
        enabled = true;
    }

    /*
        private void OnCollisionEnter2D(Collision2D other) {
            float damageTaken;
            switch (other.transform.tag) {
                case "PlayerSpell":
                    damageTaken = other.gameObject.GetComponent<Spell>().getDamage();
                    takeDamage(damageTaken);
                    break;
            }

        }
    */
    // every 2 seconds perform the print()
    private IEnumerator FadeShadow(GameObject shadowObject, float fadeSpeed) {
        /*
        while (shadowObject.GetComponent<SpriteRenderer>().color.a > 0) {
            Color newColor = shadowObject.GetComponent<SpriteRenderer>().color - new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
            shadowObject.GetComponent<SpriteRenderer>().color = newColor;
            print(newColor.a);
            yield return null;
        }*/

        Vector3 originalScale = shadowObject.transform.localScale;
        while (shadowObject.transform.localScale.x > 0.01f) {
            print(Time.deltaTime);
            shadowObject.transform.localScale -= originalScale * Time.deltaTime * 1.8f;
            yield return null;
        }
    }

    // Things to do and set during deah
    public virtual void die() {
        // Spawn corpse if we have a prefab

        if(enemyAudioSource != null && deathSound != null){
            enemyAudioSource.PlayOneShot(deathSound);
        }

        if (corpsePrefab != null) {
            GameObject corpse = Instantiate(corpsePrefab, transform.position, transform.rotation);
            corpse.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            corpse.transform.parent = transform.parent; // TODO CHANGE THIS!
            //corpse.GetComponent<SpriteRenderer>().color = originalColor;
        }

        Transform shadow = gameObject.transform.Find("Shadow");

        if (shadow != null) {
            IEnumerator fadeCorutine = FadeShadow(shadow.gameObject, 1.3f);
            StartCoroutine(fadeCorutine);
        }

        floor.GetComponent<RoomManager>().enemyDeath(transform.position, rewardSouls);

        // TODO: Debug, every enemy WILL have an animator
        if (GetComponentInChildren<Animator>() != null) {
            GetComponentInChildren<Animator>().SetTrigger("Die");
        } else {
            Destroy(gameObject);
        }
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Corpses";
        gameObject.tag = "Corpse";
        currentState = EnemyStates.DEAD;
        setToOriginalColor();
        this.enabled = false;
    }
}
