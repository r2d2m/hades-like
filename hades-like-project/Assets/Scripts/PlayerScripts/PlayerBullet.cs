using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {
    public GameObject explodeEffect;
    public float damage;
    public AudioClip[] fireSounds;
    GameObject mainCamera;
    Transform floorTransform;

    // Set a lifetime timer

    private void Awake() {
        mainCamera = GameObject.Find("MainCamera");
        floorTransform = GameObject.FindGameObjectWithTag("Floor").transform;
        transform.parent = floorTransform.transform;
    }

    void Start() {
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update() {

    }

    public void setDamage(float inputDamage) {
        damage = inputDamage;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // Collide with enemies
        //TODO: Damage multiple types of enemies!
        if (other.transform.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy>().takeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        // Create particle effects
        Instantiate(explodeEffect, transform.position, transform.rotation);
    }

    public AudioClip getFireSound() {
        int randomIndex = Random.Range(0, fireSounds.Length - 1);
        return fireSounds[randomIndex];
    }
}
