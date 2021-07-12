using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : GenericAction {
    
    public Hover() {
        OnStart.AddListener(() => {
            // Debug.Log("Float");
            
            getUserScript().floating = true;
            animator.SetTrigger("startHovering");
            animator.SetBool("floating", true);
            setUserForceY(5);
        });
        
        OnEnd.AddListener(() => {
            
        });
    }
    
    public override void fixedUpdate() {
        if(fstep == 64 / 4) {
            dispatchEnd();
        }
    }
    
    public override bool isCancelableWith(GenericAction action) {
        return !(action is Hover || action is Fall);
    }
    
}
