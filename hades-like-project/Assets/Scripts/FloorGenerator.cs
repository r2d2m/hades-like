using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour {

    List<GameObject> roomList;
    GameObject[] roomArray;
    int currentFloor;
    // Start is called before the first frame update
    void Start() {
        GameObject newRoom = Instantiate(getRandomRoom(1,1), new Vector3(0,0,0), new Quaternion(0,0,0,0));
        newRoom.transform.parent = transform;
    }

    // Update is called once per frame
    void Update() {

    }

    void generateFloor() {
    }

    // difficulty 1 = normal, 2 = hard
    GameObject getRandomRoom(int difficulty, int floorNumber){
        string roomType = "";
        switch(difficulty){
            case 1:
                roomType = "NormalRooms";
                break;
            case 2: 
                roomType = "HardRooms";
                break;
            default:
                break;
        }
        roomArray = Resources.LoadAll<GameObject>("Rooms/Floor" + floorNumber + "/" + roomType);
        return roomArray[Random.Range(0, roomArray.Length-1)];
    }
}
