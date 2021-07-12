using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirGun : GenericAction {
    
    public AirGun() {
        OnStart.AddListener(() => {
            getUserScript().floating = false;
            animator.SetBool("floating", false);
            animator.SetTrigger("startSpitting");
            
        });
        
        OnEnd.AddListener(() => {
            
        });
    }
    
    public override void fixedUpdate() {
        
        if(fstep == 0) {
            
            GameObject projectile = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/AirBullet"));
            
            Vector3 position = user.position;
            
            position.x += (float)getUserFacingX() * (user.localScale.x + projectile.transform.localScale.x) / 2;
            
            projectile.transform.position = position;
            
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2((float)getUserFacingX() * 16, 0);
            
            projectile.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            
            projectile.transform.SetParent(user.parent);
            
        }
        
        if(fstep < 16 / 4) {
            
            Vector2 velocity = user.gameObject.GetComponent<Rigidbody2D>().velocity;
            velocity.y = 0;
            user.gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
            
        }
        
        if(fstep == 24 / 4) {
            dispatchEnd();
        }
    }
    
}
