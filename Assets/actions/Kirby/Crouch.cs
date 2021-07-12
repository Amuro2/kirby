using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : GenericAction {
    
    // Vector3 saveLocalScale;
    Vector2 saveOffset;
    Vector2 saveSize;
    
    public Crouch() {
        OnStart.AddListener(() => {
            animator.SetTrigger("startCrouching");
            setUserStill(true);
            
            
            
            BoxCollider2D collider = user.gameObject.GetComponent<BoxCollider2D>();
            saveOffset = collider.offset;
            saveSize = collider.size;
            
            float y2 = collider.offset.y - collider.size.y/2;
            
            Vector2 size = collider.size;
            size.y /= 4;
            collider.size = size;
            
            Vector2 offset = collider.offset;
            offset.y = y2 + size.y/2;
            collider.offset = offset;
            
        });
        
        OnEnd.AddListener(() => {
            
            BoxCollider2D collider = user.gameObject.GetComponent<BoxCollider2D>();
            collider.offset = saveOffset;
            collider.size = saveSize;
            
            
            
            setUserStill(false);
        });
    }
    
    public override bool isCancelableWith(GenericAction action) {
        return !(action is Crouch);
    }
    
    public override void update() {
        
    }
    
}
