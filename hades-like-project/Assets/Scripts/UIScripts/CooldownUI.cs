using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CooldownUI : MonoBehaviour {
    public GameObject cooldownIconPrefab;
    public GameObject cooldownContainer;
    List<float> cooldownDurations;
    float spacing = 80f;
    public int currentSpellCount = 0;
    public Sprite[] branchSprites;
    public GameObject branchGameObject;


    List<Transform> spellSlots;
    List<GameObject> cooldownIcons;


    public GameObject manaBar;
    public GameObject manaText;
    // Start is called before the first frame update

    public void setSpellCount(int count) {
        if (cooldownIcons != null) {
            foreach (GameObject icon in cooldownIcons) {
                Destroy(icon);
            }
        }
        cooldownIcons = new List<GameObject>();
        cooldownDurations = new List<float>();
        spellSlots = new List<Transform>();
        foreach (Transform spellSlot in cooldownContainer.transform) {
            print(spellSlot);
            spellSlots.Add(spellSlot);
        }

        currentSpellCount = count;

        for (int i = 0; i < currentSpellCount; i++) {
            GameObject cooldownIcon = Instantiate(cooldownIconPrefab, spellSlots[i].localPosition + new Vector3(50, 0, 0), cooldownContainer.transform.rotation);
            //cooldownIcon.GetComponent<RectTransform>().localScale = new Vector3(30, 30, 1);
            cooldownIcon.transform.SetParent(cooldownContainer.transform, false);
            cooldownIcons.Add(cooldownIcon);
            cooldownDurations.Add(1);
        }
    }

    public void addSpell() {
        GameObject cooldownIcon = Instantiate(cooldownIconPrefab, spellSlots[currentSpellCount].localPosition + new Vector3(50, 0, 0), cooldownContainer.transform.rotation);
        //cooldownIcon.GetComponent<RectTransform>().localScale = new Vector3(30, 30, 1);
        cooldownIcon.transform.SetParent(cooldownContainer.transform, false);
        cooldownIcons.Add(cooldownIcon);
        cooldownDurations.Add(1);
        currentSpellCount++;

        updateBranchSprite();
    }

    public void updateBranchSprite() {
        int branchSpriteIndex = 0;
        if (currentSpellCount > 5) {
            branchSpriteIndex = 2;
        } else if (currentSpellCount > 3) {
            branchSpriteIndex = 1;
        } else {
            branchSpriteIndex = 0;
        }
        branchGameObject.GetComponent<SpriteRenderer>().sprite = branchSprites[branchSpriteIndex];
    }

    public void setCooldownDuration(int spellIndex, float cooldownDuration) {
        cooldownDurations[spellIndex] = cooldownDuration;
    }

    public void updateCooldowns(List<float> spellCooldowns) {
        for (int i = 0; i < currentSpellCount; i++) {
            cooldownIcons[i].GetComponentInChildren<Image>().fillAmount = spellCooldowns[i] / cooldownDurations[i];
        }
    }

    public void setSpellIcons(List<GameObject> equippedSpells) {
        for (int i = 0; i < equippedSpells.Count; i++) {
            cooldownIcons[i].GetComponent<SpriteRenderer>().sprite = equippedSpells[i].GetComponent<Spell>().spellIcon;
        }
    }

    public void UpdateManaBar(float currentAmount, float currentPercent, List<float> manaCosts) {
        manaBar.GetComponent<Image>().fillAmount = currentPercent;
        manaText.GetComponent<TextMeshProUGUI>().text = ((int)currentAmount).ToString();
        for (int i = 0; i < manaCosts.Count; i++) {
            if (currentAmount < manaCosts[i]) {
                cooldownIcons[i].GetComponent<SpriteRenderer>().color = Color.red;
            } else {
                cooldownIcons[i].GetComponent<SpriteRenderer>().color = Color.white;

            }
        }
    }
}
