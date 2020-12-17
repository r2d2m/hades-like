using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon {
    public float bulletBaseForce = 1.0f;
    public float bulletForceMod = 1.0f;
    public int bulletPierceCount = 1;

    public void setBulletForce(Vector2 forceDirection, float playerBulletForce) {
        //print(forceDirection * playerBulletForce * bulletForceMod);
        GetComponent<Rigidbody2D>().AddForce(forceDirection * (bulletBaseForce + playerBulletForce * bulletForceMod));
    }

    public void setLifeTime(float playerLifeTimeMod) {
        lifeTime = playerLifeTimeMod + baseLifeTime;
    }

    protected void setBulletBaseForce(float force) {
        this.bulletBaseForce = force;
    }

    public void setBulletPierceCount(int count){
        this.bulletPierceCount = count;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Collide with enemies
        //TODO: Damage multiple types of enemies!
        if (other.transform.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy>().takeDamage(weaponDamage);
            print(GetComponent<Rigidbody2D>().velocity);
            other.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * enemyPushForce;

            bulletPierceCount--;
            if(bulletPierceCount <= 0){
                Destroy(gameObject);
            }
        }else{
            Destroy(gameObject);
        }
    }
}
