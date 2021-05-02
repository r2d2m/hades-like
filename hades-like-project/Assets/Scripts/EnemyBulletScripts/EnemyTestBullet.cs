using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestBullet : EnemyBullet
{
    // Start is called before the first frame update
    void Start() {
        Animator anim = GetComponentInChildren<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        Destroy(gameObject, bulletLifeTime);
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, GetComponent<Rigidbody2D>().velocity.normalized);
        //Debug.DrawLine(transform.position, transform.position + new Vector3(GetComponent<Rigidbody2D>().velocity.normalized.x, GetComponent<Rigidbody2D>().velocity.normalized.y));
    }
}
