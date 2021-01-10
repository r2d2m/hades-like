using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour {
    GameObject mainCamera;
    public GameObject soundManagerObject;
    SoundManager soundManager;

    float movementSpeed = 5000f;
    float primaryCD = 0.7f;
    float altCD = 1.0f; // Rightclick = special
    float currentprimaryCD = 0;

    public GameObject playerGun;
    public GameObject primWeapon;
    public GameObject altWeapon;
    public GameObject healthBar;
    public GameObject mousePointer;
    public GameObject floorGameObject;
    public Animator animator;

    private Rigidbody2D rigidBody;

    Vector2 movementVector;

    float invulCD = 1.0f;
    float currentInvulCD = 0;

    int maxHP;
    int currentHP;

    public float damageMultiplier = 1;
    public float cooldownMultiplier = 1;
    public float rangeMultiplier = 1;
    public float forceMultipler = 1.0f;
    public float spellLifeTimeMultiplier = 1.0f;

    float minPrimCD = 0.0001f;
    float minAltCD = 0.0001f;
    float minPrimDMG = 0.5f;
    float minAltDMG = 0.5f;
    float minPrimMULT = 0.5f;
    float minAltMULT = 0.5f;


    bool flipAnimatorX = false;

    SpriteRenderer playerSpriteRenderer;
    SpriteRenderer gunSpriteRenderer;

    Color transparentColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    Color opaqueColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    public List<GameObject> equippedSpells;
    List<float> spellCooldowns;

    Vector3 currentMousePos = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start() {
        maxHP = 5;
        currentHP = maxHP;
        spellCooldowns = new List<float>() { 0, 0 };

        updateHealthBar();
        mainCamera = GameObject.Find("MainCamera");
        rigidBody = GetComponent<Rigidbody2D>();
        soundManager = soundManagerObject.GetComponent<SoundManager>();

        primaryCD = primWeapon.GetComponent<Weapon>().getBaseCoolDown();
        altCD = primWeapon.GetComponent<Weapon>().getBaseCoolDown() * 4;
        playerSpriteRenderer = animator.GetComponent<SpriteRenderer>();
        gunSpriteRenderer = playerGun.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        updateCooldowns();
        playerMovement();
        playerAim();
        playerActions();
        spellInputHandler();
        invulFlicker();

        // TODO: delete this later!
        debugPlayer();
        animator.SetFloat("MovementSpeed", movementVector.magnitude);
    }

    void spellInputHandler() {
        if (Input.GetKeyDown(KeyCode.Alpha1) && spellCooldowns[0] <= 0) {
            castSpell(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && spellCooldowns[1] <= 0) {
            castSpell(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha3) && spellCooldowns[2] <= 0) {
            castSpell(2);
        }
    }

    void castSpell(int spellIndex) {
        GameObject spellObject = Instantiate(equippedSpells[spellIndex]);
        spellObject.transform.parent = floorGameObject.transform;
        Spell spellScript = spellObject.GetComponent<Spell>();

        spellCooldowns[spellIndex] = spellScript.getCooldownTime() * cooldownMultiplier;
        spellScript.setMousePos(currentMousePos);
        spellScript.setPlayerGameObject(gameObject);
        spellScript.setPlayerPos(transform.position);
        spellScript.setGunGameObject(playerGun);
        spellScript.setMouseGameObject(mousePointer);
        spellScript.setPlayerStats(damageMultiplier, rangeMultiplier, cooldownMultiplier);
    }

    // flicker player sprite when invurnable
    void invulFlicker() {
        if (currentInvulCD > 0) {
            if (currentInvulCD % 0.1f < 0.05f) {
                gunSpriteRenderer.color = transparentColor;
                playerSpriteRenderer.color = transparentColor;
            } else {
                gunSpriteRenderer.color = opaqueColor;
                playerSpriteRenderer.color = opaqueColor;
            }
        } else {
            gunSpriteRenderer.color = opaqueColor;
            playerSpriteRenderer.color = opaqueColor;
        }
    }

    void LateUpdate() {
        playerSpriteRenderer.flipX = flipAnimatorX;
    }

    void debugPlayer() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Update all cooldowns
    void updateCooldowns() {
        if (currentprimaryCD > 0) {
            currentprimaryCD -= Time.deltaTime;
        } else {
            currentprimaryCD = 0;
        }

        if (invulCD > 0) {
            currentInvulCD -= Time.deltaTime;
        } else {
            currentInvulCD = 0;
        }

        for (int i = 0; i < spellCooldowns.Count; i++) {
            if (spellCooldowns[i] > 0) {
                spellCooldowns[i] -= Time.deltaTime;
            } else {
                spellCooldowns[i] = 0;
            }
        }
    }

    // All physics related stuff should be here!
    void FixedUpdate() {
        rigidBody.AddForce(movementVector * movementSpeed);
    }

    // Handle player input
    void playerMovement() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movementVector = new Vector2(moveHorizontal, moveVertical);
        if (moveHorizontal > 0) {
            flipAnimatorX = false;
        } else if (moveHorizontal < 0) {
            flipAnimatorX = true;
        }
    }

    // Point the gun in the direction  of the mouse
    void playerAim() {
        Vector2 deltaVec = currentMousePos - transform.position;
        float rotationAngle = Mathf.Atan2(deltaVec.y, deltaVec.x) * Mathf.Rad2Deg;

        playerGun.transform.position = transform.position + Vector3.Normalize(deltaVec) * 0.4f;
        playerGun.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        gunSpriteRenderer.flipY = (rotationAngle >= -90 && rotationAngle < 90) ? false : true;
        gunSpriteRenderer.sortingOrder = (rotationAngle >= 30 && rotationAngle <= 150) ? 9 : 11;

        Debug.DrawLine(transform.position, currentMousePos, Color.red);
    }

    // Input for rangedAttacking and non-movement related input
    void playerActions() {
        // Left click and right click action
        if (Input.GetMouseButton(0) && currentprimaryCD <= 0) {
            WeaponType currentWeaponType = primWeapon.GetComponent<Weapon>().getWeaponType();
            if (currentWeaponType == WeaponType.RANGED) {
                rangedAttack(primWeapon, transform.position, currentMousePos, damageMultiplier);
            } else if (currentWeaponType == WeaponType.MELEE) {
                meleeAttack(primWeapon, transform.position, currentMousePos);
            }
            currentprimaryCD = primaryCD * cooldownMultiplier;
        } else if (Input.GetMouseButton(1) && currentprimaryCD <= 0) {
            WeaponType currentWeaponType = altWeapon.GetComponent<Weapon>().getWeaponType();
            if (currentWeaponType == WeaponType.RANGED) {
                Vector3 randTarget;
                float targetOffset = 1f;
                float forceOffset = 0.2f;
                for (int i = 0; i < 4; i++) {
                    randTarget = new Vector3(Random.Range(-targetOffset, targetOffset), Random.Range(-targetOffset, targetOffset), 0.0f);
                    rangedAttack(primWeapon, transform.position, currentMousePos + randTarget, forceMultipler * Random.Range(1 - forceOffset, 1 + forceOffset));
                }
            } else if (currentWeaponType == WeaponType.MELEE) {
                meleeAttack(primWeapon, transform.position, currentMousePos);
            }
            currentprimaryCD = altCD * cooldownMultiplier;
        }
    }

    // Function for firing a bullet from spawnPos to targetPos 
    // TODO: rangedAttack different bullets!
    void rangedAttack(GameObject bullet, Vector2 spawnPos, Vector2 targetPos, float force) {
        mainCamera.GetComponent<CameraManager>().screenShake(0.08f, 0.05f);
        Vector2 deltaVec = targetPos - (Vector2)transform.position;
        float rotationAngle = Mathf.Atan2(deltaVec.y, deltaVec.x) * Mathf.Rad2Deg;
        GameObject newBullet = Instantiate(bullet, spawnPos + deltaVec.normalized * 0.3f, Quaternion.AngleAxis(rotationAngle, Vector3.forward));

        newBullet.GetComponent<Bullet>().setDamageMultiplier(damageMultiplier);
        newBullet.GetComponent<Bullet>().setBulletForce(deltaVec.normalized, force);
        newBullet.GetComponent<Bullet>().setLifeTime(spellLifeTimeMultiplier);
        soundManager.playSoundClip(newBullet.GetComponent<Bullet>().getAttackSound(), 0.1f);
    }

    void meleeAttack(GameObject weapon, Vector2 playerPos, Vector2 targetPos) {
        mainCamera.GetComponent<CameraManager>().screenShake(0.08f, 0.05f);
        Vector2 deltaVec = targetPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(deltaVec.y, deltaVec.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector2 spawnPos = playerPos + deltaVec.normalized * 0.5f;
        GameObject newWeapon = Instantiate(weapon, spawnPos, rotation);

        newWeapon.GetComponent<Melee>().setDamageMultiplier(damageMultiplier);
        newWeapon.GetComponent<Melee>().setDeltaVector(deltaVec.normalized);
        newWeapon.transform.parent = transform;
    }

    // Heal damage
    void healDamage(int amount) {
        currentHP += amount;
        if (currentHP > maxHP) {
            currentHP = maxHP;
        }
    }

    // Take damage 
    void takeDamage(int amount) {
        if (currentInvulCD <= 0) {
            currentHP -= amount;
            updateHealthBar();
            currentInvulCD = invulCD;
            animator.SetTrigger("TookDamage");
            print("Took: " + amount + " damage, " + currentHP + "/" + maxHP + " HP left!");

            if (currentHP <= 0) {
                gameOver();
            }
        }
    }

    void updateHealthBar() {
        HealthBarManager hpManager = healthBar.GetComponent<HealthBarManager>();
        hpManager.setMaxHP(maxHP);
        hpManager.setCurrentHP(currentHP);
    }

    // Call on this when you are DEAD
    void gameOver() {
        print("Game Over :(");
        mainCamera.GetComponent<CameraManager>().setBackground(Color.red);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        int recievedDamage;
        switch (other.gameObject.tag) {
            case "Enemy":
                recievedDamage = other.gameObject.GetComponent<Enemy>().getCollisionDamage();
                if (recievedDamage > 0) {
                    takeDamage(recievedDamage);
                }
                // TODO: POISON AND OTHER EFFECTS?
                break;

            case "Door":
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "Reward":
                other.gameObject.GetComponent<Reward>().grabReward(gameObject);
                break;
            case "EnemyBullet":
                int recievedDamage;
                recievedDamage = other.gameObject.GetComponent<EnemyBullet>().getDamage();
                if (recievedDamage > 0) {
                    takeDamage(recievedDamage);
                }
                other.gameObject.GetComponent<EnemyBullet>().collideWithPlayer();
                // TODO: POISON AND OTHER EFFECTS?
                break;
        }
    }

    public Vector3 getPlayerPosition() {
        return transform.position;
    }

    public void modifyPlayerDamageMultiplier(float damageMult) {
        if (damageMultiplier + damageMult > minPrimMULT) {
            damageMultiplier += damageMult;
        } else {
            damageMultiplier = minPrimMULT;
        }
    }

    public void modifyPlayerHealth(int maxHealthMod) {
        maxHP += maxHealthMod;
        currentHP += maxHealthMod;
        if (currentHP > maxHP) {
            currentHP = maxHP;
        }

        updateHealthBar();

        if (maxHP <= 0) {
            gameOver();
        }
    }

    public void modifyCooldown(float cooldownMod) {
        if (cooldownMultiplier - cooldownMod > minPrimCD) {
            cooldownMultiplier -= cooldownMod;
        } else {
            cooldownMultiplier = minPrimCD;
        }
    }
}
