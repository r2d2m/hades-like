using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public PlayerMain playerMain;
    public GameObject spellSlotPrefab;
    public GameObject spellIconPrefab;
    public List<GameObject> spellSlots;
    public List<GameObject> spellIcons;

    // Start is called before the first frame update
    public void initInventory() {
        spellSlots = new List<GameObject>();
        playerMain = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();
        for (int i = 0; i < 7; i++) {
            spellSlots.Add(Instantiate(spellSlotPrefab, transform.position + new Vector3(i - 7/2, 0, 0), Quaternion.identity, transform));
        }
        updateInventory();
    }

    public void updateInventory() {
        foreach(GameObject spellIcon in spellIcons){
            Destroy(spellIcon);
        }
        spellIcons.Clear();
        List<PlayerSpell> equippedSpells = playerMain.getEquippedSpells();
        for (int i = 0; i < equippedSpells.Count; i++) {
            spellIcons.Add(Instantiate(spellIconPrefab, spellSlots[i].transform.position, spellSlots[i].transform.rotation, transform));
            spellIcons[i].GetComponent<InventoryItem>().currentSpellSlotIndex = i;
            spellIcons[i].GetComponent<InventoryItem>().originalPos = spellSlots[i].GetComponent<RectTransform>().localPosition;
        }
    }

    public void setRaycastTargets(bool enabled) {
        List<PlayerSpell> equippedSpells = playerMain.getEquippedSpells();
        for (int i = 0; i < equippedSpells.Count; i++) {
            spellIcons[i].GetComponent<Image>().raycastTarget = enabled;
        }
    }

    public void switchSpellSlots(int firstSpell, int secondSpell) {
        print("switch: " + firstSpell + ", " + secondSpell);
        List<PlayerSpell> equippedSpells = playerMain.getEquippedSpells();
        if (firstSpell != secondSpell && firstSpell < equippedSpells.Count && secondSpell < equippedSpells.Count) {
            playerMain.switchSpellSlots(firstSpell, secondSpell);
        }
    }

    public void setInventoryIcons(List<PlayerSpell> equippedSpells) {
        for (int i = 0; i < equippedSpells.Count; i++) {
            spellIcons[i].GetComponent<Image>().sprite = equippedSpells[i].getSpellScript().spellIcon;
        }
    }

    public void spellDragAndDropped(int index) {
        print("Dragging " + index);
        for (int i = 0; i < 7; i++) {
            if (spellSlots[i].GetComponent<SpellSlot>().mouseIsOverSlot()) {
                switchSpellSlots(index, i);
            } else {
                spellIcons[index].GetComponent<RectTransform>().localPosition = spellIcons[index].GetComponent<InventoryItem>().originalPos;
            }
        }
    }
}
