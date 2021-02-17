using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonField : PlayerHazard {
    public void setValues(int damage, float lifeTime) {
        this.damage = damage;
        Destroy(gameObject, lifeTime);
    }
}
