using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

    List<GameObject> currentRooms;
    bool currentRoomClear;
    int activeRoomID;
    GameObject activeRoom;
    int enemiesAliveInRoom;

    int currentFloorDepth;
    int currentRoomDepth;

    // Fade effect length
    float blackFadeSpeed = 4f;
    float blackScreenDuration = 0.3f;

    FloorGenerator floorGenerator;

    // Start is called before the first frame update
    void Start() {
        currentRooms = new List<GameObject>();
        currentRoomClear = false;
        enemiesAliveInRoom = 0;
        currentFloorDepth = 1;
        currentRoomDepth = 1;
        floorGenerator = GetComponent<FloorGenerator>();
        
        initFloor();
    }

    // Update is called once per frame
    void Update() {
        // TODO: Debug shit
        debugRoomManager();
    }

    public void enemyDeath() {
        enemiesAliveInRoom--;

        if (enemiesAliveInRoom <= 0) {
            openRoomDoors(activeRoom);
        }
    }

    public void openRoomDoors(GameObject room) {
        foreach (Transform door in room.transform.Find("Doors").transform) {
            if (door.tag == "Door") {
                door.GetComponent<DoorScript>().openDoor();
            }
        }
    }

    int countAliveEnemies(GameObject room) {
        int aliveEnemies = 0;
        foreach (Transform enemy in room.transform.Find("Enemies").transform) {
            if (enemy.tag == "Enemy") {
                aliveEnemies++;
            }
        }
        return aliveEnemies;
    }

    void debugRoomManager() {
        if (Input.GetKeyDown(KeyCode.X)) {
            activeRoomID += 1;
            print("DEBUG: go to next room: " + activeRoomID);
            GameObject.Find("MainCamera").GetComponent<CameraManager>().roomChangeEffect(blackFadeSpeed, blackScreenDuration);
            StartCoroutine(goToNextRoom(1,1));
        }
    }

    private IEnumerator goToNextRoom(int difficulty, int floorNumber) {
        currentRoomDepth++;
        yield return new WaitForSeconds(1.0f / blackFadeSpeed);
        foreach(Transform child in transform){
           GameObject.Destroy(child.gameObject);
        }
        createRoom(difficulty, floorNumber);
    }

    void initFloor() {
        activeRoomID = 0;
        createRoom(1,1);
        enemiesAliveInRoom = countAliveEnemies(activeRoom);
    }

    void createRoom(int difficulty, int floorNumber){
        activeRoom = Instantiate(floorGenerator.getRandomRoom(difficulty, floorNumber), new Vector3(0,0,0), new Quaternion(0,0,0,0));
        activeRoom.transform.parent = transform;
    }
}
