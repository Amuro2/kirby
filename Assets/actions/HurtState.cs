using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : GenericAction {
    
    int duration;
    
    public HurtState(int duration) {
        this.duration = duration;
        
        if(duration <= 0) {
            dispatchEnd();
        } else {
            
            OnStart.AddListener(() => {
                freezeUserFacingX(true);
                setUserStill(true);
                animator.SetBool("hurt", true);
            });
            
            OnEnd.AddListener(() => {
                animator.SetBool("hurt", false);
                setUserStill(false);
                freezeUserFacingX(false);
            });
            
        }
    }
    
    public override void fixedUpdate() {
        if(fstep == duration) {
            dispatchEnd();
        }
    }
    
}
