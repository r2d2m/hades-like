using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFlowerManger : MonoBehaviour {
    public GameObject flowerPetalContainer;
    public GameObject healthFace;
    public Sprite[] flowerFaces;
    public int maxHP;
    public int currentHP;

    FlowerPetalManager petalManager;
    // Start is called before the first frame update
    void Awake() {
        petalManager = flowerPetalContainer.GetComponent<FlowerPetalManager>();
    }

    public void setCurrentHP(int HP) {
        petalManager.setCurrentHP(HP);
        currentHP = HP;
        float percentHP = (float) currentHP / maxHP;
        int newFaceIndex = (int) ((flowerFaces.Length - 1) * percentHP);
        print(((flowerFaces.Length - 1) * (currentHP / maxHP)));
        print(newFaceIndex);
        healthFace.GetComponent<SpriteRenderer>().sprite = flowerFaces[newFaceIndex];
    }

    public void setMaxHP(int HP) {
        petalManager.setMaxHP(HP);
        maxHP = HP;
    }
}
