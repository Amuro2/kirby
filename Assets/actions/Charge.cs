using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : GenericAction {
    
    public int time = 0;
    double c = 0;
    bool full;
    
    public Charge() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
            setUserStill(true);
        });
        
        OnEnd.AddListener(() => {
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    class ChargeLineBehaviour : MonoBehaviour {
        
        public Transform user;
        Vector3 initialPosition;
        
        void Start() {
            initialPosition = transform.position;
            
            GetComponent<Rigidbody2D>().velocity = (user.transform.position - initialPosition).normalized * 0.03125f;
        }
        
        void Update() {
            float progress = GetComponent<InitialColorTransition>().getProgress();
            
            transform.position = initialPosition + (user.transform.position - initialPosition) * progress;
            
            GetComponent<Rigidbody2D>().velocity = (user.transform.position - initialPosition).normalized * 0.03125f;
        }
        
    }
    
    public override void fixedUpdate() {
        if(c >= 125) {
            c = 0;
            
            for(int i = 0; i < 3; ++i) {
                GameObject line = GameObject.Instantiate(Resources.Load<GameObject>("effects/ChargeLine"));
                
                ChargeLineBehaviour behaviour = line.AddComponent<ChargeLineBehaviour>();
                behaviour.user = user;
                
                // line.transform.SetParent(user);
                
                float angle = Random.value * 2*Mathf.PI;
                float distance = 1 + Random.value * (1.25f - 1);
                
                line.transform.position = user.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
            }
            
            GameObject wave = GameObject.Instantiate(Resources.Load<GameObject>("effects/ChargeWave"));
            
            wave.transform.SetParent(user);
            wave.transform.position = user.position;
        }
        
        if(fstep >= 128/8 && !full) {
            full = true;
            
            for(int i = 0; i < 4; ++i) {
                
                Timeout.setFixed(() => {
                    GameObject wave = GameObject.Instantiate(Resources.Load<GameObject>("effects/ChargeWave2"));
                    
                    wave.transform.SetParent(user);
                    wave.transform.position = user.position;
                    
                    Timeout.setFixed(() => {
                        
                        wave = GameObject.Instantiate(Resources.Load<GameObject>("effects/ChargeWave2"));
                        
                        wave.GetComponent<SpriteRenderer>().color = new Color(239f/255, 191f/255, 255f/255);
                        wave.GetComponent<InitialColorTransition>().colorStart = new Color(239f/255, 191f/255, 255f/255);
                        wave.GetComponent<InitialColorTransition>().colorEnd = new Color(239f/255, 191f/255, 255f/255, 0);
                        
                        wave.transform.SetParent(user);
                        wave.transform.position = user.position;
                        
                    }, 1);
                    
                }, i * 3);
                
            }
            
            List<GameObject> objects = (new PrefabExplosion())
            .setCount(16)
            .setPrefab("effects/ChargeLine")
            .setCenter(user.position)
            .setRelativeAngleVariation(0.5)
            .setInitialDistance(0.75, 1)
            .setLaunchNorm(2, 3.25)
            .start()
            .getObjects();
            
            foreach(GameObject gameObject in objects) {
                gameObject.transform.SetParent(user);
                gameObject.GetComponent<InitialColorTransition>().exponent = 8;
            }
            
        }
        
        c += Time.deltaTime * 1000;
        ++time;
    }
    
    public override bool isCancelableWith(GenericAction action) {
        return action is Jump || action is Torpedo;
    }
    
    public float getTime() {
        return fstep;
    }
    
}
