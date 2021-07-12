using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleWallScript : MonoBehaviour {
    
    public Vector3 position2;
    public Vector3 scale2;
    Vector3 position1;
    Vector3 scale1;
    
    float step = 999999999;
    float duration = 1000;
    bool battleStarted;
    
    void Reset() {
        position2 = transform.position;
        scale2 = transform.localScale;
    }
    
    void Awake() {
        position1 = transform.position;
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
            
            transform.position = position1 + (position2 - position1) * progress;
            transform.localScale = scale1 + (scale2 - scale1) * progress;
            
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
            
            transform.position = position2 + (position1 - position2) * progress;
            transform.localScale = scale2 + (scale1 - scale2) * progress;
            
        }
        
        step += Time.deltaTime * 1000;
    }
    
}
