using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : GenericAction {
    
    static GameObject hitboxPrefab;
    GameObject hitbox;
    
    public Fall() {
        if(hitboxPrefab == null) {
            hitboxPrefab = Resources.Load<GameObject>("collision_boxes/Hitbox");
        }
        
        OnStart.AddListener(() => {
            
        });
        
        OnEnd.AddListener(() => {
            GameObject.Destroy(hitbox);
        });
    }
    
    public override void fixedUpdate() {
        if(fstep < 16) {
            animator.SetBool("usingAction", false);
        }
        
        else {
            if(hitbox == null) {
                hitbox = GameObject.Instantiate(hitboxPrefab);
                
                hitbox.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
                hitbox.GetComponent<Hitbox>().damage = 1;
                
                hitbox.transform.SetParent(user);
                hitbox.transform.localPosition = new Vector3(0, -0.5f + 0.375f/2, 0);
                hitbox.transform.localScale = new Vector3(0.875f, 0.375f, 0.5f);
                
                hitbox.SetActive(true);
            }
        }
    }
    
    public override bool isCancelableWith(GenericAction action) {
        return !(action is Fall);
    }
    
    public override string ToString() {
        if(fstep >= 16) {
            return "DamageFall";
        }
        
        return "Fall";
    }
    
}
