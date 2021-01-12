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
    public GameObject canvasUI;
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

    float minCD = 0.0001f;
    float minDmgMult = 0.5f;

    bool flipAnimatorX = false;

    SpriteRenderer playerSpriteRenderer;
    SpriteRenderer gunSpriteRenderer;

    Color transparentColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    Color opaqueColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    public List<GameObject> equippedSpells;
    List<float> spellCooldowns;

    Vector3 currentMousePos = new Vector3(0, 0, 0);

    CooldownUI cooldownUI;
    HealthBarManager hpManager;

    // Start is called before the first frame update
    void Start() {
        maxHP = 5;
        currentHP = maxHP;
        spellCooldowns = new List<float>() { 0, 0, 0, 0 };

        cooldownUI = canvasUI.GetComponent<CooldownUI>();
        hpManager = canvasUI.GetComponent<HealthBarManager>();

        cooldownUI.setSpellCount(spellCooldowns.Count);

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
        updateCooldownsUI();
        playerMovement();
        playerAim();
        //playerActions();
        spellInputHandler();
        invulFlicker();

        // TODO: delete this later!
        debugPlayer();
        animator.SetFloat("MovementSpeed", movementVector.magnitude);
    }

    public void resetCooldowns() {
        for (int i = 0; i < spellCooldowns.Count; i++) {
            spellCooldowns[i] = 0;
        }
        updateCooldownsUI();
    }

    void spellInputHandler() {
        if (Input.GetMouseButton(0) && spellCooldowns[0] <= 0) {
            castSpell(0);
        } else if (Input.GetMouseButton(1) && spellCooldowns[1] <= 0) {
            castSpell(1);
        } else if (Input.GetKey(KeyCode.Alpha1) && spellCooldowns[2] <= 0) {
            castSpell(2);
        } else if (Input.GetKey(KeyCode.Alpha2) && spellCooldowns[3] <= 0) {
            castSpell(3);
        } else if (Input.GetKey(KeyCode.Alpha3) && spellCooldowns[4] <= 0) {
            castSpell(4);
        }
    }

    void castSpell(int spellIndex) {
        GameObject spellObject = Instantiate(equippedSpells[spellIndex]);
        spellObject.transform.parent = floorGameObject.transform;
        Spell spellScript = spellObject.GetComponent<Spell>();

        spellCooldowns[spellIndex] = spellScript.getCooldownTime() * cooldownMultiplier;
        cooldownUI.setCooldownDuration(spellIndex, spellCooldowns[spellIndex]);

        spellScript.setMousePos(currentMousePos);
        spellScript.setPlayerGameObject(gameObject);
        spellScript.setPlayerPos(transform.position);
        spellScript.setGunGameObject(playerGun);
        spellScript.setMouseGameObject(mousePointer);
        spellScript.setMainCameraObject(mainCamera);
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

    void updateCooldownsUI() {
        cooldownUI.updateCooldowns(spellCooldowns);
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
        hpManager = canvasUI.GetComponent<HealthBarManager>();
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
        if (damageMultiplier + damageMult > minDmgMult) {
            damageMultiplier += damageMult;
        } else {
            damageMultiplier = minDmgMult;
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
        if (cooldownMultiplier - cooldownMod > minCD) {
            cooldownMultiplier -= cooldownMod;
        } else {
            cooldownMultiplier = minCD;
        }
    }
}
