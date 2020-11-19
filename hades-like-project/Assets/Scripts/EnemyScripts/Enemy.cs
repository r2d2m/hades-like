using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Parent class for all enemies, should include things that all enemies share!
*/
public class Enemy : MonoBehaviour
{
    protected float maxHP;
    protected float currentHP;
    protected GameObject player;
    protected int collisionDamage = 0;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 
    public void takeDamage(float damage){
        currentHP -= damage;
    }

    public void heal(){
        
    }

    public int getCollisionDamage(){
        return collisionDamage;
    }
}
