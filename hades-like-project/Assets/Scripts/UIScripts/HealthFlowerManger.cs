using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFlowerManger : MonoBehaviour {
    public GameObject flowerPetalContainer;
    public GameObject healthFace;

    FlowerPetalManager petalManager;
    // Start is called before the first frame update
    void Awake() {
        petalManager = flowerPetalContainer.GetComponent<FlowerPetalManager>();
        print(petalManager);
    }

    public void setCurrentHP(int HP) {
        petalManager.setCurrentHP(HP);
    }

    public void setMaxHP(int HP) {
        petalManager.setMaxHP(HP);
    }
}
