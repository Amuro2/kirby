using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaphone : GenericAction {
    
    public Megaphone() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
            setUserStill(true);
            
            PersistentStuff.emptyAbilityGauge("Mike");
        });
        
        OnEnd.AddListener(() => {
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    public override void fixedUpdate() {
        if(fstep == 0) {
            animator.SetTrigger("startMegaphone");
        }
        
        if(fstep == 32/4) {
            
            airStall();
            
            // 
            
            GameObject hitbox = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/MegaphoneHitbox"));
            
            hitbox.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            
            hitbox.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
                
                GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/PaintSplash"));
                effect.transform.position = collider.transform.position;
                effect.SetActive(true);
                
            });
            
            hitbox.transform.SetParent(user);
            
            Vector3 position = user.position;
            
            position.x += getUserFacingX() * hitbox.transform.localScale.x/2;
            
            hitbox.transform.position = position;
            
            hitbox.SetActive(true);
            
        }
        
        if(fstep >= 32/4 && fstep < 224/4) {
            
            // 
            
            {
                GameObject waveLine = GameObject.Instantiate(Resources.Load<GameObject>("effects/WaveLine"));
                
                float minAngle = -Mathf.PI/16;
                float maxAngle = +Mathf.PI/16;
                float angle = minAngle + Random.value * (maxAngle - minAngle);
                
                if(getUserFacingX() < 0) {
                    angle += Mathf.PI;
                }
                
                Rigidbody2D rigidbody = waveLine.GetComponent<Rigidbody2D>();
                
                rigidbody.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 24;
                
                waveLine.transform.position = user.position + (Vector3)(rigidbody.velocity.normalized) * (waveLine.transform.localScale.x/2 + 1);
            }
            
            // 
            
            if(fstep % 4 == 0) {
                GameObject waveLine = GameObject.Instantiate(Resources.Load<GameObject>("effects/WaveLine"));
                
                float minAngle = -Mathf.PI/4;
                float maxAngle = +Mathf.PI/4;
                float angle = minAngle + Random.value * (maxAngle - minAngle);
                
                if(getUserFacingX() < 0) {
                    angle += Mathf.PI;
                }
                
                Rigidbody2D rigidbody = waveLine.GetComponent<Rigidbody2D>();
                
                rigidbody.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 24;
                
                waveLine.transform.position = user.position + (Vector3)(rigidbody.velocity.normalized) * (waveLine.transform.localScale.x/2 + 1);
            }
            
            // 
            
            if(fstep % 6 == 0 || (fstep + 2) % 6 == 0) {
                for(int i = 0; i < 2; ++i) {
                    GameObject waveEllipse = GameObject.Instantiate(Resources.Load<GameObject>("effects/WaveEllipse"));
                    
                    waveEllipse.transform.position = user.position + new Vector3(getUserFacingX() * 2, -0.125f + Random.value * (0.25f), 0);
                    
                    waveEllipse.GetComponent<Rigidbody2D>().velocity = new Vector2(getUserFacingX() * 32, 0);
                }
            }
            
        }
        
        if(fstep == 224/4) {
            dispatchEnd();
        }
    }
    
}
