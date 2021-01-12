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
    GameObject player;
    GameObject mainCamera;
    GameObject currentDoor;


    // Start is called before the first frame update
    void Start() {
        currentRooms = new List<GameObject>();
        currentRoomClear = false;
        enemiesAliveInRoom = 0;
        currentFloorDepth = 1;
        currentRoomDepth = 1;
        floorGenerator = GetComponent<FloorGenerator>();
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.Find("MainCamera");
        initFloor();
    }

    // Update is called once per frame
    void Update() {
        // TODO: Debug shit
        debugRoomManager();
    }

    public void enemyDeath() {
        enemiesAliveInRoom--;
        print("Enemies alive: " + enemiesAliveInRoom);

        if (enemiesAliveInRoom <= 0) {
            roomIsClear();
        }
    }

    public void addEnemiesIntoRoom(int count) {
        print("Enemies added: " + count);
        enemiesAliveInRoom += count;
    }

    public void roomIsClear() {
        openRoomDoors(activeRoom);
        spawnRandomRewards(1, 1);
        currentRoomClear = true;
    }

    public void openRoomDoors(GameObject room) {
        currentDoor = GameObject.FindGameObjectWithTag("Door");
        currentDoor.GetComponent<DoorScript>().openDoor();
    }

    public void doorButtonClicked(){
        playerExitDoor();
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
            goToNextRoom();
        }
    }

    void goToNextRoom() {
        activeRoomID += 1;
        print("DEBUG: go to next room: " + activeRoomID);
        mainCamera.GetComponent<CameraManager>().roomChangeEffect(blackFadeSpeed, blackScreenDuration);
        StartCoroutine(fadeToNextRoom(1, 1));
    }

    private IEnumerator fadeToNextRoom(int difficulty, int floorNumber) {
        currentRoomDepth++;
        yield return new WaitForSeconds(1.0f / blackFadeSpeed);
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
        createRoom(difficulty, floorNumber);
        movePlayerToEntrance();
        float[] cameraBounds = getCameraBoundsCollider();
        mainCamera.GetComponent<CameraManager>().setCameraBounds(cameraBounds[0], cameraBounds[1]);
        moveCameraToPlayer(cameraBounds[0], cameraBounds[1]);
    }

    void moveCameraToPlayer(float boundX, float boundY) {
        float cameraX = player.transform.position.x;
        float cameraY = player.transform.position.y;
        cameraX = Mathf.Min(cameraX, boundX / 4);
        cameraX = Mathf.Max(cameraX, -boundX / 4);
        cameraY = Mathf.Min(cameraY, boundY / 4);
        cameraY = Mathf.Max(cameraY, -boundY / 4);
        mainCamera.GetComponent<CameraManager>().setCameraPosition(cameraX, cameraY);
    }

    void createRoom(int difficulty, int floorNumber) {
        activeRoom = Instantiate(floorGenerator.getRandomRoom(difficulty, floorNumber), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        activeRoom.transform.parent = transform;
        enemiesAliveInRoom = countAliveEnemies(activeRoom);
    }

    void initFloor() {
        activeRoomID = 0;
        createRoom(1, 1);
        enemiesAliveInRoom = countAliveEnemies(activeRoom);
        movePlayerToEntrance();
        float[] cameraBounds = getCameraBoundsCollider();
        mainCamera.GetComponent<CameraManager>().setCameraBounds(cameraBounds[0], cameraBounds[1]);
        moveCameraToPlayer(cameraBounds[0], cameraBounds[1]);
    }


    public void spawnRandomRewards(int count, int tier) {
        for (int i = 0; i < count; i++) {
            GameObject newReward = Instantiate(getRandomReward(tier), new Vector3(count, 0, 0), new Quaternion(0, 0, 0, 0));
            newReward.transform.parent = transform;
        }
    }

    public GameObject getRandomReward(int tier) {
        GameObject[] rewards = Resources.LoadAll<GameObject>("Rewards/Tier" + tier);
        return rewards[Random.Range(0, rewards.Length)];
    }

    public void movePlayerToEntrance() {
        Vector3 entrancePos = new Vector3(0, 0, 0);
        foreach (Transform child in activeRoom.transform) {
            if (child.CompareTag("RoomEntrance")) {
                entrancePos = child.position;
            }
        }
        player.transform.position = entrancePos + new Vector3(0, 0.5f, 0);
    }

    public float[] getCameraBoundsCollider() {
        float[] dimensions = new float[2];
        GameObject cameraBounds = GameObject.FindGameObjectWithTag("CameraBounds");
        if (cameraBounds != null) {
            dimensions[0] = cameraBounds.GetComponent<BoxCollider2D>().size.x;
            dimensions[1] = cameraBounds.GetComponent<BoxCollider2D>().size.y;
        } else {
            dimensions[0] = 0;
            dimensions[1] = 0;
        }
        print(dimensions[0]);
        print(dimensions[1]);
        return dimensions;
    }

    public void playerExitDoor() {
        if (currentRoomClear) {
            goToNextRoom();
        }
    }
}
