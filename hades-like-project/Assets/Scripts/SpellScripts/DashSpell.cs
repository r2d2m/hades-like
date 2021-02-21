using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSpell : Spell
{

    private void Awake() {
        cooldownTime = 2;     
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        playerGameObject.GetComponent<Rigidbody2D>().AddForce(getMouseDeltaVector().normalized * 6500);
    }
}
