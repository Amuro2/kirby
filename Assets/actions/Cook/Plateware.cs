using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateware : GenericAction {
    
    public Plateware() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
        });
        
        OnEnd.AddListener(() => {
            freezeUserFacingX(false);
        });
    }
    
    public override void fixedUpdate() {
        if(fstep == 0) {
            
            animator.SetTrigger("startPlateware");
            
            // 
            
            Vector2 velocity = user.gameObject.GetComponent<Rigidbody2D>().velocity;
            if(velocity.y > 0) { velocity.y = 0; }
            user.gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
            
            // 
            
            float pow = 12;
            
            makePlate((new Vector2(getUserFacingX() * 0.26f, 0.97f)) * pow);
            makePlate((new Vector2(getUserFacingX() * 0.42f, 0.90f)) * pow);
            makePlate((new Vector2(getUserFacingX() * 0.57f, 0.82f)) * pow);
        }
        
        if(fstep == 64/4) {
            dispatchEnd();
        }
    }
    
    void makePlate(Vector2 velocity) {
        GameObject plate = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/Plate"));
        
        plate.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
        
        plate.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
            
            GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/StarExplosion"));
            effect.transform.position = (plate.transform.position + collider.transform.position) / 2;
            effect.SetActive(true);
            
            PersistentStuff.fillAbilityGauge("Cook", 0.0625);
            
        });
        
        plate.transform.SetParent(user.parent);
        
        plate.transform.position = user.position + new Vector3(getUserFacingX() * 0.5f, 0.5f, 0);
        
        plate.GetComponent<Rigidbody2D>().velocity = velocity;
        
        plate.SetActive(true);
    }
    
}
