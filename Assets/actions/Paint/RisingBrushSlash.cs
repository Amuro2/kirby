using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingBrushSlash : GenericAction {
    
    GameObject hitbox;
    
    public RisingBrushSlash() {
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
        if(fstep == 0) {
            
            animator.SetTrigger("startRisingBrushSlash");
            
            airStall();
            
        }
        
        if(fstep == 16 / 4) {
            
            // Hitbox
            
            hitbox = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/PaintHitbox"));
            
            hitbox.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            
            hitbox.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
                
                GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/PaintSplash"));
                effect.transform.position = (hitbox.transform.position + collider.transform.position) / 2;
                effect.SetActive(true);
                
                PersistentStuff.fillAbilityGauge("Paint", 0.0625);
                
            });
            
            hitbox.transform.SetParent(user);
            
            hitbox.transform.localPosition = new Vector3(0, 1.5f, 0);
            
            hitbox.transform.localScale = new Vector3(4, 3, 2);
            
            hitbox.SetActive(true);
            
            // Slash animation
            
            GameObject slashEffect = GameObject.Instantiate(Resources.Load<GameObject>("effects/RisingBrushSlashAnim"));
            
            slashEffect.transform.SetParent(user);
            
            slashEffect.transform.localPosition = new Vector3(0, 1.5f, 0);
            
            // Up splash
            
            GameObject effect2 = GameObject.Instantiate(Resources.Load<GameObject>("effects/PaintUp"));
            effect2.transform.position = user.position + new Vector3(0, 1.5f, 0);
            effect2.GetComponent<SpriteRenderer>().flipY = getUserFacingX() < 0;
            effect2.SetActive(true);
            
        }
        
        if(fstep >= 96 / 4) {
            dispatchEnd();
        }
    }
    
}
