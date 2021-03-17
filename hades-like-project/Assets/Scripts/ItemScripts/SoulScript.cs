using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulScript : MonoBehaviour {
    GameObject player;
    float speed = 0.6f;
    float range = 100000;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        if ((player.transform.position - transform.position).magnitude < range) {
            Vector2 deltaVec = (player.transform.position - transform.position);
            GetComponent<Rigidbody2D>().AddForce(deltaVec.normalized * speed * Time.deltaTime);
            //GetComponent<Rigidbody2D>().velocity = deltaVec * speed;
            //speed += 0.05f;
        }
    }

    public void DestroyMe() {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2);
    }
}
