using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

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
    List<GameObject> rewardsInRoom;

    public GameObject startRoom;
    public GameObject debugRoom;
    public GameObject soulRewardPrefab;
    public bool DEBUG_startInStartRoom;
    public bool DEBUG_startInDebugRoom;

    // Start is called before the first frame update
    void Start() {
        currentRoomClear = false;
        enemiesAliveInRoom = 0;
        currentFloorDepth = 1;
        currentRoomDepth = 1;
        floorGenerator = GetComponent<FloorGenerator>();
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.Find("MainCamera");
        if (DEBUG_startInStartRoom) {
            initStartRoom();
        } else {
            initFloor();
        }
    }

    // Update is called once per frame
    void Update() {
        // TODO: Debug shit
        debugRoomManager();
    }

    public void enemyDeath(Vector3 enemyPosition, int rewardSouls) {
        enemiesAliveInRoom--;
        print("Enemies alive: " + enemiesAliveInRoom);
        GameObject newSouls = Instantiate(soulRewardPrefab, enemyPosition, Quaternion.identity);
        print("Souls:" + rewardSouls);
        ParticleSystem.EmissionModule em = newSouls.GetComponentInChildren<ParticleSystem>().emission;
        em.rateOverTime = rewardSouls * 2f;
        newSouls.GetComponent<SoulScript>().soulCount = rewardSouls;
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
        //spawnRandomRewards(1, 1);
        currentRoomClear = true;
    }

    public void openRoomDoors(GameObject room) {
        currentDoor = GameObject.FindGameObjectWithTag("Door");
        currentDoor.GetComponent<DoorScript>().openDoor();
    }

    public void doorButtonClicked() {
        print("DoorButtonClicked!");
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

        if (currentRoomDepth % 4 == 0) {
            //createRewardRoom(1, Random.Range(1, 4));
            createRewardRoom(1, 5);
        } else {
            createRoom(difficulty, floorNumber);
            StartCoroutine(disableAllEnemies(0.7f));
        }

        movePlayerToEntrance();
        setCameraAndGridBounds();
        player.GetComponent<PlayerMain>().resetCooldowns();
    }

    private IEnumerator disableAllEnemies(float disableTime) {
        foreach (Transform enemy in activeRoom.transform.Find("Enemies").transform) {
            enemy.gameObject.GetComponent<Enemy>().disableEnemy();
        }

        yield return new WaitForSeconds(disableTime);

        foreach (Transform enemy in activeRoom.transform.Find("Enemies").transform) {
            enemy.gameObject.GetComponent<Enemy>().enableEnemy();
        }
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
        if (DEBUG_startInDebugRoom) {
            activeRoom = Instantiate(debugRoom, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        } else {
            activeRoom = Instantiate(floorGenerator.getRandomRoom(difficulty, floorNumber), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        }
        activeRoom.transform.parent = transform;
        enemiesAliveInRoom = countAliveEnemies(activeRoom);
        setCameraAndGridBounds();
    }

    //TODO Stopping duplicate loot has a very bad solution
    void createRewardRoom(int floorNumber, int numberOfRewards) {
        activeRoom = Instantiate(floorGenerator.getRandomRewardRoom(floorNumber), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        activeRoom.transform.parent = transform;
        GameObject rewardSpawnPoint = GameObject.Find("RewardSpawnPoint");
        rewardsInRoom = new List<GameObject>(numberOfRewards);
        List<string> rewardNames = new List<string>();
        GameObject randomReward;
        for (int i = 0; i < numberOfRewards; i++) {
            int tries = 50;
            GameObject newReward = null;
            while (tries > 0) {
                randomReward = getRandomReward(1);
                if (!rewardNames.Contains((randomReward.name + "(Clone)"))) {
                    newReward = Instantiate(randomReward, rewardSpawnPoint.transform.position + new Vector3(1.3f * i - numberOfRewards * 0.65f, 0, 0), rewardSpawnPoint.transform.rotation);
                    break;
                }
                tries--;
            }
            rewardsInRoom.Add(newReward);
            rewardNames.Add(newReward.name);
        }
        currentRoomClear = true;
        print("Spawned " + numberOfRewards + " rewards!");

    }

    public void rewardWasGrabbed() {
        for (int i = 0; i < rewardsInRoom.Count; i++) {
            rewardsInRoom[i].GetComponent<Reward>().despawnSelf();
        }
        openRoomDoors(activeRoom);
    }

    void initFloor() {
        activeRoomID = 0;
        createRoom(1, 1);
        //createRewardRoom(1, 3);
        enemiesAliveInRoom = countAliveEnemies(activeRoom);
        movePlayerToEntrance();
        setCameraAndGridBounds();

    }

    void initStartRoom() {
        activeRoom = Instantiate(startRoom, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        activeRoom.transform.parent = transform;
        movePlayerToEntrance();
        setCameraAndGridBounds();
        //disableAllEnemies(0.7f);
    }

    public void exitStartRoom() {
        goToNextRoom();
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
        GameObject cameraBounds = activeRoom.transform.Find("CameraBounds").gameObject;

        // GameObject cameraBounds = GameObject.FindGameObjectWithTag("CameraBounds");
        if (cameraBounds != null) {
            dimensions[0] = cameraBounds.GetComponent<BoxCollider2D>().size.x;
            dimensions[1] = cameraBounds.GetComponent<BoxCollider2D>().size.y;
        } else {
            dimensions[0] = 0;
            dimensions[1] = 0;
        }
        return dimensions;
    }

    public float[] setCameraAndGridBounds() {
        float[] cameraBounds = getCameraBoundsCollider();
        mainCamera.GetComponent<CameraManager>().setCameraBounds(cameraBounds[0], cameraBounds[1]);
        GetComponent<Grid>().setGridWorld(cameraBounds[0], cameraBounds[1]);
        GetComponent<Grid>().CreateGrid();
        //TODO maybe dont move camera here??
        moveCameraToPlayer(cameraBounds[0], cameraBounds[1]);
        print(cameraBounds[0] + ", " + cameraBounds[1]);
        return cameraBounds;
    }

    public void playerExitDoor() {
        if (currentRoomClear) {
            goToNextRoom();
        }
    }
}
