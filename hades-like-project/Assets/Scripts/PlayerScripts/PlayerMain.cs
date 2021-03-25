using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour {
    GameObject mainCamera;
    public GameObject soundManagerObject;
    SoundManager soundManager;

    float movementSpeed = 5000f;

    public GameObject playerGun;
    public GameObject primWeapon;
    public GameObject altWeapon;
    public GameObject canvasUI;
    public GameObject mousePointer;
    public GameObject floorGameObject;
    public GameObject shadowObject;
    public Animator animator;

    private Rigidbody2D rigidBody;

    Vector2 movementVector;

    float invulCD = 1.0f;
    float currentInvulCD = 0;

    int maxHP;
    int currentHP;
    int currentSouls = 0;

    public float globalDamageMultiplier = 1.0f;
    public float globalCooldownMultiplier = 1.0f;
    public float rangeMultiplier = 1.0f;
    public float spellSpeedMultiplier = 1.0f;
    public float spellForceMultipler = 1.0f;
    public float spellLifeTimeMultiplier = 1.0f;
    public float movementSpeedMultiplier = 1.0f;

    public float agility = 0;
    public float strength = 0;
    public float intelligence = 0;

    float minCD = 0.0001f;
    float minDmgMult = 0.5f;
    float minLifeTime = 0.5f;
    float minForce = 0.1f;
    float minMoveSpeed = 0.1f;
    float minSpellSpeed = 0.1f;

    public bool inDialogue = false;
    bool flipAnimatorX = false;

    SpriteRenderer playerSpriteRenderer;
    SpriteRenderer gunSpriteRenderer;

    Color transparentColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    Color opaqueColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    public List<GameObject> equippedSpells;
    List<float> maxSpellCooldowns;

    List<float> spellCooldowns;
    List<float> manaCosts;

    Vector3 currentMousePos = new Vector3(0, 0, 0);

    CooldownUI spellUI;
    public GameObject InventoryUI;
    SoulCountUI soulCountUI;
    HealthFlowerManger hpManager;
    ManaUI manaUI;

    bool inventoryIsOpen = false;
    bool usingBasicAttack;
    int currentHoldSpellID = -1;

    float maxMana = 100;
    float currentMana;

    // Start is called before the first frame update
    void Start() {
        currentMana = maxMana;
        maxHP = 5;
        currentHP = maxHP;
        usingBasicAttack = false;

        spellCooldowns = new List<float>();
        manaCosts = new List<float>();
        maxSpellCooldowns = new List<float>();

        spellUI = canvasUI.GetComponent<CooldownUI>();
        spellUI.setSpellCount(equippedSpells.Count);
        spellUI.setSpellIcons(equippedSpells);
        InventoryUI.GetComponent<InventoryManager>().initInventory();
        InventoryUI.SetActive(false);
        manaUI = canvasUI.GetComponent<ManaUI>();
        soulCountUI = canvasUI.GetComponent<SoulCountUI>();

        hpManager = canvasUI.GetComponent<HealthFlowerManger>();
        updateHealthBar();
        mainCamera = GameObject.Find("MainCamera");
        rigidBody = GetComponent<Rigidbody2D>();
        soundManager = soundManagerObject.GetComponent<SoundManager>();

        playerSpriteRenderer = animator.GetComponent<SpriteRenderer>();
        gunSpriteRenderer = playerGun.GetComponentInChildren<SpriteRenderer>();
        mousePointer.GetComponent<MousePointerScript>().setMouseAim();

        updateSpellManaCosts();
    }

    // Update is called once per frame
    void Update() {
        currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        updateCooldowns();
        handleInventory();
        if (!inventoryIsOpen) {
            playerMovement();
            playerAim();
            if (!inDialogue) { spellInputHandler(); }
        }

        invulFlicker();

        // TODO: delete this later!
        debugPlayer();
        animator.SetFloat("MovementSpeed", movementVector.magnitude);
        if (currentMana < maxMana) {
            currentMana = Mathf.Min(currentMana + Time.deltaTime * 10, maxMana);
        }

        spellUI.UpdateManaBar(currentMana, currentMana / maxMana, manaCosts);
    }

    public List<GameObject> getEquippedSpells() {
        return equippedSpells;
    }

    //TODO: clean this up
    void updateSpellManaCosts() {
        for (int i = 0; i < equippedSpells.Count; i++) {
            GameObject spellObject = Instantiate(equippedSpells[i]);
            spellObject.transform.parent = floorGameObject.transform;
            Spell spellScript = spellObject.GetComponent<Spell>();
            spellScript.setPlayerStats(globalDamageMultiplier, agility, strength, intelligence);
            float manaCost = spellScript.getManaCost();
            float coooldown = spellScript.getCooldownTime();
            manaCosts.Add(manaCost);
            spellCooldowns.Add(0);
            maxSpellCooldowns.Add(coooldown);
            Destroy(spellObject);
        }
    }

    public void switchSpellSlots(int firstSpellIndex, int secondSpellIndex) {
        GameObject temp = equippedSpells[firstSpellIndex];
        float tempCD = spellCooldowns[firstSpellIndex];
        equippedSpells[firstSpellIndex] = equippedSpells[secondSpellIndex];
        spellCooldowns[firstSpellIndex] = spellCooldowns[secondSpellIndex];
        equippedSpells[secondSpellIndex] = temp;
        spellCooldowns[secondSpellIndex] = tempCD;
        updateCooldowns();
        updateIcons();
    }

    public void resetCooldowns() {
        for (int i = 0; i < spellCooldowns.Count; i++) {
            spellCooldowns[i] = 0;
        }
        spellUI.updateCooldowns(spellCooldowns);
        currentMana = maxMana;
    }

    public void pickupSpell(GameObject newSpell) {
        if (equippedSpells.Count < 7) {
            equippedSpells.Add(newSpell);
            spellCooldowns.Add(0);
            // TODO: cleana upp
            GameObject spellObject = Instantiate(newSpell);
            Spell spellScript = spellObject.GetComponent<Spell>();
            spellScript.setPlayerStats(globalDamageMultiplier, agility, strength, intelligence);
            float manaCost = spellScript.getManaCost();
            float coooldown = spellScript.getCooldownTime();
            manaCosts.Add(manaCost);
            spellCooldowns.Add(0);
            maxSpellCooldowns.Add(coooldown);
            Destroy(spellObject);
            //spellUI.setSpellCount(equippedSpells.Count);
            spellUI.addSpell();
            InventoryUI.GetComponent<InventoryManager>().updateInventory();
            spellUI.UpdateManaBar(currentMana, currentMana / maxMana, manaCosts);
            updateIcons();
        } else {
            toggleInventory(true);
        }
    }

    public void updateIcons() {
        spellUI.setSpellIcons(equippedSpells);
        InventoryUI.GetComponent<InventoryManager>().setInventoryIcons(equippedSpells);
    }

    public void handleInventory() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            inventoryIsOpen = !inventoryIsOpen;
            toggleInventory(inventoryIsOpen);
        }
    }

    public void toggleInventory(bool toggle) {
        if (toggle) {
            Time.timeScale = 0;
            InventoryUI.SetActive(true);
            mousePointer.GetComponent<MousePointerScript>().setMouseHand();
            updateIcons();
        } else {
            Time.timeScale = 1;
            InventoryUI.SetActive(false);
            mousePointer.GetComponent<MousePointerScript>().setMouseAim();
        }
    }

    //Todo FIX THIS MESS
    void spellInputHandler() {
        if (Input.GetMouseButton(0) && equippedSpells.Count > 0) {
            spellCooldownCheckAndCast(0);
            usingBasicAttack = true;
        }
        if (Input.GetMouseButton(1) && equippedSpells.Count > 1) {
            spellCooldownCheckAndCast(1);
            usingBasicAttack = true;
        }
        if (Input.GetKey(KeyCode.Space) && equippedSpells.Count > 2) {
            spellCooldownCheckAndCast(2);
            usingBasicAttack = true;
        }
        if (Input.GetKey(KeyCode.Q) && equippedSpells.Count > 3) {
            spellCooldownCheckAndCast(3);
            usingBasicAttack = true;
        }
        if (Input.GetKey(KeyCode.E) && equippedSpells.Count > 4) {
            spellCooldownCheckAndCast(4);
            usingBasicAttack = true;
        }
        if (Input.GetKey(KeyCode.R) && equippedSpells.Count > 5) {
            spellCooldownCheckAndCast(5);
            usingBasicAttack = true;
        }
        if (Input.GetKey(KeyCode.F) && equippedSpells.Count > 6) {
            spellCooldownCheckAndCast(6);
            usingBasicAttack = true;
        }
        usingBasicAttack = false;
    }


    void spellCooldownCheckAndCast(int spellIndex) {
        if (spellCooldowns[spellIndex] <= 0) {
            GameObject spellObject = Instantiate(equippedSpells[spellIndex]);
            spellObject.transform.parent = floorGameObject.transform;
            Spell spellScript = spellObject.GetComponent<Spell>();
            spellScript.setPlayerStats(globalDamageMultiplier, agility, strength, intelligence);
            float manaCost = spellScript.getManaCost();
            if (currentMana >= manaCost) {
                if (spellScript.getIsBasicAttack()) {
                    if (!usingBasicAttack) {
                        castSpell(spellObject, spellScript, spellIndex);
                    } else {
                        Destroy(spellObject);
                    }
                } else {
                    castSpell(spellObject, spellScript, spellIndex);
                    usingBasicAttack = false;
                }
                currentMana -= manaCost;
            } else {
                Destroy(spellObject);
            }
        }
    }

    void castSpell(GameObject spellObject, Spell spellScript, int spellIndex) {
        print(spellScript.getCooldownTime());
        spellCooldowns[spellIndex] = spellScript.getCooldownTime() * globalCooldownMultiplier;
        spellUI.setCooldownDuration(spellIndex, spellCooldowns[spellIndex]);
        spellScript.setMousePos(currentMousePos);
        spellScript.setPlayerGameObject(gameObject);
        spellScript.setPlayerPos(transform.position);
        spellScript.setGunGameObject(playerGun);
        spellScript.setMouseGameObject(mousePointer);
        spellScript.setMainCameraObject(mainCamera);
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

        spellUI.updateCooldowns(spellCooldowns);
    }

    // All physics related stuff should be here!
    void FixedUpdate() {
        rigidBody.AddForce(movementVector * movementSpeed * movementSpeedMultiplier);
    }

    // Handle player input
    void playerMovement() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movementVector = new Vector2(moveHorizontal, moveVertical);

        // Cap diagonal movespeed but keep smoothing from GetAxis()
        if (movementVector.magnitude > 1) {
            movementVector.Normalize();
        }

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
        // TODO: FIX THIS 
        //gunSpriteRenderer.sortingOrder = (rotationAngle >= 30 && rotationAngle <= 150) ? 9 : 11;

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
        hpManager = canvasUI.GetComponent<HealthFlowerManger>();
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
            case "EnemyBullet":
                recievedDamage = other.gameObject.GetComponent<EnemyBullet>().getDamage();
                if (recievedDamage > 0) {
                    takeDamage(recievedDamage);
                }
                other.gameObject.GetComponent<EnemyBullet>().collideWithPlayer();
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
            case "ExitDoor":
                break;
            case "PlayerHazard":
                takeDamage(other.gameObject.GetComponent<PlayerHazard>().getDamage());
                break;
            case "Loot":
                currentSouls += other.gameObject.GetComponent<SoulScript>().soulCount;
                other.gameObject.GetComponent<SoulScript>().DestroyMe();
                soulCountUI.setSoulCount(currentSouls);
                break;
        }
    }

    public Vector3 getPlayerPosition() {
        return transform.position;
    }

    public void modifyPlayerDamageMultiplier(float damageMult) {
        /*
        if (damageMultiplier + damageMult > minDmgMult) {
            damageMultiplier += damageMult;
        } else {
            damageMultiplier = minDmgMult;
        }*/
        globalDamageMultiplier = Mathf.Max(globalDamageMultiplier + damageMult, minDmgMult);
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
        /*
        if (globalCooldownMultiplier - cooldownMod > minCD) {
            globalCooldownMultiplier -= cooldownMod;
        } else {
            globalCooldownMultiplier = minCD;
        }*/
        globalCooldownMultiplier = Mathf.Max(globalCooldownMultiplier - cooldownMod, minCD);
    }


    public void modifyLifeTime(float lifeTimeMod) {
        spellLifeTimeMultiplier = Mathf.Max(spellLifeTimeMultiplier + lifeTimeMod, minLifeTime);
    }

    public void modifySpellForce(float forceMod) {
        spellForceMultipler = Mathf.Max(spellForceMultipler + forceMod, minForce);
    }


    public void modifySpellSpeed(float speedMod) {
        spellSpeedMultiplier = Mathf.Max(spellSpeedMultiplier + speedMod, minSpellSpeed);
    }

    public void modifyPlayerSpeed(float speedMod) {
        movementSpeedMultiplier = Mathf.Max(movementSpeedMultiplier + speedMod, minMoveSpeed);

    }
}
