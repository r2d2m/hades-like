using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour {

    public float durationTime;
    public Color effectColor;
    public GameObject target;

    protected float elapsedTime = 0;

    protected void UpdateDuration() {
        elapsedTime += Time.deltaTime;
    }

}
