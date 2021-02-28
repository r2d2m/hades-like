using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private char[] currentSentence;
    public float typingSpeed = 0.05f;
    public float skipSpeed = 0.01f;
    private float currentSpeed;
    private int index;
    private int letterIndex;
    private bool typing;

    void OnEnable() {
        currentSpeed = typingSpeed;
        StartCoroutine(Type(sentences[index].ToCharArray()));
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (typing) {
                Skip();
            } else {
                currentSpeed = typingSpeed;
                NextSentence();
            }
            
        }
    }

    IEnumerator Type(char[] sentence) {
        typing = true;

        for (int i = letterIndex; i < sentence.Length; i++) {
            textDisplay.text += sentence[i];
            letterIndex++;
            yield return new WaitForSeconds(currentSpeed);    
        }

        typing = false;
    }

    void Skip() {
        currentSpeed = skipSpeed;
    }

    void NextSentence() {

        if (index < sentences.Length - 1) {
            index++;
            letterIndex = 0;
            textDisplay.text = "";
            char[] sentence = sentences[index].ToCharArray();
            StartCoroutine(Type(sentence));
        } 
    }
}
