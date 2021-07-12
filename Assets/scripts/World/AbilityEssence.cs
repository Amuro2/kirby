using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEssence : DetectionBox {
    
    public string ability;
    
    GameObject fill;
    GameObject stand;
    
    Vector3 initialPosition;
    
    public void Reset() {
        tagToDetect = "Player";
    }
    
    bool abilityUnlocked() {
        return PersistentStuff.abilityUnlocked(ability);
    }
    
    void Awake() {
        initialPosition = transform.position;
        
        for(int i = 0; i < transform.childCount; ++i) {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        fill = new GameObject();
        fill.name = "Fill";
        fill.AddComponent<SpriteRenderer>();
        fill.transform.SetParent(transform);
        fill.transform.localPosition = new Vector3(0, 0.1f, 0);
        
        stand = new GameObject();
        stand.name = "Stand";
        stand.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("images/ability_essence");
        stand.transform.SetParent(transform);
        stand.transform.position = transform.position;
        
        GetComponent<SpriteRenderer>().sprite = null;
        
        refresh();
    }
    
    void OnEnable() {
        
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    double step = 0;
    int c;
    double particleCountdown = 0;
    double particle2Countdown = 0;
    
    // Update is called once per frame
    void Update() {
        if(!abilityUnlocked()) {
            Vector3 position = initialPosition;
            
            position.y += Mathf.Sin((float)step * Mathf.PI/1024) * 0.125f;
            
            transform.position = position;
            
        } else {
            transform.position = initialPosition;
        }
        
        if(!abilityUnlocked() && particleCountdown <= 0) {
            GameObject particle = Instantiate(Resources.Load<GameObject>("effects/AbilityParticle"));
            
            particle.GetComponent<SpriteRenderer>().color = PersistentStuff.getAbilityColorDim(ability);
            particle.GetComponent<InitialColorTransition>().colorStart = PersistentStuff.getAbilityColorDim(ability);
            particle.GetComponent<InitialColorTransition>().colorEnd = PersistentStuff.getAbilityColorDim(ability);
            particle.GetComponent<InitialColorTransition>().colorEnd[3] = 0;
            
            Vector3 position = transform.position;
            
            position.x += Mathf.Sin(c * Mathf.PI/3) * transform.localScale.x/2;
            position.y -= transform.localScale.y/2;
            position.z += Mathf.Cos(c * Mathf.PI/3) * 0.25f;
            
            particle.transform.position = position;
            
            particle.transform.SetParent(transform);
            
            particleCountdown = 250;
            ++c;
        }
        
        if(!abilityUnlocked() && particle2Countdown <= 0) {
            
            // 
            
            // GameObject particle = Instantiate(Resources.Load<GameObject>("effects/AbilityParticle2"));
            
            // float angle = Random.value * 2*Mathf.PI;
            
            // Vector3 position = transform.position;
            
            // position += new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 0.5f;
            
            // particle.transform.position = position;
            
            // particle.GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 0.75f;
            
            // particle.transform.SetParent(transform);
            
            // 
            
            List<GameObject> particles = (new PrefabExplosion())
            .setCount(32)
            .setPrefab("effects/AbilityParticle2")
            .setCenter(transform.position)
            .setInitialAngle(Random.value * 2*Mathf.PI)
            .setLaunchNorm(0.875, 1.125)
            .start()
            .getObjects();
            
            foreach(GameObject particle in particles) {
                particle.transform.SetParent(transform);
            }
            
            particle2Countdown = 500;
        }
        
        if(detectsObjects() && !abilityUnlocked()) {
            GameObject splashScreen = Instantiate(Resources.Load<GameObject>("AbilitySplashCanvas"));
            
            splashScreen.SetActive(false);
            
            splashScreen.GetComponent<AbilitySplash>().ability = ability;
            
            splashScreen.SetActive(true);
            World.current.gameObject.SetActive(false);
            PersistentStuff.unlockAbility(ability);
            refresh();
        }
        
        step += Time.deltaTime * 1000;
        
        particleCountdown -= Time.deltaTime * 1000;
        particle2Countdown -= Time.deltaTime * 1000;
    }
    
    void refresh() {
        if(abilityUnlocked()) {
            fill.SetActive(false);
        } else {
            
            SpriteRenderer spriteRenderer = fill.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("images/icons/" + ability);
            
            Color color = spriteRenderer.color;
            color[3] = 0.8745098f;
            spriteRenderer.color = color;
            
        }
    }
    
}
