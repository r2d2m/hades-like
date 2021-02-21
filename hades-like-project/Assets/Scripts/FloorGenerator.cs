using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour {

    GameObject[] roomArray;

    public GameObject getRandomRewardRoom(int floorNumber) {
        roomArray = Resources.LoadAll<GameObject>("Rooms/Floor" + floorNumber + "/RewardRooms");
        return roomArray[Random.Range(0, roomArray.Length)];
    }

    // difficulty 1 = normal, 2 = hard
    public GameObject getRandomRoom(int difficulty, int floorNumber) {
        string roomType = "";
        switch (difficulty) {
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
        return roomArray[Random.Range(0, roomArray.Length)];
    }
}
