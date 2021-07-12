using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladle : GenericAction {
    
    GameObject hitbox;
    LadleScript ladleScript;
    GameObject ladleObject;
    
    public Ladle() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
        });
        
        OnEnd.AddListener(() => {
            GameObject.Destroy(hitbox);
            // GameObject.Destroy(ladleScript.gameObject);
            GameObject.Destroy(ladleObject);
            
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    public override void update() {
        setUserStill(isGrounded());
    }
    
    public override void fixedUpdate() {
        if(fstep == 0) {
            
            animator.SetTrigger("startLadle");
            
        }
        
        if(fstep == 4) {
            
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
            
            hitbox.transform.localPosition = new Vector3(getUserFacingX() * 1, 0, 0);
            
            hitbox.SetActive(true);
            
            // 
            
            ladleObject = GameObject.Instantiate(Resources.Load<GameObject>("effects/Ladle"));
            ladleObject.transform.SetParent(user.parent);
            // ladleObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            ladleObject.GetComponent<SpriteRenderer>().flipX = getUserFacingX() < 0;
            // ladleScript = ladleObject.GetComponent<LadleScript>();
            ladleObject.SetActive(true);
            
        }
        
        if(hitbox != null) {
            
            float progression = (float)(fstep - 4)/(64/4 - 4);
            float firstX = getUserFacingX() * 1;
            float lastX = getUserFacingX() * 7;
            float x = firstX + Mathf.Sin(progression * Mathf.PI) * (lastX - firstX);
            
            hitbox.transform.localPosition = new Vector3(x, 0, 0);
            
            // 
            
            // ladleScript.setRatio(Mathf.Sin(progression * Mathf.PI));
            
            // float ladleMinWidth = ladleScript.transform.localScale.x * ladleScript.minWidth;
            // float ladleMaxWidth = ladleScript.transform.localScale.x * ladleScript.maxWidth;
            // ladleMaxWidth = ladleScript.GetComponent<SpriteRenderer>().bounds.size.x;
            // ladleMaxWidth = ladleScript.maxWidth;
            // ladleMaxWidth = 10;
            // float minScaleX = ladleScript.transform.localScale.x;
            // float maxScaleX = ladleScript.transform.localScale.x * ladleScript.GetComponent<SpriteRenderer>().size.x / 1f;
            // float minDistance = user.localScale.x/2;
            // minDistance = user.localScale.x/2 + ladleMinWidth;
            // minDistance = user.localScale.x/2 + minScaleX;
            // float maxDistance = minDistance + ladleMaxWidth/2;
            // maxDistance = user.localScale.x/2 + ladleMaxWidth;
            // maxDistance = user.localScale.x/2 + maxScaleX;
            
            // Debug.Log(minDistance + " -- " + maxDistance);
            // Debug.Log(minScaleX + " -- " + maxScaleX);
            
            // Vector3 ladlePosition = user.position;
            // ladlePosition.x += getUserFacingX() * (minDistance + Mathf.Sin(progression * Mathf.PI) * (maxDistance - minDistance));
            // ladleScript.transform.position = ladlePosition;
            
            ladleObject.transform.position = user.position;
            
        }
        
        if(fstep == 64/4) {
            dispatchEnd();
        }
    }
    
}
