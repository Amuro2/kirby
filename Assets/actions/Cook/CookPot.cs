using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookPot : GenericAction {
    
    static GameObject cookPotPrefab;
    static PrefabExplosion explosion;
    
    GameObject cookPotObject;
    
    GameObject cookPotBox;
    int gatheredCount;
    
    int endStep = 768/4;
    
    public CookPot() {
        if(cookPotPrefab == null) {
            cookPotPrefab = Resources.Load<GameObject>("effects/CookPot");
        }
        
        if(explosion == null) {
            explosion = new PrefabExplosion();
            
            explosion.setCount(32);
            explosion.setPrefab("effects/SmokeBall");
            explosion.setLaunchNorm(4, 6);
            explosion.setAngleVariation(Mathf.PI/32);
        }
        
        explosion.clearInitListeners();
        explosion.addInitListener((GameObject smokeBall) => {
            smokeBall.transform.SetParent(user.parent);
        });
        
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
            setUserStill(true);
            
            user.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            
            PersistentStuff.emptyAbilityGauge("Cook");
            
            // user.gameObject.GetComponent<Damageable>().enabled = false;
            user.GetComponent<Damageable>().setInvincible(true);
        });
        
        OnEnd.AddListener(() => {
            
            InitialScaleTransition st = cookPotObject.AddComponent<InitialScaleTransition>();
            
            st.scaleStart = new Vector3(1, 1, 1);
            st.scaleEnd = new Vector3(0, 0, 0);
            st.msDuration = 125;
            st.exponent = 1f/2f;
            st.OnEnd.AddListener(() => {
                explosion.setCenter(cookPotObject.transform.position);
                explosion.start();
            });
            st.OnEnd.AddListener(st.destroy);
            
            user.GetComponent<Damageable>().setInvincible(false);
            // user.gameObject.GetComponent<Damageable>().enabled = true;
            GameObject.Destroy(cookPotBox);
            
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    public override void update() {
        user.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
    
    public override void fixedUpdate() {
        if(fstep == 0) {
            
            gatheredCount = 1;
            
            animator.SetTrigger("startCookPot0");
            
            cookPotBox = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/CookPotBox"));
            
            cookPotBox.transform.SetParent(user);
            
            cookPotBox.transform.localPosition = new Vector3(0, 0, 0);
            
            cookPotBox.GetComponent<CookPotBox>().OnAllIn.AddListener(() => {
                
                gatheredCount += cookPotBox.GetComponent<CookPotBox>().gatheredCount;
                
                GameObject.Destroy(cookPotBox);
                cookPotBox = null;
                
                endStep = fstep + 512/4;
                
                animator.SetTrigger("startCookPot1");
                
                // 
                
                InitialPositionTransition pt = cookPotObject.AddComponent<InitialPositionTransition>();
                
                pt.setPositionStart(cookPotObject.transform.position);
                pt.setPositionEnd(user.position + new Vector3(0, -0.625f, -0.03125f));
                pt.msDuration = 250;
                pt.exponent = 1f/3f;
                pt.OnEnd.AddListener(pt.removeComponent);
                
            });
            
            cookPotBox.SetActive(true);
            
            // 
            
            cookPotObject = GameObject.Instantiate(cookPotPrefab);
            
            cookPotObject.transform.position = user.transform.position + new Vector3(getUserFacingX() * 1, 0.5f, 0.03125f);
            
            cookPotObject.transform.SetParent(user.parent);
            
            InitialScaleTransition st = cookPotObject.AddComponent<InitialScaleTransition>();
            
            st.scaleStart = new Vector3(0, 0, 0);
            st.scaleEnd = new Vector3(1, 1, 1);
            st.msDuration = 125;
            st.exponent = 1f/2f;
            st.OnEnd.AddListener(() => {
                explosion.setCenter(cookPotObject.transform.position);
                explosion.start();
            });
            st.OnEnd.AddListener(st.removeComponent);
            
        }
        
        if(cookPotBox != null) {
            
            cookPotBox.GetComponent<CookPotBox>().origin = user.transform.position + new Vector3(0, 2, 0);
            
            if(fstep == 512/4 && !cookPotBox.GetComponent<CookPotBox>().started) {
                
                cookPotBox.GetComponent<CookPotBox>().dispatchAllIn();
                
            }
            
        }
        
        else {
            
        }
        
        if(fstep == endStep) {
            for(int i = 0; i < gatheredCount; ++i) {
                makeFood();
            }
            
            dispatchEnd();
        }
    }
    
    void makeFood() {
        GameObject food = GameObject.Instantiate(Resources.Load<GameObject>("entities/Cherry"));
        
        food.transform.SetParent(user.parent);
        
        food.transform.localPosition = new Vector3(0, 2, 0);
        
        float minValue = Mathf.PI/2 - 0.03125f;
        float maxValue = Mathf.PI/2 + 0.03125f;
        float angle = minValue + Random.value * (maxValue - minValue);
        
        float power = 6;
        
        food.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle) * power, Mathf.Sin(angle) * power);
        
        food.SetActive(true);
    }
    
}
