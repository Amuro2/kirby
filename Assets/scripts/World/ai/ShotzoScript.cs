using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotzoScript : MonoBehaviour {
    
    Detector rangeDetect;
    
    public float attackRate = 2000;
    public float c = 0;
    
    static GameObject chargeWavePrefab;
    static GameObject releaseWavePrefab;
    static GameObject bulletPrefab;
    
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
        
        if(releaseWavePrefab == null) {
            releaseWavePrefab = Resources.Load<GameObject>("effects/SmokeExplosion");
        }
        
        if(bulletPrefab == null) {
            bulletPrefab = Resources.Load<GameObject>("collision_boxes/ShotzoBullet");
        }
        
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
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
                    
                    // particle.transform.position = transform.position + new Vector3(Mathf.Sign(target.position.x - transform.position.x) * (transform.localScale.x) / 2, 0, 0);
                    
                    Vector3 direction = target.position - transform.position;
                    particle.transform.position = transform.position + direction.normalized * (transform.localScale.x + transform.localScale.y) / 4;
                    
                    particle.transform.SetParent(transform.parent);
                    
                    chargeWaveTimeout = 75;
                    
                }
                
                if(chargeLineTimeout <= 0) {
                    chargeLineTimeout = 0;
                }
                
                if(attackTimeout <= 0) {
                    charging = false;
                    
                    Vector3 direction = target.position - transform.position;
                    
                    // 
                    
                    List<GameObject> objects = (new PrefabExplosion())
                    .setCount(32)
                    // .setCenter(transform.position + new Vector3(Mathf.Sign(target.position.x - transform.position.x) * (transform.localScale.x) / 2, 0, 0))
                    .setCenter(transform.position + direction.normalized * (transform.localScale.x + transform.localScale.y) / 4)
                    .setPrefab("effects/SmokeBall")
                    .setRelativeAngleVariation(0.5)
                    .setLaunchNorm(3, 6)
                    .start()
                    .getObjects();
                    
                    foreach(GameObject gameObject in objects) {
                        gameObject.transform.SetParent(transform.parent);
                    }
                    
                    // GameObject wave = Instantiate(releaseWavePrefab);
                    
                    // wave.transform.position = transform.position + new Vector3(Mathf.Sign(target.position.x - transform.position.x) * (transform.localScale.x) / 2, 0, 0);
                    // wave.transform.position = transform.position + direction.normalized * (transform.localScale.x + transform.localScale.y) / 4;
                    
                    // wave.transform.SetParent(transform.parent);
                    
                    // 
                    
                    for(int i = 0; i < 3; ++i) {
                        
                        Timeout.setMs(() => {
                            
                            GameObject projectile = Instantiate(bulletPrefab);
                            
                            // projectile.GetComponent<Hitbox>().whiteList.Add(gameObject);
                            projectile.GetComponent<Hitbox>().tagToHit = "Player";
                            
                            Vector3 initialPosition = transform.position;
                            
                            // initialPosition.x += Mathf.Sign(target.transform.position.x - transform.position.x) * transform.localScale.x / 2;
                            initialPosition += direction.normalized * (transform.localScale.x + transform.localScale.y) / 4;
                            
                            projectile.transform.position = initialPosition;
                            
                            projectile.GetComponent<Rigidbody2D>().velocity = (target.transform.position - initialPosition).normalized * 10;
                            
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
