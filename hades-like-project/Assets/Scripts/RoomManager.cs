﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public GameObject currentFloor;
    List<GameObject> currentRooms;
    int activeRoomID;
    
    // Fade effect length
    float blackFadeSpeed = 4f;
    float blackScreenDuration = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        currentRooms = new List<GameObject>();

        initFloor();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Delete this later!
        debugRoomManager();
    }

    void debugRoomManager(){
        if(Input.GetKeyDown(KeyCode.X)){
            activeRoomID += 1;
            print("DEBUG: go to next room: " + activeRoomID);
            GameObject.Find("MainCamera").GetComponent<EffectManager>().roomChangeEffect(blackFadeSpeed, blackScreenDuration);
            StartCoroutine(activateRoom(activeRoomID));
        }
    }

    private IEnumerator activateRoom(int roomID){
        yield return new WaitForSeconds(1.0f/blackFadeSpeed);
        for(int i = 0; i < currentRooms.Count; i++){
            if(i == activeRoomID){
                currentRooms[i].SetActive(true);
            }else{
                currentRooms[i].SetActive(false);
            }
        }
    }

    void initFloor(){
        // Get references to all rooms in current floor
        foreach(Transform child in currentFloor.transform){
            currentRooms.Add(child.gameObject);
            // Set all Rooms to middle of SPACE
            child.gameObject.transform.position = new Vector3(0,0,0);
            print(child.gameObject);
        }

        // Set all rooms except first to inactive        
        for(int i = 1; i < currentRooms.Count; i++){
            currentRooms[i].SetActive(false);
        } 

        activeRoomID = 0;
    }
}
