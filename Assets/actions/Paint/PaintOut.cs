using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintOut : GenericAction {
    
    GameObject hitbox;
    
    public PaintOut() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
            setUserStill(true);
            
            PersistentStuff.emptyAbilityGauge("Paint");
            
        });
        
        OnEnd.AddListener(() => {
            GameObject.Destroy(hitbox);
            
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    public override void update() {
        
    }
    
    float[] shuffleValues(float[] values) {
        List<float> possibleValues = new List<float>(values);
        float[] res = new float[values.Length];
        
        int i = 0;
        
        while(possibleValues.Count > 0) {
            int index = (int)(Random.value * possibleValues.Count);
            float item = possibleValues[index];
            
            res[i] = item;
            
            ++i;
            
            possibleValues = new List<float>(possibleValues);
            possibleValues.Remove(item);
        }
        
        return res;
    }
    
    float random(float min, float max) {
        return min + Random.value * (max - min);
    }
    
    int irandom(int min, int max) {
        return min + (int)(Random.value * (max - min) + 1);
    }
    
    public override void fixedUpdate() {
        if(fstep == 0) {
            animator.SetTrigger("startPaintOut0");
            airStall();
            
        }
        
        if(fstep == (0 + 48) / 4) {
            paintOutHitbox();
            
            // Draw splashes in the foreground
            
            splashPaint(0, 8, (432 - 48)/4);
            
        }
        
        if(fstep == (128) / 4) {
            animator.SetTrigger("startPaintOut1");
            airStall();
            
        }
        
        if(fstep == (128 + 48) / 4) {
            paintOutHitbox();
            
            splashPaint(4, 12, (432 - (128+48))/4);
        }
        
        if(fstep == (256) / 4) {
            animator.SetTrigger("startPaintOut2");
            airStall();
            
        }
        
        if(fstep == (256 + 48) / 4) {
            paintOutHitbox();
            
            splashPaint(8, 14, (432 - (256+48))/4);
        }
        
        if(fstep == (384) / 4) {
            dispatchEnd();
        }
    }
    
    void paintOutHitbox() {
        
        hitbox = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/PaintOutHitbox"));
        
        hitbox.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
        
        hitbox.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
            
            GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/PaintSplash"));
            effect.transform.position = collider.transform.position;
            effect.SetActive(true);
            
        });
        
        hitbox.transform.SetParent(user);
        
        hitbox.transform.localPosition = new Vector3(0, 0, 0);
        
        hitbox.SetActive(true);
        
    }
    
    void splashPaint(float initialDistanceMin, float initialDistanceMax, float timeout) {
        
        (new PrefabExplosion())
        .setCount(16)
        // .setCount(1)
        .setCenter(user.position + new Vector3(0, 0, -1))
        .setPrefab("effects/PaintOutDroplet")
        .setInitialDistance(initialDistanceMin, initialDistanceMax)
        .setRelativeAngleVariation(0.5)
        .addInitListener((GameObject droplet) => {
            float[] values = shuffleValues(new float[] {1, Random.value, 0});
            Color color = new Color(values[0], values[1], values[2]);
            
            droplet.GetComponent<SpriteRenderer>().color = color;
            
            InitialScaleTransition st = droplet.AddComponent<InitialScaleTransition>();
            float sz = random(2, 4);
            st.scaleStart = new Vector3(0, 0, 0);
            st.scaleEnd = new Vector3(sz, sz, sz);
            st.msDuration = random(50, 125);
            st.exponent = 1f/2;
            st.OnEnd.AddListener(st.removeComponent);
            
            GameObject.Destroy(droplet.GetComponent<Rigidbody2D>());
            
            droplet.transform.SetParent(user.parent);
            
            Timeout.setFixed(() => {
                InitialScaleTransition st = droplet.AddComponent<InitialScaleTransition>();
                st.scaleStart = new Vector3(sz, sz, sz);
                st.scaleEnd = new Vector3(0, 0, 0);
                st.msDuration = random(50, 250);
                st.exponent = 1f/2;
                st.OnEnd.AddListener(() => {
                    GameObject.Destroy(droplet);
                });
            }, (int)timeout + irandom(-4, +4)).transform.SetParent(user.parent);
            
            (new PrefabExplosion())
            .setCount(32)
            // .setCount(1)
            .setCenter(droplet.transform.position)
            .setPrefab("effects/PaintOutDroplet")
            .setInitialDistance(sz/2)
            .setInitialDistance(0.5 - 0.0, 0.5 + 0.125)
            .setRelativeAngleVariation(0.5)
            .addInitListener((GameObject gameObject) => {
                gameObject.GetComponent<SpriteRenderer>().color = color;
                
                InitialScaleTransition st = gameObject.AddComponent<InitialScaleTransition>();
                float sz = random(0.03125f, 0.125f);
                st.scaleStart = new Vector3(0, 0, 0);
                st.scaleEnd = new Vector3(sz, sz, sz);
                st.msDuration = random(50, 125);
                st.exponent = 1f/2;
                st.OnEnd.AddListener(st.removeComponent);
                
                GameObject.Destroy(gameObject.GetComponent<Rigidbody2D>());
                
                gameObject.transform.SetParent(droplet.transform);
                
                Timeout.setFixed(() => {
                    if(gameObject != null) {
                        InitialScaleTransition st = gameObject.AddComponent<InitialScaleTransition>();
                        st.scaleStart = new Vector3(sz, sz, sz);
                        st.scaleEnd = new Vector3(0, 0, 0);
                        st.msDuration = random(50, 250);
                        st.exponent = 1f/2;
                        st.OnEnd.AddListener(() => {
                            GameObject.Destroy(gameObject);
                        });
                    }
                }, (int)timeout + irandom(-4, +4)).transform.SetParent(user.parent);
                
            })
            .start();
        })
        .start();
        ;
        
    }
    
}
