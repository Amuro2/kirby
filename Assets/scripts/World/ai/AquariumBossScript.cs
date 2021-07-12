using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumBossScript : MonoBehaviour {
    
    static GameObject chargeWavePrefab;
    static GameObject releaseParticlePrefab;
    
    Detector detector;
    
    float decisionRate = 3000;
    float c = 0;
    
    Transform target;
    int actionId = -1;
    float strikeTimeout;
    
    void Awake() {
        detector = gameObject.GetComponent<Detector>();
        
        if(chargeWavePrefab == null) {
            chargeWavePrefab = Resources.Load<GameObject>("effects/ChargeWave");
        }
        
        if(releaseParticlePrefab == null) {
            releaseParticlePrefab = Resources.Load<GameObject>("effects/Star4");
        }
        
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        
        if(detector.hasObjects()) {
            
            if(c >= decisionRate) {
                target = detector.getRandomObject().transform;
                
                actionId = (int)(Random.value * 3);
                
                if(actionId == 0) { strikeTimeout = 250; }
                if(actionId == 1) { strikeTimeout = 250; }
                if(actionId == 2) { strikeTimeout = 250; }
                
                c %= decisionRate;
            }
            
            //// Projectile shot ////
            
            if(actionId == 0) {
                if(strikeTimeout >= 0) {
                    GameObject particle = Instantiate(chargeWavePrefab);
                    
                    particle.transform.position = transform.position + new Vector3(Mathf.Sign(target.position.x - transform.position.x) * (transform.localScale.x) / 2, 0, 0);
                    particle.transform.SetParent(transform.parent);
                }
                
                if(strikeTimeout <= 0) {
                    
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
                    
                    actionId = -1;
                    
                }
            }
            
            //// Dash ////
            
            if(actionId == 1) {
                if(strikeTimeout >= 0) {
                    GameObject particle = Instantiate(chargeWavePrefab);
                    
                    particle.transform.position = transform.position;
                    particle.transform.localScale = transform.localScale;
                    
                    particle.transform.SetParent(transform.parent);
                }
                
                if(strikeTimeout <= 0) {
                    
                    Hitbox hitbox = gameObject.AddComponent<Hitbox>();
                    hitbox.damage = 10;
                    
                    Timeout.setMs(() => {
                        Destroy(hitbox);
                    }, 1000);
                    
                    GetComponent<Rigidbody2D>().velocity = (target.position - transform.position).normalized * 16;
                    
                    actionId = -1;
                    
                }
            }
            
            //// Needle Burst ////
            
            if(actionId == 2) {
                if(strikeTimeout >= 0) {
                    GameObject particle = Instantiate(chargeWavePrefab);
                    
                    particle.transform.position = transform.position;
                    particle.transform.localScale = transform.localScale;
                    
                    particle.transform.SetParent(transform.parent);
                }
                
                if(strikeTimeout <= 0) {
                    
                    for(float i = 0, count = 16; i < count; ++i) {
                        
                        float angle = i/count * 2*Mathf.PI;
                        Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                        
                        GameObject projectile = Instantiate(Resources.Load<GameObject>("collision_boxes/TotemBullet"));
                        
                        // projectile.GetComponent<Hitbox>().whiteList.Add(gameObject);
                        projectile.GetComponent<Hitbox>().tagToHit = "Player";
                        
                        projectile.transform.position = transform.position + direction.normalized * 2;
                        
                        projectile.GetComponent<Rigidbody2D>().velocity = direction.normalized * 10;
                        
                        projectile.GetComponent<ContactVanish>().blackList.Add(gameObject);
                        
                        projectile.SetActive(true);
                        
                    }
                    
                    actionId = -1;
                    
                }
            }
            
            strikeTimeout -= Time.deltaTime * 1000;
            c += Time.deltaTime * 1000;
        }
        
    }
    
}
