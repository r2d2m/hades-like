using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointerScript : MonoBehaviour {

    public GameObject mouseHand;
    public GameObject mouseAim;

    void Start() {

    }

    void Update() {
        Cursor.visible = false;
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void setMouseHand() {
        mouseHand.SetActive(true);
        mouseAim.SetActive(false);
    }

    public void setMouseAim() {
        mouseHand.SetActive(false);
        mouseAim.SetActive(true);
    }
}
