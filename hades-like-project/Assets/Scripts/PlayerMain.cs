using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    GameObject mainCamera;

    float movementSpeed = 5000f;
    float bulletForce = 1800f;
    float shootCD = 0.2f; 
    float specialCD = 1.0f; // Rightclick = special
    float currentShootCD;

    public GameObject playerGun;
    public GameObject playerBullet;

    private Rigidbody2D rigidBody;

    Vector2 movementVector;

    float invisCD = 1.0f;
    float currentInvisCD;

    int maxHP;
    int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = 5;
        currentHP = maxHP;
        currentShootCD = 0;

        currentInvisCD = 0;
        mainCamera = GameObject.Find("MainCamera");
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        updateCooldowns();
        playerMovement();
        playerAim();
        playerActions();

        // TODO: delete this later!
        debugPlayer();
    }

    void debugPlayer(){
        if(Input.GetKeyDown(KeyCode.Z)){
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Update all cooldowns
    void updateCooldowns(){
        if(currentShootCD > 0){
            currentShootCD -= Time.deltaTime;
        }else{
            currentShootCD = 0;
        }

        if(invisCD > 0){
            currentInvisCD -= Time.deltaTime;
        }else{
            currentInvisCD = 0;
        }
    }
    
    // All physics related stuff should be here!
    void FixedUpdate()
    {
        rigidBody.AddForce(movementVector*movementSpeed);
    }

    // Handle player input
    void playerMovement(){
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movementVector = new Vector2(moveHorizontal, moveVertical);
    }

    // Point the gun in the direction  of the mouse
    void playerAim(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 deltaVec = mousePos - transform.position;

        playerGun.transform.position = transform.position + Vector3.Normalize(deltaVec) * 0.3f;
        Debug.DrawLine(transform.position, mousePos, Color.red);
    }

    // Input for shooting and non-movement related input
    void playerActions(){
        // SHoot!
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButton(0) && currentShootCD <= 0){
            shoot(playerGun.transform.position, mousePos);
            currentShootCD = shootCD;
        }else if(Input.GetMouseButton(1) && currentShootCD <= 0){
            Vector3 randSpawn, randTarget;
            float spawnOffset = 0.3f;
            float targetOffset = 0.5f;
            for(int i = 0; i < 4; i++){
                randSpawn = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0.0f);
                randTarget = randSpawn = new Vector3(Random.Range(-targetOffset, targetOffset), Random.Range(-targetOffset, targetOffset), 0.0f);
                shoot(playerGun.transform.position + randSpawn, mousePos + randTarget);
            }
            currentShootCD = specialCD;
        }
    }

    // Function for firing a bullet from spawnPos to targetPos 
    // TODO: Shoot different bullets!
    void shoot(Vector2 spawnPos, Vector2 targetPos){
        mainCamera.GetComponent<CameraManager>().screenShake(0.08f,0.05f);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 deltaVec = mousePos - transform.position;
        GameObject newBullet = Instantiate(playerBullet, spawnPos, Quaternion.Euler(0,0, Random.Range(0, 360)));
        newBullet.GetComponent<Rigidbody2D>().AddForce(deltaVec.normalized * bulletForce);
    }

    // Heal damage
    void healDamage(int amount){
        currentHP += amount;
        if(currentHP > maxHP){
            currentHP = maxHP;
        }
    }

    // Take damage 
    void takeDamage(int amount){
        if(currentInvisCD <= 0){
            currentHP -= amount;
            currentInvisCD = invisCD;
            print("Took: " + amount + " damage, " + currentHP + "/" + maxHP + " HP left!");
            if(currentHP <= 0){
                gameOver();
            }
        }
    }

    // Call on this when you are DEAD
    void gameOver(){
        print("Game Over :(");
        mainCamera.GetComponent<CameraManager>().setBackground(Color.red);
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemy"){
            int recievedDamage = other.gameObject.GetComponent<Enemy>().getCollisionDamage();
            if(recievedDamage > 0){
                takeDamage(recievedDamage);
            }
        }
    }

    public Vector3 getPlayerPosition(){
        return transform.position;
    }
}
