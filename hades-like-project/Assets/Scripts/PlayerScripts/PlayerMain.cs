using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour {
    GameObject mainCamera;

    public GameObject soundManagerObject;
    SoundManager soundManager;


    float movementSpeed = 5000f;
    float bulletForce = 1800f;
    float primaryCD = 0.2f;
    float altCD = 1.0f; // Rightclick = special
    float currentprimaryCD;

    public GameObject playerGun;
    public GameObject primBullet;
    public GameObject altBullet;
    public GameObject healthBar;

    private Rigidbody2D rigidBody;

    Vector2 movementVector;

    float invisCD = 1.0f;
    float currentInvisCD;

    int maxHP;
    int currentHP;

    float primBaseDamage;
    float primDamageMultiplier;
    float altBaseDamage;
    float altDamageMultiplier;

    float minPrimCD = 0.0001f;
    float minAltCD = 0.0001f;
    float minPrimDMG = 0.5f;
    float minAltDMG = 0.5f;
    float minPrimMULT = 0.5f;
    float minAltMULT = 0.5f;
    
    // Start is called before the first frame update
    void Start() {
        maxHP = 5;
        currentHP = maxHP;
        primBaseDamage = 1;
        primDamageMultiplier = 1;
        altBaseDamage = 1;
        altDamageMultiplier = 1;

        updateHealthBar();

        currentprimaryCD = 0;
        currentInvisCD = 0;
        mainCamera = GameObject.Find("MainCamera");
        rigidBody = GetComponent<Rigidbody2D>();
        soundManager = soundManagerObject.GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update() {
        updateCooldowns();
        playerMovement();
        playerAim();
        playerActions();

        // TODO: delete this later!
        debugPlayer();
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
    }

    // Point the gun in the direction  of the mouse
    void playerAim() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 deltaVec = mousePos - transform.position;

        playerGun.transform.position = transform.position + Vector3.Normalize(deltaVec) * 0.3f;
        Debug.DrawLine(transform.position, mousePos, Color.red);
    }

    // Input for shooting and non-movement related input
    void playerActions() {
        // SHoot!
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && currentprimaryCD <= 0) {
            shoot(primBullet, playerGun.transform.position, mousePos);
            currentprimaryCD = primaryCD;
        } else if (Input.GetMouseButton(1) && currentprimaryCD <= 0) {
            Vector3 randSpawn, randTarget;
            float spawnOffset = 0.3f;
            float targetOffset = 0.5f;
            for (int i = 0; i < 4; i++) {
                randSpawn = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0.0f);
                randTarget = randSpawn = new Vector3(Random.Range(-targetOffset, targetOffset), Random.Range(-targetOffset, targetOffset), 0.0f);
                shoot(primBullet, playerGun.transform.position + randSpawn, mousePos + randTarget);
            }
            currentprimaryCD = altCD;
        }
    }

    // Function for firing a bullet from spawnPos to targetPos 
    // TODO: Shoot different bullets!
    void shoot(GameObject bullet, Vector2 spawnPos, Vector2 targetPos) {
        mainCamera.GetComponent<CameraManager>().screenShake(0.08f, 0.05f);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 deltaVec = mousePos - transform.position;
        GameObject newBullet = Instantiate(bullet, spawnPos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        newBullet.GetComponent<PlayerBullet>().setDamage(primBaseDamage * primDamageMultiplier);
        newBullet.GetComponent<Rigidbody2D>().AddForce(deltaVec.normalized * bulletForce);
        soundManager.playSoundClip(newBullet.GetComponent<PlayerBullet>().getFireSound(), 0.1f);
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
        switch (other.gameObject.tag) {
            case "Enemy":
                int recievedDamage = other.gameObject.GetComponent<Enemy>().getCollisionDamage();
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
        }
    }

    public Vector3 getPlayerPosition() {
        return transform.position;
    }

    public void modifyPlayerDamage(float primDmg, float primMult, float altDmg, float altMult) {
        if (primBaseDamage + primDmg > minPrimDMG) {
            primBaseDamage += primDmg;
        }else{
            primBaseDamage = minPrimDMG;
        }

        if (primDamageMultiplier + primMult > minPrimMULT) {
            primDamageMultiplier += primMult;
        }else{
            primBaseDamage = minPrimMULT;
        }

        if (altBaseDamage + altDmg > minAltDMG) {
            altBaseDamage += altDmg;
        }else{
            altBaseDamage = minAltDMG;
        }

        if (altDamageMultiplier + altMult > minAltMULT) {
            altDamageMultiplier += altMult;
        }else{
            altDamageMultiplier = minAltMULT;
        }
    }

    public void modifyPlayerHealth(int maxHealthMod){
        maxHP += maxHealthMod;
        currentHP += maxHealthMod;
        if(currentHP > maxHP){
            currentHP = maxHP;
        }

        updateHealthBar();

        if(maxHP <= 0){
            gameOver();
        }
    }

    public void modifyCooldowns(float primaryCooldown, float altCooldown){
        if(primaryCD - primaryCooldown > minPrimCD){
            primaryCD -= primaryCooldown;
        }else{
            primaryCD = minPrimCD;
        }

        if(altCD - altCooldown > minAltCD){
            altCD -= altCooldown;
        }else{
            altCD = minAltCD;
        }
    }
}
