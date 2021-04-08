using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour {

    public GameObject UI;
    public GameObject background;
    public TextMeshProUGUI textDisplay;

    public float fadeTime = 0.2f;
    public float slideDistance = 0.5f;

    private float padding = 10f;
    private float margin = 1;

    private float windowX;
    private float windowY;

    private bool fadingIn = false;
    private bool fadingOut = false;


    void Update() {
        UI.transform.position = new Vector2(windowX, windowY);
    }

    public void OpenWindow(float x, float y, string infoText) {

        windowX = x;
        windowY = y + margin - slideDistance;

        UI.transform.position = new Vector2(x, y + margin - slideDistance);
        textDisplay.text = infoText;

        Vector2 backgroundSize = new Vector2(textDisplay.GetComponent<RectTransform>().rect.width + padding, textDisplay.preferredHeight + padding);
        background.GetComponent<RectTransform>().sizeDelta = backgroundSize;

        fadingOut = false;
        fadingIn = true;
        StartCoroutine(FadeIn());

    }

    public void CloseWindow() {
        fadingOut = true;
        fadingIn = false;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn() {

        Color deltaColor = new Color(0, 0, 0, 1 / fadeTime);

        while (textDisplay.color.a < 1 && !fadingOut) {

            textDisplay.color += Time.deltaTime * deltaColor;
            background.GetComponent<Image>().color += Time.deltaTime * deltaColor;

            windowY += slideDistance * Time.deltaTime / fadeTime;

            yield return null;

        }

        fadingIn = false;
    }

    IEnumerator FadeOut() {

        Color deltaColor = new Color(0, 0, 0, 1 / fadeTime);

        while (textDisplay.color.a > 0 && !fadingIn) {

            textDisplay.color -= Time.deltaTime * deltaColor;
            background.GetComponent<Image>().color -= Time.deltaTime * deltaColor;

            windowY -= slideDistance * Time.deltaTime / fadeTime;

            yield return null;

        }

        fadingOut = false;

    }

}
