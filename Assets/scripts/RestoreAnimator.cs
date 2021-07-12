using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreAnimator : MonoBehaviour {
    
    void Awake() {
        Animator animator = GetComponent<Animator>();
        
        if(animator != null) {
            animator.keepAnimatorControllerStateOnDisable = true;
        }
    }
    
}
