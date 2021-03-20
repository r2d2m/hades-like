using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCountUI : MonoBehaviour {
    public GameObject soulCountObject;
    // Start is called before the first frame update
    public void setSoulCount(int soulCount) {
        soulCountObject.GetComponent<TMPro.TextMeshProUGUI>().text = soulCount.ToString();
    }
}
