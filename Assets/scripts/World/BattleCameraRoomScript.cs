using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraRoomScript : MonoBehaviour {
    
    public Vector3 scale2;
    Vector3 scale1;
    
    float step = 999999999;
    float duration = 500;
    bool battleStarted;
    
    void Reset() {
        scale2 = transform.localScale;
    }
    
    void Awake() {
        scale1 = transform.localScale;
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(Flags.isFlagUp("inBattle")) {
            if(!battleStarted) {
                battleStarted = true;
                step = 0;
            }
            
            float progress = step / duration;
            
            if(progress > 1) {
                progress = 1;
            }
            
            transform.localScale = scale1 + (scale2 - scale1) * Mathf.Pow(progress, 1f/2);
            // transform.localScale = transform.localScale + (scale2 - transform.localScale) * progress;
            
        }
        
        else {
            if(battleStarted) {
                battleStarted = false;
                step = 0;
            }
            
            float progress = step / duration;
            
            if(progress > 1) {
                progress = 1;
            }
            
            transform.localScale = scale2 + (scale1 - scale2) * Mathf.Pow(progress, 1f/2);
            // transform.localScale = transform.localScale + (scale1 - transform.localScale) * progress;
            
        }
        
        step += Time.deltaTime * 1000;
        
        if(step > duration) {
            step = duration;
        }
    }
}
