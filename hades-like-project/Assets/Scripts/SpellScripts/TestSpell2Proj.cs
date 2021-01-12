using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpell2Proj : Spell {
    public GameObject explosionObject;
    float livedTime = 0;
    // Start is called before the first frame update

    private void Awake() {
        cooldownTime = 1;
    }

    void Start() {
        transform.position = gunGameObject.transform.position;
        GetComponent<Rigidbody2D>().AddForce(getMouseVector() * 1000);
        setRotationTowardsVector(mousePos - playerPos);
    }

    // Update is called once per frame
    void Update() {
        livedTime += Time.deltaTime;
        if(livedTime > rangeMultiplier * 1.0f){
            explode();
            Destroy(gameObject);
        }
    }

    void explode() {
        GameObject explosion = Instantiate(explosionObject, transform.position, transform.rotation);
        explosion.transform.parent = transform.parent;
        explosion.GetComponent<Spell>().damage = 1 * damageMultiplier;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.transform.tag) {
            case "Enemy":
                explode();
                Destroy(gameObject);
                break;
        }
    }
}
