using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour {

    public GameObject heartPrefab;
    public GameObject emptyHeartPrefab;
    public GameObject heartContainer;
    public GameObject emptyContainer;

    float heartSpacing = 20f;
    // Start is called before the first frame update
    void Awake() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void setCurrentHP(int HP) {
        foreach (Transform child in heartContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < HP; i++) {
            GameObject heart = Instantiate(heartPrefab, new Vector3(heartSpacing * i + heartSpacing / 2, 0, 0), transform.rotation);
            heart.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            heart.transform.SetParent(heartContainer.transform, false);
        }
    }

    public void setMaxHP(int HP) {
        foreach (Transform child in emptyContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < HP; i++) {
            GameObject emptyHeart = Instantiate(emptyHeartPrefab, new Vector3(heartSpacing * i + heartSpacing / 2, 0, 0), transform.rotation);
            emptyHeart.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            emptyHeart.transform.SetParent(emptyContainer.transform, false);
        }
    }
}
