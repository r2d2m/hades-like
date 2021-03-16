using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour {

    public TextMeshProUGUI textDisplay;
    public TextAsset dialogueFile;
    public GameObject[] answerButtons;
    public Answer[] answers;

    public float typingSpeed = 0.05f;
    public float skipSpeed = 0.01f;

    public bool answering {
        get; 
        private set;
    }

    private Dictionary<int, TopicInfo> topics;
    private int nButtons = 3;

    private string[] sentences;
    private char[] currentSentence;
    private List<int> playedBefore;

    private float currentSpeed;
    private int index;
    private int letterIndex;
    private bool typing;
    private bool clickCooldown = false;
    

    void Awake() {
        InitializeComponents();
        LoadTopics();

        sentences = topics[0].sentences;
        answers = topics[0].answers;
        playedBefore.Add(0);
    }

    void OnEnable() {
        currentSpeed = typingSpeed;
        StartCoroutine(Type(sentences[index].ToCharArray()));
    }


    void Update() {
        
        if (!ValidInput()) {
            return;
        } 
            
        if (typing) {
            Skip();
        } else {
            currentSpeed = typingSpeed;
            NextSentence();
        }
        
    }

    bool ValidInput() {
        bool mouseInput = Input.GetKeyUp(KeyCode.Mouse0);
        bool spaceInput = Input.GetKeyDown(KeyCode.Space);
        return ((mouseInput || spaceInput) && !clickCooldown);
    }

    // Load topics from .json-file and put into a dictionary.

    void LoadTopics() {
        
        Conversation conversationFromFile = JsonUtility.FromJson<Conversation>(dialogueFile.text);

        foreach (TopicInfo info in conversationFromFile.conversation) {
            topics.Add(info.id,info);
        }
    }

    void InitializeComponents() {
        
        textDisplay = GameObject.FindGameObjectWithTag("DialogueDisplay").GetComponent<TextMeshProUGUI>();
        
        topics = new Dictionary<int, TopicInfo>();
        playedBefore = new List<int>();

        Array.Resize(ref answerButtons,nButtons);
        
        for (int i = 0; i < nButtons; i++) {
            answerButtons[i] = GameObject.FindGameObjectWithTag("DialogueDisplay").gameObject.transform.GetChild(i).gameObject;
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
        
        } else {

            answering = true;
            textDisplay.text = "";

            MakeButtons();

            currentSpeed = typingSpeed;
            typing = false;

        }
    }


    IEnumerator StartClickCooldown(float time) {
        clickCooldown = true;
        yield return new WaitForSeconds(time);
        clickCooldown = false;
    }

    void MakeButtons() {

        for (int i = 0; i < answers.Length; i++) {
            GameObject btn = answerButtons[i];
            Answer ans = answers[i];

            if (ans.text == "") {
                continue; 
            }

            int ptr = ans.pointer;
            
            btn.GetComponent<Button>().interactable = true;
            btn.GetComponent<Button>().onClick.RemoveAllListeners();
            btn.GetComponent<Button>().onClick.AddListener(delegate { NewTopic(ptr); });

            btn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = ans.text;

            Color textColor;
            textColor = (playedBefore.Contains(ptr) ? new Color(0.5f,0.5f,0.5f,1f) : new Color(1f,1f,1f,1f));
            btn.GetComponentsInChildren<TextMeshProUGUI>()[0].color = textColor;
        }         
        
        
    }

    public void NewTopic(int id) {


        foreach (GameObject btn in answerButtons) {
            btn.GetComponentsInChildren<TextMeshProUGUI>()[0].color = new Color(1f,1f,1f,0f);
            btn.GetComponent<Button>().interactable = false;
        }

        TopicInfo info = topics[id];

        answering = false;
        sentences = info.sentences;
        answers = info.answers;
        index = 0;
        letterIndex = 0;
        playedBefore.Add(id);

        StartCoroutine(StartClickCooldown(0.2f));
        StartCoroutine(Type(sentences[index].ToCharArray()));

    }

}

// Container class

[System.Serializable]
public class Conversation {
    
    public TopicInfo[] conversation;

}

// Information about topics.

[System.Serializable]
public class TopicInfo {
    
    public int id;
    public string[] sentences;
    public Answer[] answers;

}

// Pointer points to next topic.

[System.Serializable]
public class Answer {
    
    public int pointer;
    public string text;

}
