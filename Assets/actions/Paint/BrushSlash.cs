using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushSlash : GenericAction {
    
    GameObject paintHitboxPrefab = Resources.Load<GameObject>("collision_boxes/PaintHitbox");
    GameObject paintSplashPrefab = Resources.Load<GameObject>("effects/PaintSplash");
    GameObject brushSlashAnimPrefab = Resources.Load<GameObject>("effects/BrushSlashAnim");
    GameObject paintSidePrefab = Resources.Load<GameObject>("effects/PaintSide");
    
    GameObject hitbox;
    
    public BrushSlash() {
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
            
            animator.SetTrigger("startBrushSlash");
            
            airStall();
            
        }
        
        if(fstep == 16 / 4) {
            
            // Hitbox
            
            hitbox = GameObject.Instantiate(paintHitboxPrefab);
            
            hitbox.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            
            hitbox.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
                
                GameObject effect = GameObject.Instantiate(paintSplashPrefab);
                effect.transform.position = (hitbox.transform.position + collider.transform.position) / 2;
                effect.SetActive(true);
                
                PersistentStuff.fillAbilityGauge("Paint", 0.0625);
                
            });
            
            hitbox.transform.SetParent(user);
            
            hitbox.transform.localPosition = new Vector3(getUserFacingX() * 1, 0, 0);
            
            hitbox.SetActive(true);
            
            // Slash animation
            
            GameObject slashEffect = GameObject.Instantiate(brushSlashAnimPrefab);
            
            slashEffect.transform.SetParent(user);
            
            slashEffect.transform.localPosition = new Vector3(getUserFacingX() * 1, 0, 0);
            
            slashEffect.GetComponent<SpriteRenderer>().flipX = getUserFacingX() < 0;
            
            // Side splash
            
            GameObject effect2 = GameObject.Instantiate(paintSidePrefab);
            effect2.transform.position = user.position + new Vector3(getUserFacingX() * 1, 0, 0);
            effect2.GetComponent<SpriteRenderer>().flipX = getUserFacingX() < 0;
            effect2.SetActive(true);
            
        }
        
        if(fstep >= 64 / 4) {
            dispatchEnd();
        }
    }
    
}
