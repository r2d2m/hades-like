using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    float movementSpeed = 5000f;
    float movementAcc = 0.1f;
    float bulletForce = 1000f;

    public GameObject playerGun;
    public GameObject playerBullet;

    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerMovement();
        playerAim();
        playerActions();
    }

    void playerMovement(){
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        rigidBody.AddForce(movement*movementSpeed);
    }

    void playerAim(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 deltaVec = mousePos - transform.position;

        playerGun.transform.position = transform.position + Vector3.Normalize(deltaVec) * 0.4f;
        Debug.DrawLine(transform.position, mousePos, Color.red);
    }

    void playerActions(){
        // SHoot!
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(1)){
            print("Shooting");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 deltaVec = mousePos - transform.position;
            GameObject newBullet = Instantiate(playerBullet, playerGun.transform.position, playerGun.transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(deltaVec.normalized * bulletForce);
        }
    }
}
