using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHomingBullet : EnemyBullet {
    private Rigidbody2D rigidbody;
    public float homingForce;
    public GameObject playerObject;


    // Start is called before the first frame update
    void Start() {
        damage = 1;
        homingForce = 50;
        pushForce = 5;
        bulletLifeTime = 1;
        Animator anim = GetComponentInChildren<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        Destroy(gameObject, bulletLifeTime);
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, GetComponent<Rigidbody2D>().velocity.normalized);
        //Debug.DrawLine(transform.position, transform.position + new Vector3(GetComponent<Rigidbody2D>().velocity.normalized.x, GetComponent<Rigidbody2D>().velocity.normalized.y));
    }

    void FixedUpdate() {
        GetComponent<Rigidbody2D>().AddForce(homingForce * (playerObject.transform.position - transform.position).normalized);
    }
}
