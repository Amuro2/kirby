using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpit : GenericAction {
    
    public StarSpit() {
        OnStart.AddListener(() => {
            animator.SetTrigger("startSpitting");
            
            // 
            
            (new PrefabExplosion())
            .setCount(5)
            .setCenter(user.position + new Vector3(getUserFacingX() * user.localScale.x/2, 0, 0))
            .setAngleStartEndRelativeToDirection(new Vector2(getUserFacingX(), 0), Mathf.PI/4)
            .setLaunchNorm(3)
            .setPrefab("effects/WaveLine")
            .addInitListener((GameObject gameObject) => {
                gameObject.transform.localScale = new Vector3(0.5f, 2f, 1);
                gameObject.transform.SetParent(user.parent);
            })
            .start()
            .setLaunchNorm(8)
            .start();
            
            // 
            
            GameObject projectile = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/StarBullet"));
            
            Vector3 position = user.position;
            position.x += getUserFacingX() * user.localScale.x/2;
            projectile.transform.position = position;
            
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(getUserFacingX() * 24, 0);
            
            projectile.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
            projectile.GetComponent<Hitbox>().damage *= getUserScript().getBellyCount();
            
            projectile.transform.SetParent(user.parent);
            
            getUserScript().setBellyCount(0);
        });
        
        OnEnd.AddListener(() => {
            
        });
    }
    
    public override void fixedUpdate() {
        if(fstep == 16/4) {
            dispatchEnd();
        }
    }
    
}
