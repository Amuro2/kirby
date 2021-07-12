using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : GenericAction {
    
    GameObject drawingHitbox;
    
    public Painter() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
            setUserStill(true);
            
            animator.SetTrigger("startPainter");
            
        });
        
        OnEnd.AddListener(() => {
            GameObject.Destroy(drawingHitbox);
            
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    public override void fixedUpdate() {
        if(fstep == 0) {
            
            // Drawing Animation
            
            GameObject anim = GameObject.Instantiate(Resources.Load<GameObject>("effects/PainterAnim"));
            
            anim.transform.SetParent(user);
            anim.transform.localPosition = new Vector3(getUserFacingX() * 1, 0, 0);
            
            anim.SetActive(true);
            
            // Drawing Hitbox
            
            drawingHitbox = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/PainterDrawing"));
            
            drawingHitbox.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            drawingHitbox.GetComponent<Hitbox>().tagToHit = "Enemy";
            
            drawingHitbox.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
                
            });
            
            drawingHitbox.transform.SetParent(user);
            
            drawingHitbox.transform.localPosition = new Vector3(getUserFacingX() * 1, 0, 0);
            
            drawingHitbox.SetActive(true);
        }
        
        else if(fstep >= 192 / 4) {
            
            // animator.SetTrigger("startPainter2");
            
            // Painted character
            
            GameObject.Destroy(drawingHitbox);
            
            GameObject painted = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/PaintedCharacter"));
            
            painted.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            painted.GetComponent<Hitbox>().tagToHit = "Enemy";
            
            painted.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
                
                GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/PaintSplash"));
                effect.transform.position = (painted.transform.position + collider.transform.position) / 2;
                
                effect.SetActive(true);
                
            });
            
            painted.GetComponent<StraightLineAI>().headingPlayer = false;
            painted.GetComponent<StraightLineAI>().direction = getUserFacingX();
            painted.GetComponent<StraightLineAI>().speed = 128;
            
            painted.transform.SetParent(user.parent);
            painted.transform.position = user.position + new Vector3(getUserFacingX() * 1, 0, 0);
            
            // Vector3 position = user.transform.position;
            // position.x += (float)(getUserFacingX() * 1);
            // painted.transform.position = position;
            
            painted.GetComponent<SpriteRenderer>().flipX = getUserFacingX() < 0;
            
            painted.SetActive(true);
            
            PersistentStuff.fillAbilityGauge("Paint", 0.125);
            
            // 
            
            dispatchEnd();
        }
        
        if(fstep == 256 / 4) {
            
        }
    }
    
}
