using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointerScript : MonoBehaviour
{

    void Update() {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
