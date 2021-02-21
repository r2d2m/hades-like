using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : EventTrigger {
    private bool currentlyDragging;
    public int currentSpellSlotIndex = -1;
    public Vector3 originalPos = Vector3.zero;
    
    public override void OnPointerDown(PointerEventData data) {
        currentlyDragging = true;
        GetComponentInParent<InventoryManager>().setRaycastTargets(false);
        print(currentSpellSlotIndex);
    }

    public override void OnPointerUp(PointerEventData data) {
        currentlyDragging = false;
        GetComponentInParent<InventoryManager>().spellDragAndDropped(currentSpellSlotIndex);
        GetComponentInParent<InventoryManager>().setRaycastTargets(true);
        //transform.position = originalPos;
    }

    void Update() {
        if (currentlyDragging) {
            Vector2 newPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            transform.position = new Vector3(newPos.x, newPos.y, 0);
        }
    }
}
