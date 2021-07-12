using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doves : GenericAction {
    
    public Doves() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
        });
        
        OnEnd.AddListener(() => {
            freezeUserFacingX(false);
        });
    }
    
    public override void fixedUpdate() {
        if(fstep == 0/4) {
            animator.SetTrigger("startDoves");
        }
        
        if(fstep == 16/4) {
            
            airStall();
            
            float pow = 12;
            
            makeDove(new Vector2(getUserFacingX() * 0.5f, 0.866f) * pow);
            makeDove(new Vector2(getUserFacingX() * 0.90f, 0.43f) * pow);
            makeDove(new Vector2(getUserFacingX() * 1, 0) * pow);
            
        }
        
        if(fstep == 128/4) {
            dispatchEnd();
        }
    }
    
    void makeDove(Vector2 velocity) {
        GameObject dove = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/Dove"));
        
        dove.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
        
        dove.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
            
            GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/SmokeExplosion"));
            effect.transform.position = (dove.transform.position + collider.transform.position) / 2;
            effect.SetActive(true);
            
            PersistentStuff.fillAbilityGauge("Magic", 0.0625);
            
            GameObject.Destroy(dove);
            
        });
        
        dove.transform.SetParent(user.parent);
        
        dove.transform.position = user.position + new Vector3(getUserFacingX() * 0.25f, 0, 0);
        
        dove.GetComponent<Rigidbody2D>().velocity = velocity;
        
        dove.GetComponent<SpriteRenderer>().flipX = getUserFacingX() < 0;
        
        dove.SetActive(true);
    }
    
}
