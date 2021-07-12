using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffBlockScript : PeriodicDecisionAI {
    
    Detector rangeDetect;
    
    public float attackRate = 2000;
    public float c = 0;
    
    static GameObject chargeWavePrefab;
    static GameObject releaseParticlePrefab;
    
    bool charging;
    Transform target;
    float attackTimeout;
    float chargeWaveTimeout;
    float chargeLineTimeout;
    
    void Awake() {
        rangeDetect = gameObject.GetComponent<Detector>();
        
        if(chargeWavePrefab == null) {
            chargeWavePrefab = Resources.Load<GameObject>("effects/ChargeWave");
        }
        
        if(releaseParticlePrefab == null) {
            releaseParticlePrefab = Resources.Load<GameObject>("effects/Star4");
        }
        
    }
    
    void Update() {
        if(rangeDetect.hasObjects()) {
            if(c >= attackRate) {
                
                target = rangeDetect.getRandomObject().transform;
                
                charging = true;
                
                attackTimeout = 500;
                
                c %= attackRate;
            }
            
            if(charging) {
                if(chargeWaveTimeout <= 0) {
                    GameObject particle = Instantiate(chargeWavePrefab);
                    
                    particle.transform.position = transform.position + new Vector3(Mathf.Sign(target.position.x - transform.position.x) * (transform.localScale.x) / 2, 0, 0);
                    particle.transform.SetParent(transform.parent);
                    
                    chargeWaveTimeout = 75;
                    
                }
                
                if(chargeLineTimeout <= 0) {
                    chargeLineTimeout = 0;
                }
                
                if(attackTimeout <= 0) {
                    charging = false;
                    
                    // 
                    
                    List<GameObject> objects = (new PrefabExplosion())
                    .setCount(8)
                    .setCenter(transform.position + new Vector3(Mathf.Sign(target.position.x - transform.position.x) * (transform.localScale.x) / 2, 0, 0))
                    .setPrefab(releaseParticlePrefab)
                    .setRelativeAngleVariation(0.5)
                    .setLaunchNorm(3, 6)
                    .start()
                    .getObjects();
                    
                    foreach(GameObject gameObject in objects) {
                        gameObject.transform.SetParent(transform.parent);
                    }
                    
                    // 
                    
                    for(int i = 0; i < 2; ++i) {
                        
                        Timeout.setMs(() => {
                            
                            GameObject projectile = Instantiate(Resources.Load<GameObject>("collision_boxes/TotemBullet"));
                            
                            // projectile.GetComponent<Hitbox>().whiteList.Add(gameObject);
                            projectile.GetComponent<Hitbox>().tagToHit = "Player";
                            
                            Vector3 initialPosition = transform.position;
                            
                            initialPosition.x += Mathf.Sign(target.transform.position.x - transform.position.x) * transform.localScale.x / 2;
                            
                            projectile.transform.position = initialPosition;
                            
                            projectile.GetComponent<Rigidbody2D>().velocity = (target.transform.position - initialPosition).normalized * 10;
                            
                            projectile.GetComponent<ContactVanish>().blackList.Add(gameObject);
                            
                            projectile.SetActive(true);
                            
                        }, i * 75);
                        
                    }
                    
                }
                
                chargeWaveTimeout -= Time.deltaTime * 1000;
                chargeLineTimeout -= Time.deltaTime * 1000;
                attackTimeout -= Time.deltaTime * 1000;
                
            }
            
            c += Time.deltaTime * 1000;
        }
        
    }
    
}
