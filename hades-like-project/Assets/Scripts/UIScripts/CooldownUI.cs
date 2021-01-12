using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour {
    public GameObject cooldownIconPrefab;
    public GameObject cooldownContainer;
    List<float> cooldownDurations;
    float spacing = 30f;
    int currentSpellCount = 0;

    List<GameObject> cooldownIcons;
    // Start is called before the first frame update

    private void Start() {
        cooldownIcons = new List<GameObject>();
        cooldownDurations = new List<float>();
        setSpellCount(4);
    }

    public void setSpellCount(int count) {
        cooldownIcons = new List<GameObject>();
        cooldownDurations = new List<float>();
        currentSpellCount = count;
        for (int i = 0; i < currentSpellCount; i++) {
            GameObject cooldownIcon = Instantiate(cooldownIconPrefab, new Vector3(spacing * i + spacing / 2, 0, 0), cooldownContainer.transform.rotation);
            cooldownIcon.GetComponent<RectTransform>().localScale = new Vector3(30, 30, 1);
            cooldownIcon.transform.SetParent(cooldownContainer.transform, false);
            cooldownIcons.Add(cooldownIcon);
            cooldownDurations.Add(1);
        }
    }

    public void setCooldownDuration(int spellIndex, float cooldownDuration) {
        cooldownDurations[spellIndex] = cooldownDuration;
    }

    public void updateCooldowns(List<float> spellCooldowns) {
            print(spellCooldowns.Count + " / " + currentSpellCount + ", " + cooldownDurations.Count);

        for (int i = 0; i < currentSpellCount; i++) {
            print(spellCooldowns[i] + " / " + cooldownDurations[i]);
            cooldownIcons[i].GetComponentInChildren<Image>().fillAmount = spellCooldowns[i] / cooldownDurations[i];
        }
    }
}
