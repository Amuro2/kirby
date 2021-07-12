using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Damageable : MonoBehaviour {
    
    public int health = 100;
    public int maxHealth = 100;
    public bool inhalable = false;
    public bool destroyOnDefeat = true;
    public bool stunnable = true;
    
    public UnityEvent OnHurt = new UnityEvent();
    public UnityEvent<int, int> OnHealthChange = new UnityEvent<int, int>();
    public UnityEvent OnDefeat = new UnityEvent();
    
    Hitbox hitbox;
    
    bool defeated;
    bool invincible;
    bool bypassKirbies;
    
    void Awake() {
        OnDefeat.AddListener(() => {
            GameObject effect = Instantiate(Resources.Load<GameObject>("effects/StarExplosion"));
            effect.transform.position = transform.position;
            effect.transform.Rotate(0, 0, (float)(-360f/5 + Random.value * (+360f/5 - -360f/5)));
            effect.SetActive(true);
        });
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
    public double getHealthRatio() {
        if(health < 0) { return 0; }
        
        if(health > maxHealth) { return 1; }
        
        return (double)health / maxHealth;
    }
    
    public void setHealth(int newHealth) {
        
        int oldHealth = health;
        
        health = newHealth;
        
        if(oldHealth != newHealth) {
            OnHealthChange.Invoke(newHealth, oldHealth);
        }
        
        // Debug.Log("Heatlh now: " + newHealth + " -- before: " + oldHealth);
        
        if(health <= 0 && !defeated) {
            // Debug.Log("Uhm... dead?");
            
            health = 0;
            
            OnDefeat.Invoke();
            
            if(destroyOnDefeat) {
                Destroy(gameObject);
            }
            
            defeated = true;
        }
        
        if(health > 0 && defeated) {
            defeated = false;
        }
        
        if(health > maxHealth) {
            health = maxHealth;
        }
        
    }
    
    public void hurt(int damage) {
        if(!invincible) {
            
            Kirby kirby = GetComponent<Kirby>();
            
            if(kirby != null) {
                kirby.bypassKirbies = bypassKirbies;
            }
            
            OnHurt.Invoke();
            
            setHealth(health - damage);
            
            if(kirby != null) {
                kirby.setAction(new HurtState(4));
            }
            
            if(stunnable && health > 0) {
                Timeout.setFixed(enableBehaviours, 4);
                disableBehaviours();
            }
        }
    }
    
    public void heal(int amount) {
        setHealth(health + amount);
    }
    
    public double getCurrentHealth() {
        return health;
    }
    
    public double getMaxHealth() {
        return maxHealth;
    }
    
    void disableBehaviours() {
        // Debug.Log(gameObject.name + " stun start.");
        
        foreach(MonoBehaviour component in gameObject.GetComponents<MonoBehaviour>()) {
            component.enabled = false;
        }
    }
    
    void enableBehaviours() {
        // Debug.Log(gameObject.name + " stun end.");
        
        foreach(MonoBehaviour component in gameObject.GetComponents<MonoBehaviour>()) {
            component.enabled = true;
        }
    }
    
    public void setInvincible(bool invincible) {
        this.invincible = invincible;
    }
    
    public bool isInvincible() { return invincible; }
    
    public void spawn(string prefabPath) {
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        
        if(prefab != null) {
            GameObject gameObject = Instantiate(prefab);
            gameObject.transform.position = transform.position;
            gameObject.SetActive(true);
        }
    }
    
    public void spawn(GameObject prefab) {
        if(prefab != null) {
            GameObject gameObject = Instantiate(prefab);
            gameObject.transform.position = transform.position;
            gameObject.SetActive(true);
        }
    }
    
    public void setBypassKirbies(bool bypassKirbies) {
        this.bypassKirbies = bypassKirbies;
    }
    
    public void setHitbox(Hitbox hitbox) {
        this.hitbox = hitbox;
    }
    
    public Hitbox getHitbox() {
        return hitbox;
    }
    
}
