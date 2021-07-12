using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using UnityEngine.UI;
using UnityEngine.EventSystems;

using UnityEngine.Events;

public class TitleScript : MonoBehaviour {
    
    void Awake() {
        
    }
    
    // Start is called before the first frame update
    void Start() {
        Transform firstButton = transform.Find("Button");
        
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
    public void startEmpty() {
        Controls.abilityMap["A"] = "Jump";
        Controls.abilityMap["B"] = "Inhale";
        Controls.abilityMap["X"] = "Jump";
        Controls.abilityMap["Y"] = "Inhale";
        PersistentStuff.setAllAbilities(false);
        PersistentStuff.transitionToRoom("Greenhouse1", new Vector3(0, 8, 0), false);
    }
    
}
