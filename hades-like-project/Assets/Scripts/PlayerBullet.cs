using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    public GameObject explodeEffect;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy() {
        Instantiate(explodeEffect, transform.position, transform.rotation);
    }
}
