using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackInTheBox : GenericAction {
    
    public JackInTheBox() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
        });
        
        OnEnd.AddListener(() => {
            freezeUserFacingX(false);
        });
    }
    
    public override void fixedUpdate() {
        if(fstep == 0) {
            animator.SetTrigger("startJackInTheBox");
        }
        
        if(fstep == 16/4) {
            
            airStall();
            
            // 
            
            GameObject clownHead = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/ClownHead"));
            
            clownHead.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            
            clownHead.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
                
                GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/FireworkExplosion"));
                effect.transform.position = (clownHead.transform.position + collider.transform.position) / 2;
                effect.SetActive(true);
                
                PersistentStuff.fillAbilityGauge("Magic", 0.0625);
                
            });
            
            clownHead.transform.SetParent(user.parent);
            
            clownHead.transform.position = user.position + new Vector3(getUserFacingX() * 0.5f, 0.5f, 0);
            
            float pow = 20;
            clownHead.GetComponent<Rigidbody2D>().velocity = new Vector2(getUserFacingX() * 0.309f * pow, 0.95f * pow);
            
            clownHead.GetComponent<SpriteRenderer>().flipX = getUserFacingX() < 0;
            
            clownHead.SetActive(true);
            
        }
        
        if(fstep == 128/4) {
            dispatchEnd();
        }
    }
    
}
