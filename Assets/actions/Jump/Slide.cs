using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : GenericAction {
    
    double power = 16;
    Transform hitbox;
    
    Vector2 saveOffset;
    Vector2 saveSize;
    
    public Slide() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
            setUserStill(true);
            
            animator.SetTrigger("startSliding");
            
            // Box Collider size shift
            
            BoxCollider2D collider = user.GetComponent<BoxCollider2D>();
            saveOffset = collider.offset;
            saveSize = collider.size;
            
            float y2 = collider.offset.y - collider.size.y/2;
            
            Vector2 size = collider.size;
            size.y /= 4;
            collider.size = size;
            
            Vector2 offset = collider.offset;
            offset.y = y2 + size.y/2;
            collider.offset = offset;
            
            // 
            
            hitbox = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/Hitbox")).transform;
            
            hitbox.SetParent(user);
            
            Vector3 position = user.position;
            
            position.x += getUserFacingX() * user.localScale.x/2;
            
            hitbox.position = position;
            
            hitbox.GetComponent<Hitbox>().damage = 1;
            hitbox.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
        });
        
        OnEnd.AddListener(() => {
            
            // Box Collider size shift
            
            BoxCollider2D collider = user.GetComponent<BoxCollider2D>();
            collider.offset = saveOffset;
            collider.size = saveSize;
            
            // 
            
            GameObject.Destroy(hitbox.gameObject);
            
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    public override void update() {
        setUserSpeedX(getUserFacingX() * (float)power);
        
        // power /= 1 + 0.015625;
        
        // if(step == 96) {
            // power = 0;
        // }
        
        // if(step == 144) {
            // dispatchEnd();
        // }
        
    }
    
    public override void fixedUpdate() {
        
        power /= 1.0625;
        
        if(fstep == 96/4) {
            power = 0;
        }
        
        if(fstep == 136/4) {
            dispatchEnd();
        }
        
    }
    
}
