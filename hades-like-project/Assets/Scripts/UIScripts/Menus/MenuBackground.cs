using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBackground : MonoBehaviour
{
    public GameObject mousePointer;

    public GameObject firstLayer;
    public GameObject secondLayer;
    public GameObject thirdLayer;

    public float firstMovementFraction = 0.1f;
    public float secondMovementFraction = 0.05f;
    public float thirdMovementFraction = 0.025f;

    
    // Update is called once per frame
    void Update()
    {
        firstLayer.transform.position = -mousePointer.transform.position*firstMovementFraction;
        secondLayer.transform.position = -mousePointer.transform.position*secondMovementFraction;
        thirdLayer.transform.position = -mousePointer.transform.position*thirdMovementFraction;
    }
}
