using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public bool mouseOverThis = false;

    public bool mouseIsOverSlot() {
        return mouseOverThis;
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        mouseOverThis = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
        mouseOverThis = false;
    }

}