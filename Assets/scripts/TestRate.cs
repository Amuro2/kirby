using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;

public class TestRate : MonoBehaviour {
    
    int updateCounter = 0;
    int fixedUpdateCounter = 0;
    DateTime updateDateTime;
    DateTime fixedUpdateDateTime;
    
    Text updateText;
    Text fixedUpdateText;
    
    void Awake() {
        updateText = transform.Find("Text (Update)").gameObject.GetComponent<Text>();
        fixedUpdateText = transform.Find("Text (FixedUpdate)").gameObject.GetComponent<Text>();
    }
    
    void Update() {
        
        updateText.text = "UPDATE: " + (DateTime.Now.Subtract(updateDateTime).TotalMilliseconds) + " -- " + Time.deltaTime;
        
        ////  ////
        
        ++updateCounter;
        updateDateTime = DateTime.Now;
        
    }
    
    void FixedUpdate() {
        
        fixedUpdateText.text = "FIXED UPDATE: " + (DateTime.Now.Subtract(fixedUpdateDateTime).TotalMilliseconds) + " -- " + Time.deltaTime;
        
        ////  ////
        
        ++fixedUpdateCounter;
        fixedUpdateDateTime = DateTime.Now;
        
    }
    
}
