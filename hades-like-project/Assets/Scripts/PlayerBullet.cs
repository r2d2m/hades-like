using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public GameObject explodeEffect;
    GameObject mainCamera;

    // Set a lifetime timer
    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // Collide with enemies
        //TODO: Damage multiple types of enemies!
        if(other.transform.tag == "Enemy"){
            other.gameObject.GetComponent<Enemy>().takeDamage(1);
            Destroy(gameObject);
        }   
    }

    private void OnDestroy() {
        // Create particle effects
        Instantiate(explodeEffect, transform.position, transform.rotation);
    }
}
