using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPetalManager : MonoBehaviour {
    public GameObject flowerPetalPrefab;
    public List<GameObject> flowerPetals;
    int currentHP = 0;
    int maxHP = 0;
    Color damagedColor = new Color(0.2f, 0, 0);

    // Start is called before the first frame update
    void Awake() {
        flowerPetals = new List<GameObject>();
    }

    public void setCurrentHP(int newCurrentHP) {
        for (int i = 0; i < maxHP; i++) {
            if (i < newCurrentHP) {
                flowerPetals[i].GetComponent<SpriteRenderer>().color = Color.white;
            } else {
                flowerPetals[i].GetComponent<SpriteRenderer>().color = damagedColor;
            }
        }
        currentHP = newCurrentHP;
    }

    public void setMaxHP(int newMaxHP) {
        if (newMaxHP > maxHP) {
            Vector3 offsetVec = new Vector3(0, 0.5f, 0);
            Quaternion currentRotation = new Quaternion();

            for (int i = 0; i < maxHP; i++) {
                currentRotation = Quaternion.Euler(0, 0, 360 / newMaxHP * -i);
                flowerPetals[i].transform.position = transform.position + currentRotation * offsetVec;
                flowerPetals[i].transform.rotation = currentRotation;
            }

            for (int i = maxHP; i < newMaxHP; i++) {
                currentRotation = Quaternion.Euler(0, 0, 360 / newMaxHP * -i);
                flowerPetals.Add(Instantiate(flowerPetalPrefab, transform.position + currentRotation * offsetVec, currentRotation, transform));

            }

        } else if (newMaxHP < maxHP) {
            Vector3 offsetVec = new Vector3(0, 0.5f, 0);
            Quaternion currentRotation = new Quaternion();
            Destroy(flowerPetals[flowerPetals.Count - 1]);
            flowerPetals.RemoveAt(flowerPetals.Count - 1);

            for (int i = 0; i < newMaxHP; i++) {
                currentRotation = Quaternion.Euler(0, 0, 360 / newMaxHP * -i);
                flowerPetals[i].transform.position = transform.position + currentRotation * offsetVec;
                flowerPetals[i].transform.rotation = currentRotation;
            }
        }
        maxHP = newMaxHP;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            setMaxHP(maxHP + 1);
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            setMaxHP(maxHP - 1);
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            setCurrentHP(currentHP + 1);
        }

        if (Input.GetKeyDown(KeyCode.N)) {
            setCurrentHP(currentHP - 1);
        }
    }
}
