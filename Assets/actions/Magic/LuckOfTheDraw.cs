using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckOfTheDraw : GenericAction {
    
    static GameObject fireworksPrefab;
    
    public LuckOfTheDraw() {
        if(fireworksPrefab == null) {
            fireworksPrefab = Resources.Load<GameObject>("collision_boxes/LuckOfTheDrawHitbox");
        }
        
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
            setUserStill(true);
        });
        
        OnEnd.AddListener(() => {
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    public override void fixedUpdate() {
        if(fstep == 0) {
            animator.SetTrigger("startLuckOfTheDraw");
        }
        
        if(fstep == 512/4) {
            switch((int)(Random.value * 6)) {
                case 0: {// 1UP
                    // Debug.Log("1UP");
                    
                    GameObject oneUp = GameObject.Instantiate(Resources.Load<GameObject>("entities/1UP"));
                    
                    oneUp.transform.position = user.position + new Vector3(0, 2, 0);
                    
                    break;
                }
                case 1:
                case 2: {// Cherries
                    // Debug.Log("Cherries");
                    
                    for(int i = 0; i < (int)(Random.value * 4) + 1; ++i) {
                        GameObject cherry = GameObject.Instantiate(Resources.Load<GameObject>("entities/Cherry"));
                        
                        cherry.transform.position = user.position + new Vector3(0, 2, 0);
                    }
                    
                    break;
                }
                case 3:
                case 4:
                case 5: {// Fireworks
                    // Debug.Log("Fireworks");
                    
                    GameObject hitbox = GameObject.Instantiate(fireworksPrefab);
                    
                    hitbox.transform.position = user.position;
                    hitbox.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
                    
                    break;
                }
            }
            
            PersistentStuff.emptyAbilityGauge("Magic");
            
            dispatchEnd();
        }
    }
    
}
