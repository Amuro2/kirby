using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Lifespan : MonoBehaviour {
    
    public float msLifespan = 1000;
    
    public UnityEvent OnEnd = new UnityEvent();
    
    float step = 0;
    
    void FixedUpdate() {
        step += Time.deltaTime * 1000;
        
        if(step >= msLifespan) {
            OnEnd.Invoke();
            
            Destroy(gameObject);
        }
    }
    
}
