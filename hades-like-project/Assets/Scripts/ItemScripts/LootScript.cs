using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootScript : MonoBehaviour {
    GameObject player;
    float speed = 0.3f;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate() {
        if ((player.transform.position - transform.position).magnitude < 1000) {
            Vector2 deltaVec = (player.transform.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(deltaVec * speed * Time.deltaTime);
            //GetComponent<Rigidbody2D>().velocity = deltaVec * speed;
            //speed += 0.05f;
        }
    }

    public void DestroyMe() {
        Destroy(gameObject);
    }
}
