using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet {
    Transform floorTransform;

    void Awake() {
        floorTransform = GameObject.FindGameObjectWithTag("Floor").transform;
        transform.parent = floorTransform.transform;
        Destroy(gameObject, lifeTime);
    }
}
