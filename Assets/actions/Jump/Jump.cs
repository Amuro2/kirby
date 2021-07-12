using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : GenericAction {
    
    float first = 6.0f;
    float last = 0;
    float exponent = 1f/44;
    int duration = 80;
    
    static GameObject hitboxPrefab;
    GameObject hitbox;
    
    public Jump() {
        if(hitboxPrefab == null) {
            hitboxPrefab = Resources.Load<GameObject>("collision_boxes/Hitbox");
        }
        
        OnStart.AddListener(() => {
            animator.SetTrigger("startJumping");
        });
        
        OnEnd.AddListener(() => {
            GameObject.Destroy(hitbox);
        });
    }
    
    public override bool isCancelableWith(GenericAction action) {
        return !(action is Fall);
    }
    
    public override void update() {
        if(getPower() < 0.0000001) {
            dispatchEnd();
        }
    }
    
    float getPower() {
        float progress = (float)step / duration;
        
        if(progress > 1) { progress = 1; }
        
        float power = first + Mathf.Pow(progress, exponent) * (last - first);
        
        return power;
        
    }
    
    float getPowerAt(float progress) {
        if(progress > 1) { progress = 1; }
        
        float power = first + Mathf.Pow(progress, exponent) * (last - first);
        
        return power;
    }
    
    public override void fixedUpdate() {
        if(fstep == 0) {
            
            // rigidbody.AddForce(new Vector2(0f, 700));
            
            Vector2 velocity = getUserScript().rigidbody.velocity;
            
            velocity.y = 0;
            
            getUserScript().rigidbody.velocity = velocity;
            
        }
        
        if(fstep == 1) {
            hitbox = GameObject.Instantiate(hitboxPrefab);
            
            hitbox.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            hitbox.GetComponent<Hitbox>().damage = 1;
            hitbox.GetComponent<Hitbox>().OnHit.AddListener((GameObject target) => {
                // GameObject gameObject = GameObject.Instantiate(Resources.Load<GameObject>("effects/SmokeExplosion"));
                // gameObject.transform.position = (target.transform.position + hitbox.transform.position) / 2;
                // gameObject.SetActive(true);
            });
            
            hitbox.transform.SetParent(user);
            hitbox.transform.localPosition = new Vector3(0, 0.5f - 0.375f/2, 0);
            hitbox.transform.localScale = new Vector3(0.875f, 0.375f, 0.5f);
            
            hitbox.SetActive(true);
        }
        
        else if(fstep == 16) {
            GameObject.Destroy(hitbox);
        }
        
        {
            Vector2 velocity = getUserScript().rigidbody.velocity;
            
            first = 552f;
            exponent = 1f/44;
            
            first = 384f;
            exponent = 1f/14;
            
            velocity.y += getPowerAt((float)fstep/duration*4) * Time.deltaTime;
            
            getUserScript().rigidbody.velocity = velocity;
            
            // getUserScript().rigidbody.AddForce(new Vector2(0f, power));
        }
        
    }
    
}
