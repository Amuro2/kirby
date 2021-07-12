using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpLadle : GenericAction {
    
    GameObject hitbox;
    
    public UpLadle() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
        });
        
        OnEnd.AddListener(() => {
            GameObject.Destroy(hitbox);
            
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    public override void update() {
        setUserStill(isGrounded());
    }
    
    public override void fixedUpdate() {
        if(fstep == 0/4) {
            
            animator.SetTrigger("startUpLadle");
            
            // 
            
            hitbox = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/LadleHitbox"));
            
            hitbox.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            
            hitbox.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
                
                GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/StarExplosion"));
                effect.transform.position = (hitbox.transform.position + collider.transform.position) / 2;
                effect.SetActive(true);
                
                PersistentStuff.fillAbilityGauge("Cook", 0.0625);
                
            });
            
            hitbox.transform.SetParent(user);
            
            hitbox.transform.localPosition = new Vector3(0, 1, 0);
            hitbox.transform.localScale = new Vector3(0.375f, 1, 1);
            
            hitbox.SetActive(true);
            
        }
        
        
        float progression = (float)fstep/(64/4);
        float firstY = 1;
        float lastY = 5;
        float y = firstY + Mathf.Sin(progression * Mathf.PI) * (lastY - firstY);
        
        hitbox.transform.localPosition = new Vector3(0, y, 0);
        
        if(fstep == 64/4) {
            dispatchEnd();
        }
    }
    
}
