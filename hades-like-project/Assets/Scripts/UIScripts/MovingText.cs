using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;



public class MovingText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private bool mouse_over = false;
    public TextMeshProUGUI textDisplay;

    public float standardDilate = 0.24f;
    public float dilateAmplitude = 0.15f;
    public float period = 2f;
   
    private float t = 0f;

    void Awake() {
        textDisplay = this.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    void Update() {
        if (mouse_over) {
            
            t += Time.deltaTime;
            textDisplay.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, standardDilate + dilateAmplitude + (float)(dilateAmplitude * Math.Sin(period*t - Math.PI/2)));
        }
        
        
    }
 
    public void OnPointerEnter(PointerEventData eventData) {
         mouse_over = true;
         t = 0f;
    }
 
     public void OnPointerExit(PointerEventData eventData) {
         mouse_over = false;
         textDisplay.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, standardDilate);
     }
   
}
