using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNameScript : MonoBehaviour {
    
    int step = 0;
    int initialTransitionDuration = 250;
    int persistDuration = 5000;
    int endTransitionDuration = 250;
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void FixedUpdate() {
        step += (int)(Time.deltaTime * 1000);
        
        if(step < initialTransitionDuration) {
            float progress = (float)step/initialTransitionDuration;
            
            setAlpha(progress);
        }
        
        else if(step < initialTransitionDuration + persistDuration) {
            setAlpha(1);
        }
        
        else {
            float progress = (float)(step - initialTransitionDuration - persistDuration)/(endTransitionDuration);
            
            if(progress > 1) { progress = 1; }
            
            setAlpha(1 - progress);
        }
        
    }
    
    void setAlpha(float alpha) {
        GetComponent<CanvasGroup>().alpha = alpha;
    }
    
}
