using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSpin : GenericAction {
    
    GameObject hitbox;
    GameObject anim;
    
    public PaintSpin() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
            setUserStill(true);
            
            animator.SetTrigger("startPaintSpin");
            
            user.GetComponent<Damageable>().setInvincible(true);
            
        });
        
        OnEnd.AddListener(() => {
            GameObject.Destroy(hitbox);
            GameObject.Destroy(anim);
            
            user.GetComponent<Damageable>().setInvincible(false);
            
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    public override void update() {
        setUserSpeedX(getUserFacingX() * 12);
        
        airStall();
    }
    
    public override void fixedUpdate() {
        if(fstep == 0) {
            
            // Hitbox
            
            hitbox = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/PaintHitbox"));
            
            hitbox.GetComponent<Hitbox>().rehitRate = 32;
            
            hitbox.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            
            hitbox.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
                
                GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/PaintSplash"));
                effect.transform.position = (hitbox.transform.position + collider.transform.position) / 2;
                effect.SetActive(true);
                
                PersistentStuff.fillAbilityGauge("Paint", 0.0625/4);
                
            });
            
            hitbox.transform.SetParent(user);
            
            hitbox.transform.localPosition = new Vector3(0, 0, 0);
            hitbox.transform.localScale = new Vector3(4, 2, 2);
            
            hitbox.SetActive(true);
            
            // Paint animation
            
            anim = GameObject.Instantiate(Resources.Load<GameObject>("effects/PaintSpinAnim"));
            
            anim.transform.SetParent(user);
            
            anim.transform.localPosition = new Vector3(0, 0, 0);
            
        }
        
        if(fstep >= 128 / 4) {
            dispatchEnd();
        }
    }
    
}
