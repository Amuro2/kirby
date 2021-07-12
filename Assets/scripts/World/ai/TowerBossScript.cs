using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBossScript : MonoBehaviour {
    
    static GameObject chargeWavePrefab;
    static GameObject releaseParticlePrefab;
    
    Detector detector;
    
    float decisionRate = 3000;
    float c = 0;
    
    Transform target;
    int actionId = -1;
    float strikeTimeout;
    
    class BlockTracer : MonoBehaviour {
        public class Block : MonoBehaviour {
            public static List<GameObject> blocks = new List<GameObject>();
            
            void Start() {
                blocks.Add(gameObject);
            }
            
            void OnDestroy() {
                blocks.Remove(gameObject);
            }
        }
        
        void Update() {
            
            Vector3 position = transform.position;
            position.x = Mathf.Round(position.x - 0.5f) + 0.5f;
            position.y = Mathf.Round(position.y - 0.5f) + 0.5f;
            
            float x = position.x;
            float y = position.y;
            
            bool canBePlaced = true;
            
            foreach(GameObject block in Block.blocks) {
                float blockX1 = block.transform.position.x - block.transform.localScale.x/2;
                float blockX2 = block.transform.position.x + block.transform.localScale.x/2;
                float blockY1 = block.transform.position.y - block.transform.localScale.y/2;
                float blockY2 = block.transform.position.y + block.transform.localScale.y/2;
                
                if(x >= blockX1 && x <= blockX2 && y >= blockY1 && y <= blockY2) {
                    canBePlaced = false;
                    
                    break;
                }
            }
            
            if(canBePlaced) {
                GameObject block = GameObject.Instantiate(Resources.Load<GameObject>("entities/GolemBlock"));
                
                block.transform.position = position;
                
                block.transform.SetParent(transform.parent);
                
                block.AddComponent<Block>();
            }
            
        }
    }
    
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
                
                actionId = (int)(Random.value * 5);
                
                if(actionId == 0) { strikeTimeout = 250; }
                if(actionId == 1) { strikeTimeout = 250; }
                if(actionId >= 2) { strikeTimeout = 250; }
                
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
                    
                    GameObject projectile = Instantiate(Resources.Load<GameObject>("collision_boxes/TotemBullet"));
                    
                    projectile.AddComponent<BlockTracer>();
                    
                    // projectile.GetComponent<Hitbox>().whiteList.Add(gameObject);
                    projectile.GetComponent<Hitbox>().tagToHit = "Player";
                    projectile.GetComponent<Hitbox>().OnHit.AddListener((GameObject gameObject) => {
                        Destroy(projectile);
                    });
                    
                    Vector3 initialPosition = transform.position;
                    
                    initialPosition.x += Mathf.Sign(target.transform.position.x - transform.position.x) * transform.localScale.x / 2;
                    
                    projectile.transform.position = initialPosition;
                    
                    projectile.GetComponent<Rigidbody2D>().velocity = (target.transform.position - initialPosition).normalized * 10;
                    
                    projectile.GetComponent<ContactVanish>().blackList.Add(gameObject);
                    
                    projectile.SetActive(true);
                    
                    // 
                    
                    actionId = -1;
                    
                }
            }
            
            //// Move ////
            
            if(actionId == 1) {
                if(strikeTimeout >= 0) {
                    
                }
                
                if(strikeTimeout <= 0) {
                    
                    Vector3 center = new Vector3(34, 4.5f, 0);
                    Vector3 targetDirection = target.position - center;
                    
                    GetComponent<Rigidbody2D>().velocity = ((center - targetDirection) - transform.position).normalized * 16;
                    
                    actionId = -1;
                    
                }
            }
            
            //// Axis projectile ////
            
            if(actionId >= 2) {
                if(strikeTimeout >= 0) {
                    
                }
                
                if(strikeTimeout <= 0) {
                    
                    GameObject projectile = Instantiate(Resources.Load<GameObject>("collision_boxes/TotemBullet"));
                    
                    projectile.AddComponent<BlockTracer>();
                    
                    // projectile.GetComponent<Hitbox>().whiteList.Add(gameObject);
                    projectile.GetComponent<Hitbox>().tagToHit = "Player";
                    projectile.GetComponent<Hitbox>().OnHit.AddListener((GameObject gameObject) => {
                        Destroy(projectile);
                    });
                    
                    if((int)(Random.value * 2) == 0) {
                        
                        float side = (int)(Random.value * 2);
                        // float x = 21 + side * (47 - 21);
                        float x = 21.6f + side * (46.4f - 21.6f);
                        float y = (int)(Random.value * 11);
                        y = y - 0.5f;
                        
                        projectile.transform.position = new Vector3(x, y, 0);
                        
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector3(-(side*2-1), 0, 0) * 10;
                        
                    }
                    
                    else {
                        
                        float side = (int)(Random.value * 2);
                        // float y = -1 + side * (10 + 1);
                        float y = -0.4f + side * (9.4f + 0.4f);
                        float x = (int)(Random.value * 26);
                        x = x + 21.5f;
                        
                        projectile.transform.position = new Vector3(x, y, 0);
                        
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -(side*2-1), 0) * 10;
                        
                    }
                    
                    projectile.GetComponent<ContactVanish>().blackList.Add(gameObject);
                    
                    projectile.SetActive(true);
                    
                    // 
                    
                    actionId = -1;
                    
                }
            }
            
            
            
        }
        
        strikeTimeout -= Time.deltaTime * 1000;
        c += Time.deltaTime * 1000;
    }
}
