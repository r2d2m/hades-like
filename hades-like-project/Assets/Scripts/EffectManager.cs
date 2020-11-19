using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{

    public GameObject blackScreen;

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

    public void setBackground(Color color){
        gameObject.GetComponent<Camera>().backgroundColor = color;
    }

    public void roomChangeEffect(float fadeSpeed, float afterFadeWait){
        IEnumerator fadeIn = fadeToBlack(fadeSpeed, afterFadeWait);
        StartCoroutine(fadeIn);
    }

    private IEnumerator fadeToBlack(float fadeSpeed, float afterFadeWait){
        IEnumerator fadeOut = fadeToNormal(fadeSpeed);
        while(blackScreen.GetComponent<SpriteRenderer>().color.a < 1){
            Color newColor = blackScreen.GetComponent<SpriteRenderer>().color + new Color(0, 0, 0,Time.deltaTime * fadeSpeed);
            blackScreen.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }
        yield return new WaitForSeconds(afterFadeWait);
            StartCoroutine(fadeOut);
    }

    private IEnumerator fadeToNormal(float fadeSpeed){
        print("fade to normal");
        while(blackScreen.GetComponent<SpriteRenderer>().color.a > 0){
            Color newColor = blackScreen.GetComponent<SpriteRenderer>().color - new Color(0, 0, 0,Time.deltaTime * fadeSpeed);
            blackScreen.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }
    }
}
