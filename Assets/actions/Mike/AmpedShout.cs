using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpedShout : GenericAction {
    
    public AmpedShout() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
        });
        
        OnEnd.AddListener(() => {
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    public override void update() {
        setUserStill(isGrounded());
    }
    
    public override void fixedUpdate() {
        if(fstep == 16 / 4) {
            animator.SetTrigger("startAmpedShout");
            
            GameObject projectile = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/AmpedWave"));
            
            projectile.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            
            projectile.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
                
                GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/StarExplosion"));
                effect.transform.position = (projectile.transform.position + collider.transform.position) / 2;
                effect.SetActive(true);
                
                PersistentStuff.fillAbilityGauge("Mike", 0.125);
                
            });
            
            projectile.transform.SetParent(user.parent);
            
            projectile.transform.position = user.position + new Vector3(getUserFacingX() * 0.5f, 0.5f, 0);
            
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(getUserFacingX() * 24, 16);
            
            projectile.SetActive(true);
        }
        
        if(fstep == 96 / 4) {
            dispatchEnd();
        }
    }
    
}
