using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointerScript : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
