using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class PeriodicDecisionAI : MonoBehaviour {
    
    public int rate = 2048;
    public int counter = 0;
    
    public UnityEvent OnDecision = new UnityEvent();
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(counter >= rate) {
            OnDecision.Invoke();
            
            counter = 0;
        }
        
        ++counter;
    }
    
}
