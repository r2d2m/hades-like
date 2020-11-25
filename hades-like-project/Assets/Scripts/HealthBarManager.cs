using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{

    public GameObject heartPrefab;
    public GameObject emptyHeartPrefab;

    float heartSpacing;
    // Start is called before the first frame update
    void Start()
    {
        heartSpacing = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCurrentHP(int HP){
        for(int i = 0; i < HP; i++){
            GameObject heart = Instantiate(heartPrefab, new Vector3(heartSpacing * i + heartSpacing/2, 205, 0), transform.rotation);
            heart.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            heart.transform.SetParent(transform, false);
        }
    }

    public void setMaxHP(int HP){
        for(int i = 0; i < HP; i++){
            GameObject emptyHeart = Instantiate(emptyHeartPrefab, new Vector3(heartSpacing * i + heartSpacing/2, 205, 0), transform.rotation);
            emptyHeart.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            emptyHeart.transform.SetParent(transform, false);
        }
    }
}
