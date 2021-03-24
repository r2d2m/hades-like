using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstNeedlesSub : Spell {
    // Start is called before the first frame update

    public int waveDirection = 1;
    public float timeSinceSpawn = 0.0f;
    public Vector3 moveVec;
    GameObject targetEnemy = null;
    Vector3 goalPos;
    float closestDistance = Mathf.Infinity;
    float maxDist;

    private void Awake() {
    }

    void Start() {
        goalPos = transform.position;
        setRotationTowardsVector(moveVec);
        Destroy(gameObject, 10 * rangeMultiplier);
        maxDist = 13 * rangeMultiplier;
    }

    private void Update() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn >= 0.5f) {
            decideTarget();
            if (targetEnemy != null) {
                GetComponent<Rigidbody2D>().AddForce((targetEnemy.transform.position - transform.position).normalized * 200);
            } else {
                GetComponent<Rigidbody2D>().AddForce((goalPos - transform.position).normalized * 200);
                if ((goalPos - transform.position).magnitude < 0.2f) {
                    Destroy(gameObject);
                }
            }
        }

        if (targetEnemy) {
            if (targetEnemy.GetComponent<Enemy>().currentHP <= 0) {
                Destroy(gameObject);
            }
        }
    }

    private void decideTarget() {
        Vector3 targetVec = Vector3.zero;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            targetVec = (enemy.transform.position - transform.position);
            if (targetVec.magnitude < closestDistance && targetVec.magnitude < maxDist) {
                targetEnemy = enemy;
                goalPos = targetEnemy.transform.position;
                closestDistance = targetVec.magnitude;
            }
        }
        setRotationTowardsVector(targetVec);

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
