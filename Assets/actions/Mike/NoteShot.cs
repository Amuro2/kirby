using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteShot : GenericAction {
    
    public double angle;
    int endTimeout = -1;
    
    public NoteShot() {
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
        if(fstep % (80/4) == 0) {
            
            bool firstNote = fstep == 0;
            
            GameObject projectile;
            
            if(firstNote) {
                projectile = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/MusicalNoteStrong"));
                
                animator.SetTrigger("startNoteShot");
                
            } else {
                projectile = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/MusicalNote"));
            }
            
            projectile.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            
            projectile.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
                
                GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/StarExplosion"));
                effect.transform.position = (projectile.transform.position + collider.transform.position) / 2;
                effect.SetActive(true);
                
                if(firstNote) {
                    PersistentStuff.fillAbilityGauge("Mike", 0.125);
                } else {
                    PersistentStuff.fillAbilityGauge("Mike", 0.0625);
                }
                
            });
            
            projectile.transform.SetParent(user.parent);
            
            projectile.transform.position = user.position + new Vector3(getUserFacingX() * 0.5f, 0, 0);
            
            Vector2 velocity = new Vector2(getUserFacingX() * 8, 0);
            
            float trueAngle = (float)(getUserFacingX() * angle * Mathf.PI/6);
            
            float cos = Mathf.Cos(trueAngle);
            float sin = Mathf.Sin(trueAngle);
            
            float x = cos * velocity.x - sin * velocity.y;
            float y = sin * velocity.x + cos * velocity.y;
            
            velocity.x = x;
            velocity.y = y;
            
            projectile.GetComponent<Rigidbody2D>().velocity = velocity;
            
            projectile.SetActive(true);
            
        }
        
        if(endTimeout > 0) {
            --endTimeout;
            
            if(endTimeout == 0) {
                dispatchEnd();
            }
        }
    }
    
    public void requestEnd() {
        if(endTimeout < 0) {
            endTimeout = 64 / 4;
        }
    }
    
    public void cancelEndRequest() {
        endTimeout = -1;
    }
    
}
