using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyAction : GenericAction {
    
    public EmptyAction() {
        OnStart.AddListener(() => {
            
        });
        
        OnEnd.AddListener(() => {
            
        });
    }
    
    public override void update() {
        if(step == 1) {
            dispatchEnd();
        }
    }
    
}
