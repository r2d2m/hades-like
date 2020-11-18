using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{

    float shakeDuration;
    float shakeStrength;
    Vector3 initPos;
    // Start is called before the first frame update
    void Start()
    {
        shakeDuration = 0;
        shakeStrength = 0;
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) screenShake(0.1f,0.5f);
        if(shakeDuration > 0){
            transform.localPosition = initPos + Random.insideUnitSphere * shakeStrength;
            shakeDuration -= Time.deltaTime;
        }else{
            shakeDuration = 0;
            transform.localPosition = initPos; 
        }
    }

    public void screenShake(float duration, float strength){
        shakeDuration = duration;
        shakeStrength = strength;
    }
}
