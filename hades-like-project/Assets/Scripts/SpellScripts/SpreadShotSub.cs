using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShotSub : Spell {
    // Start is called before the first frame update

    public int waveDirection = 1;
    public float timeSinceSpawn = 0.0f;
    public Vector3 moveVec;

    private void Awake() {
    }

    void Start() {
        setRotationTowardsVector(moveVec);
        Destroy(gameObject, 0.5f * rangeMultiplier);
    }

    // Update is called once per frame
    void FixedUpdate() {
        timeSinceSpawn += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.transform.tag) {
            case "Enemy":
                other.gameObject.GetComponent<Enemy>().takeDamage(damage);
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
