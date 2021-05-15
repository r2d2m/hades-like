using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public GameObject player;
    public GameObject blackScreen;

    bool currentlyShaking;
    bool followingPlayer;
    float shakeDuration;
    float shakeStrength;
    float cameraSens;
    float cameraDampening;
    Vector3 cameraDestination;
    Vector3 cameraPos;
    Vector3 mousePos;
    Vector3 playerPos;

    float maxXPositions, maxYPositions;

    // Start is called before the first frame update
    void Start() {
        shakeDuration = 0;
        shakeStrength = 0;
        followingPlayer = true;
        cameraDestination = transform.position;
        cameraPos = transform.position;
        cameraSens = 15.0f; // Higher = less impact from mousePos
        cameraDampening = 0.04f; // less = faster

        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);

        GetComponent<Camera>().transparencySortMode = TransparencySortMode.CustomAxis;
        GetComponent<Camera>().transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);
    }

    // Update is called once per frame
    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerPos = player.transform.position;

        debugCameraManager();
        if (followingPlayer == true) {
            cameraDestination = new Vector3(playerPos.x + (mousePos.x - playerPos.x) / cameraSens, playerPos.y + (mousePos.y - playerPos.y) / cameraSens, -10);
            // x camera center offset ~= 9, y camera center offset ~= 5
            cameraDestination = new Vector3(Mathf.Clamp(cameraDestination.x, -maxXPositions / 2 + GetComponent<Camera>().orthographicSize * 1.78f, maxXPositions / 2 - GetComponent<Camera>().orthographicSize * 1.78f), Mathf.Clamp(cameraDestination.y, -maxYPositions / 2 + GetComponent<Camera>().orthographicSize, maxYPositions / 2 - GetComponent<Camera>().orthographicSize), -10);
        } else {
            cameraDestination = new Vector3(0, 0, -1);
        }

        Vector3 velocity = Vector3.zero;
        cameraPos = Vector3.SmoothDamp(transform.position, cameraDestination, ref velocity, cameraDampening);
        // cameraPos = cameraDestination;


        if (shakeDuration > 0) {
            cameraPos = cameraPos + Random.insideUnitSphere * shakeStrength;
            shakeDuration -= Time.deltaTime;
        } else {
            shakeDuration = 0;
        }

        transform.position = cameraPos;
    }


    void debugCameraManager() {
        if (Input.GetKeyDown(KeyCode.O)) {
            followingPlayer = !followingPlayer;
            print("TOGGLE CAMERA! Following player: " + followingPlayer);
        }
    }

    public void setCameraPosition(float x, float y) {
        transform.position = new Vector3(x, y, -10);
        cameraDestination = transform.position;
    }

    public void setCameraBounds(float maxX, float maxY) {
        maxXPositions = maxX;
        maxYPositions = maxY;
    }

    public void screenShake(float duration, float strength) {
        shakeDuration = duration;
        shakeStrength = strength;
    }

    public void setBackground(Color color) {
        gameObject.GetComponent<Camera>().backgroundColor = color;
    }

    public void roomChangeEffect(float fadeSpeed, float afterFadeWait) {
        IEnumerator fadeIn = fadeToBlack(fadeSpeed, afterFadeWait);
        StartCoroutine(fadeIn);
    }

    private IEnumerator fadeToBlack(float fadeSpeed, float afterFadeWait) {
        IEnumerator fadeOut = fadeToNormal(fadeSpeed);
        while (blackScreen.GetComponent<SpriteRenderer>().color.a < 1) {
            Color newColor = blackScreen.GetComponent<SpriteRenderer>().color + new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
            blackScreen.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }
        yield return new WaitForSeconds(afterFadeWait);
        StartCoroutine(fadeOut);
    }

    private IEnumerator fadeToNormal(float fadeSpeed) {
        while (blackScreen.GetComponent<SpriteRenderer>().color.a > 0) {
            Color newColor = blackScreen.GetComponent<SpriteRenderer>().color - new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
            blackScreen.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }
    }
}
