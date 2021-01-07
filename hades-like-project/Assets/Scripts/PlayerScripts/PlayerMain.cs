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
    public Animator animator;

    private Rigidbody2D rigidBody;

    Vector2 movementVector;

    float invisCD = 1.0f;
    float currentInvisCD = 0;

    int maxHP;
    int currentHP;

    float primFlatDamageBonus = 1;
    float primDamageMultiplier = 1;
    float altBaseDamage = 1;
    float altDamageMultiplier = 1;

    float minPrimCD = 0.0001f;
    float minAltCD = 0.0001f;
    float minPrimDMG = 0.5f;
    float minAltDMG = 0.5f;
    float minPrimMULT = 0.5f;
    float minAltMULT = 0.5f;

    float playerBulletForceMod = 1.0f;
    float weaponLifeTimeMod = 1.0f;

    bool flipAnimatorX = false;

    // Start is called before the first frame update
    void Start() {
        maxHP = 5;
        currentHP = maxHP;

        updateHealthBar();
        mainCamera = GameObject.Find("MainCamera");
        rigidBody = GetComponent<Rigidbody2D>();
        soundManager = soundManagerObject.GetComponent<SoundManager>();

        primaryCD = primWeapon.GetComponent<Weapon>().getBaseCoolDown();
        altCD = primWeapon.GetComponent<Weapon>().getBaseCoolDown() * 4;

    }

    // Update is called once per frame
    void Update() {
        updateCooldowns();
        playerMovement();
        playerAim();
        playerActions();

        // TODO: delete this later!
        debugPlayer();
        animator.SetFloat("MovementSpeed", movementVector.magnitude);
    }

    void LateUpdate() {
        animator.GetComponent<SpriteRenderer>().flipX = flipAnimatorX;
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

        if (invisCD > 0) {
            currentInvisCD -= Time.deltaTime;
        } else {
            currentInvisCD = 0;
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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 deltaVec = mousePos - transform.position;
        float rotationAngle = Mathf.Atan2(deltaVec.y, deltaVec.x) * Mathf.Rad2Deg;

        playerGun.transform.position = transform.position + Vector3.Normalize(deltaVec) * 0.4f;
        playerGun.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        playerGun.GetComponentInChildren<SpriteRenderer>().flipY = (rotationAngle >= -90 && rotationAngle < 90) ? false : true;
        playerGun.GetComponentInChildren<SpriteRenderer>().sortingOrder = (rotationAngle >= 30 && rotationAngle <= 150) ? 9 : 11;

        print(rotationAngle);
        Debug.DrawLine(transform.position, mousePos, Color.red);
    }

    // Input for rangedAttacking and non-movement related input
    void playerActions() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Left click and right click action
        if (Input.GetMouseButton(0) && currentprimaryCD <= 0) {
            WeaponType currentWeaponType = primWeapon.GetComponent<Weapon>().getWeaponType();
            if (currentWeaponType == WeaponType.RANGED) {
                rangedAttack(primWeapon, transform.position, mousePos, playerBulletForceMod);
            } else if (currentWeaponType == WeaponType.MELEE) {
                meleeAttack(primWeapon, transform.position, mousePos);
            }
            currentprimaryCD = primaryCD;
        } else if (Input.GetMouseButton(1) && currentprimaryCD <= 0) {
            WeaponType currentWeaponType = altWeapon.GetComponent<Weapon>().getWeaponType();
            if (currentWeaponType == WeaponType.RANGED) {
                Vector3 randTarget;
                float targetOffset = 1f;
                float forceOffset = 0.2f;
                for (int i = 0; i < 4; i++) {
                    randTarget = new Vector3(Random.Range(-targetOffset, targetOffset), Random.Range(-targetOffset, targetOffset), 0.0f);
                    rangedAttack(primWeapon, transform.position, mousePos + randTarget, playerBulletForceMod * Random.Range(1 - forceOffset, 1 + forceOffset));
                }
            } else if (currentWeaponType == WeaponType.MELEE) {
                meleeAttack(primWeapon, transform.position, mousePos);
            }
            currentprimaryCD = altCD;
        }
    }

    // Function for firing a bullet from spawnPos to targetPos 
    // TODO: rangedAttack different bullets!
    void rangedAttack(GameObject bullet, Vector2 spawnPos, Vector2 targetPos, float force) {
        mainCamera.GetComponent<CameraManager>().screenShake(0.08f, 0.05f);
        Vector2 deltaVec = targetPos - (Vector2)transform.position;
        float rotationAngle = Mathf.Atan2(deltaVec.y, deltaVec.x) * Mathf.Rad2Deg;
        GameObject newBullet = Instantiate(bullet, spawnPos + deltaVec.normalized * 0.3f, Quaternion.AngleAxis(rotationAngle, Vector3.forward));

        newBullet.GetComponent<Bullet>().setDamage(primFlatDamageBonus, primDamageMultiplier);
        newBullet.GetComponent<Bullet>().setBulletForce(deltaVec.normalized, force);
        newBullet.GetComponent<Bullet>().setLifeTime(weaponLifeTimeMod);
        soundManager.playSoundClip(newBullet.GetComponent<Bullet>().getAttackSound(), 0.1f);
    }

    void meleeAttack(GameObject weapon, Vector2 playerPos, Vector2 targetPos) {
        mainCamera.GetComponent<CameraManager>().screenShake(0.08f, 0.05f);
        Vector2 deltaVec = targetPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(deltaVec.y, deltaVec.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector2 spawnPos = playerPos + deltaVec.normalized * 0.5f;
        GameObject newWeapon = Instantiate(weapon, spawnPos, rotation);
        newWeapon.GetComponent<Melee>().setDamage(primFlatDamageBonus, primDamageMultiplier);
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
        if (currentInvisCD <= 0) {
            currentHP -= amount;
            updateHealthBar();
            currentInvisCD = invisCD;
            print("Took: " + amount + " damage, " + currentHP + "/" + maxHP + " HP left!");
            if (currentHP <= 0) {
                gameOver();
            }
        }
    }

    void updateHealthBar() {
        healthBar.GetComponent<HealthBarManager>().setMaxHP(maxHP);
        healthBar.GetComponent<HealthBarManager>().setCurrentHP(currentHP);
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

    public void modifyPlayerDamage(float primDmg, float primMult, float altDmg, float altMult) {
        if (primFlatDamageBonus + primDmg > minPrimDMG) {
            primFlatDamageBonus += primDmg;
        } else {
            primFlatDamageBonus = minPrimDMG;
        }

        if (primDamageMultiplier + primMult > minPrimMULT) {
            primDamageMultiplier += primMult;
        } else {
            primFlatDamageBonus = minPrimMULT;
        }

        if (altBaseDamage + altDmg > minAltDMG) {
            altBaseDamage += altDmg;
        } else {
            altBaseDamage = minAltDMG;
        }

        if (altDamageMultiplier + altMult > minAltMULT) {
            altDamageMultiplier += altMult;
        } else {
            altDamageMultiplier = minAltMULT;
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

    public void modifyCooldowns(float primaryCooldown, float altCooldown) {
        if (primaryCD - primaryCooldown > minPrimCD) {
            primaryCD -= primaryCooldown;
        } else {
            primaryCD = minPrimCD;
        }

        if (altCD - altCooldown > minAltCD) {
            altCD -= altCooldown;
        } else {
            altCD = minAltCD;
        }
    }
}
