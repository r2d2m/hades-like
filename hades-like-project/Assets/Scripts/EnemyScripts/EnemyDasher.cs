using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDasher : Enemy
{

    public GameObject corpsePrefab;

    Vector3 movementVector;
    Rigidbody2D rb;
    float dashCD;
    float currentDashCD;
    float dashStrength;
    bool doDash;

    // Start is called before the first frame update
    void Start()
    {
        collisionDamage = 1;
        maxHP = 3;
        currentHP = maxHP;
        doDash = false;
        dashStrength = 2000;
        dashCD = 0.8f;
        currentDashCD = dashCD;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement();

        // TODO: this is placeholder
        if(currentHP <= 0){
            die();
        }
    }

    void movement(){
        movementVector = (player.transform.position - transform.position).normalized;
        if(currentDashCD <= 0){
            doDash = true;
            currentDashCD = dashCD + Random.Range(-0.2f, 0.2f);
        }else{ 
            currentDashCD -= Time.deltaTime;
        }
    }

    private void FixedUpdate() {
        if(doDash){
            if(Random.Range(0,100) < 75){
                rb.AddForce(movementVector * dashStrength);
            }else{
                rb.AddForce(-movementVector * dashStrength);
            }
            doDash = false;
        }
    }

    void die(){
        GameObject corpse = Instantiate(corpsePrefab, transform.position, transform.rotation);
        corpse.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        corpse.transform.parent = transform.parent; // TODO CHANGE THIS!
        Destroy(gameObject);
        //TODO: Spawn corpse etc
    }
}
