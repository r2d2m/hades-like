using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBeamSub : Spell {
    // Start is called before the first frame update

    public int waveDirection = 1;
    public Vector3 mouseVector;
    public float timeSinceSpawn = 0.0f;

    private void Awake() {
        cooldownTime = 0.3f;
    }

    void Start() {
        GetComponent<Rigidbody2D>().AddForce(mouseVector * 1300);
        Destroy(gameObject, 0.5f * rangeMultiplier);
    }

    // Update is called once per frame
    void FixedUpdate() {
        timeSinceSpawn += Time.deltaTime;
        //GetComponent<Rigidbody2D>().AddForce(waveDirection * transform.up * Mathf.Sin(timeSinceSpawn * 10f) * 5000f * Time.deltaTime);
        //transform.position = Vector2.MoveTowards(transform.position, transform.up * Mathf.Sin(Time.deltaTime) * 10, 10 * Time.deltaTime);
        transform.position = transform.position + waveDirection * transform.up * Mathf.Sin(timeSinceSpawn * 20f) * 0.18f;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.transform.tag) {
            case "Enemy":
                other.gameObject.GetComponent<Enemy>().takeDirectionalDamage(damage, transform.position);
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
