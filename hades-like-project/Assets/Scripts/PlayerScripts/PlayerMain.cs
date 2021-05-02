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

    int baseMaxHP;
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

    public List<GameObject> startingSpells;
    List<PlayerSpell> equippedSpells;

    Vector3 currentMousePos = new Vector3(0, 0, 0);

    CooldownUI spellUI;
    public GameObject InventoryUI;
    SoulCountUI soulCountUI;
    HealthFlowerManger hpManager;
    ManaUI manaUI;

    bool inventoryIsOpen = false;
    bool usingBasicAttack;
    int currentHoldSpellID = -1;

    float baseMana = 100;
    float maxMana;
    float currentMana;

    float manaMaxGainPerInt = 10;
    float manaRegGainPerInt = 1;

    float movementSpeedPerAgi = 0.02f;
    float cooldownSpeedPerAgi = 0.03f;

    int nrOfStrPerMaxHealth = 3;
    float strDamageMultGain = 0.01f;
    private Dictionary<KeyCode, int> inputs;
    int maxSpellCount = 7;

    // Start is called before the first frame update
    void Start() {
        maxMana = baseMana + intelligence * manaMaxGainPerInt;
        currentMana = maxMana;
        baseMaxHP = 5;
        maxHP = baseMaxHP + (int)(strength / nrOfStrPerMaxHealth);
        currentHP = maxHP;
        usingBasicAttack = false;

        equippedSpells = new List<PlayerSpell>();
        for (int i = 0; i < maxSpellCount; i++) {
            equippedSpells.Add(null);
        }
        equipStartingSpells();

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
        MakeInputDictionary();
    }

    void equipStartingSpells() {
        for (int i = 0; i < startingSpells.Count; i++) {
            equippedSpells[i] = new PlayerSpell(startingSpells[i], startingSpells[i].GetComponent<Spell>().getCooldownTime(), startingSpells[i].GetComponent<Spell>().getManaCost());
        }
    }

    // Update is called once per frame
    void Update() {
        //maxMana = 100 + intelligence * manaMaxGainPerInt;
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
            currentMana = Mathf.Min(currentMana + Time.deltaTime * (1.0f / 10.0f * maxMana), maxMana);
        }

        spellUI.UpdateManaBar(currentMana, currentMana / maxMana, equippedSpells);
    }

    void MakeInputDictionary() {
        inputs = new Dictionary<KeyCode, int>() {
            {KeyCode.Mouse0,0},
            {KeyCode.Mouse1,1},
            {KeyCode.Space,2},
            {KeyCode.Q,3},
            {KeyCode.E,4},
            {KeyCode.R,5},
            {KeyCode.F,6}
        };
    }

    public List<PlayerSpell> getEquippedSpells() {
        return equippedSpells;
    }

    //TODO: clean this up
    void updateSpellManaCosts() {
        foreach (PlayerSpell spell in equippedSpells) {
            if (spell != null) {
                Spell spellScript = spell.getSpellScript();
                spellScript.setPlayerStats(globalDamageMultiplier, agility, strength, intelligence);
                spell.manaCost = spellScript.getManaCost();
                spell.currentCooldown = 0;
                spell.cooldown = spellScript.getCooldownTime();
                print(spellScript.getCooldownTime());
            }
        }
    }

    public void switchSpellSlots(int firstSpellIndex, int secondSpellIndex) {
        PlayerSpell temp = equippedSpells[firstSpellIndex];
        equippedSpells[firstSpellIndex] = equippedSpells[secondSpellIndex];
        equippedSpells[secondSpellIndex] = temp;
        updateIcons();
        updateCooldowns();
    }

    public void resetCooldowns() {
        foreach (PlayerSpell spell in equippedSpells) {
            if (spell != null) {
                spell.currentCooldown = 0;
            }
        }
        spellUI.updateCooldowns(equippedSpells);
        currentMana = maxMana;
    }

    public void pickupSpell(GameObject newSpellObject) {
        for (int i = 0; i < equippedSpells.Count; i++) {
            if (equippedSpells[i] == null) {
                Spell spellScript = newSpellObject.GetComponent<Spell>();
                spellScript.setPlayerStats(globalDamageMultiplier, agility, strength, intelligence);
                PlayerSpell newSpell = new PlayerSpell(newSpellObject, spellScript.getCooldownTime(), spellScript.getManaCost());
                equippedSpells[i] = newSpell;
                //spellUI.addSpell();
                InventoryUI.GetComponent<InventoryManager>().updateInventory();
                spellUI.UpdateManaBar(currentMana, currentMana / maxMana, equippedSpells);
                updateIcons();
                return;
            }
        }
        toggleInventory(true);
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

    void spellInputHandler() {
        foreach (KeyValuePair<KeyCode, int> entry in inputs) {
            if (Input.GetKey(entry.Key) && equippedSpells.Count > entry.Value) {
                spellCooldownCheckAndCast(entry.Value);
                usingBasicAttack = true;
                break;
            }
        }
        usingBasicAttack = false;
    }


    void spellCooldownCheckAndCast(int spellIndex) {
        print(equippedSpells[spellIndex]);
        if (equippedSpells[spellIndex] != null && equippedSpells[spellIndex].currentCooldown <= 0) {
            GameObject spellObject = Instantiate(equippedSpells[spellIndex].spellObject);
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
                playerGun.GetComponentInChildren<Animator>().SetTrigger("Shoot");
            } else {
                Destroy(spellObject);
            }
        }
    }

    void castSpell(GameObject spellObject, Spell spellScript, int spellIndex) {
        //print(maxSpellCooldowns[spellIndex]);
        equippedSpells[spellIndex].currentCooldown = equippedSpells[spellIndex].cooldown;
        spellUI.setCooldownDuration(spellIndex, equippedSpells[spellIndex].cooldown);
        //print(spellCooldowns[spellIndex]);
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

        for (int i = 0; i < equippedSpells.Count; i++) {
            if (equippedSpells[i] != null) {
                if (equippedSpells[i].currentCooldown > 0) {
                    equippedSpells[i].currentCooldown -= Time.deltaTime;
                } else {
                    equippedSpells[i].currentCooldown = 0;
                }
            }
        }

        spellUI.updateCooldowns(equippedSpells);
    }

    // All physics related stuff should be here!
    void FixedUpdate() {
        rigidBody.AddForce(movementVector * movementSpeed * (1 + agility * movementSpeedPerAgi));
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
            mainCamera.GetComponent<CameraManager>().screenShake(0.1f, 0.14f);
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

    public void addPlayerStats(float strengthGain, float agilityGain, float intelligenceGain) {
        strength = Mathf.Max(strength + strengthGain, 0);
        agility = Mathf.Max(agility + agilityGain, 0);
        intelligence = Mathf.Max(intelligence + intelligenceGain, 0);
        updateManaAndHealth();
    }

    public void multiplyPlayerStats(float strengthMult, float agilityMult, float intelligenceMult) {
        strength = Mathf.Max(strength * strengthMult, 0);
        agility = Mathf.Max(agility * agilityMult, 0);
        intelligence = Mathf.Max(intelligence * intelligenceMult, 0);
        updateManaAndHealth();
    }

    public void updateManaAndHealth() {
        maxMana = baseMana + intelligence * manaMaxGainPerInt;
        maxHP = baseMaxHP + (int)(strength / nrOfStrPerMaxHealth);
        updateHealthBar();
    }
}
