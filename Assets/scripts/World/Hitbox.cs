using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Hitbox : MonoBehaviour {
    
    public int damage = 0;
    
    public int rehitRate = -1;
    public bool bypassKirbies = false;
    public bool bypassInvincibility = false;
    public float pushBack = 0;
    
    public List<GameObject> whiteList = new List<GameObject>();
    public string tagToHit;
    
    public UnityEvent<GameObject> OnHit = new UnityEvent<GameObject>();
    
    Dictionary<GameObject, int> rehitMap = new Dictionary<GameObject, int>();
    
    void Awake() {
        whiteList.Add(gameObject);
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        IEnumerable<GameObject> colliders = getColliders();
        
        string str = "";
        
        foreach(GameObject collider in colliders) {
            Damageable damageable = collider.GetComponent<Damageable>();
            
            if(!damageable.isInvincible() || bypassInvincibility) {
                dispatchHit(collider);
            }
            
            str += collider.name + " ";
        }
        
        if(str != "") {
            // Debug.Log(str);
        }
        
        registerHit(colliders);
        
        updateRehit();
    }
    
    bool predicateCollider(GameObject gameObject) {
        return damageableIsValid(gameObject.GetComponent<Damageable>()) && !whiteList.Contains(gameObject) && !rehitMap.ContainsKey(gameObject) && (tagToHit == null || tagToHit == "" || gameObject.tag == tagToHit);
    }
    
    public IEnumerable<GameObject> getColliders() {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position - transform.localScale/2, transform.position + transform.localScale/2);
        
        HashSet<GameObject> filtered = new HashSet<GameObject>();
        
        foreach(Collider2D collider in colliders) {
            if(predicateCollider(collider.gameObject)) {
                filtered.Add(collider.gameObject);
            }
        }
        
        return filtered;
    }
    
    public void registerHit(IEnumerable<GameObject> colliders) {
        if(rehitRate != 0) {
            foreach(GameObject collider in colliders) {
                rehitMap[collider] = rehitRate;
            }
        }
    }
    
    public void updateRehit() {
        Dictionary<GameObject, int> clone = new Dictionary<GameObject, int>(rehitMap);
        
        foreach(GameObject collider in clone.Keys) {
            if(rehitMap[collider] > 0) {
                --rehitMap[collider];
                
                if(rehitMap[collider] == 0) {
                    rehitMap.Remove(collider);
                }
            }
        }
    }
    
    public void dispatchHit(GameObject target) {
        Damageable damageable = target.GetComponent<Damageable>();
        
        if(damageableIsValid(damageable)) {
            damageable.setBypassKirbies(bypassKirbies);
            damageable.setHitbox(this);
            damageable.hurt(damage);
            damageable.setInvincible(true);
            Timeout.setFixed(() => {
                damageable.setInvincible(false);
            }, 4);
        }
        
        Rigidbody2D rigidbody = target.GetComponent<Rigidbody2D>();
        
        if(rigidbody != null && pushBack != 0) {
            Vector2 velocity = rigidbody.velocity;
            
            velocity.x = Mathf.Sign(target.transform.position.x - transform.position.x);
            velocity.y = 0.125f;
            
            rigidbody.velocity = velocity.normalized * pushBack;
        }
        
        OnHit.Invoke(target);
        
    }
    
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
    
    bool damageableIsValid(Damageable damageable) {
        return damageable != null && damageable.enabled;
    }
    
}
