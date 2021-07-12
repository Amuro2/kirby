using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class InhaleBox : MonoBehaviour {
    
    public Transform origin;
    
    public UnityEvent<GameObject> OnInhaleStart = new UnityEvent<GameObject>();
    public UnityEvent<GameObject> OnInhaleEnd = new UnityEvent<GameObject>();
    public UnityEvent<GameObject> OnInhaleInterrupt = new UnityEvent<GameObject>();
    
    HashSet<GameObject> inhaling = new HashSet<GameObject>();
    
    public Dictionary<GameObject, Vector3> originalLocalScales = new Dictionary<GameObject, Vector3>();
    
    void Awake() {
        OnInhaleStart.AddListener((GameObject gameObject) => {
            Hitbox hitbox = gameObject.GetComponent<Hitbox>();
            
            if(hitbox != null) {
                hitbox.enabled = false;
            }
        });
        
        OnInhaleInterrupt.AddListener((GameObject gameObject) => {
            Hitbox hitbox = gameObject.GetComponent<Hitbox>();
            
            if(hitbox != null) {
                hitbox.enabled = true;
            }
        });
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position - transform.localScale/2, transform.position + transform.localScale/2);
        
        HashSet<GameObject> clone = new HashSet<GameObject>(inhaling);
        
        foreach(GameObject gameObject in clone) {
            bool found = false;
            
            for(int i = 0; i < colliders.Length && !found; ++i) {
                if(colliders[i].gameObject == gameObject) {
                    found = true;
                }
            }
            
            if(!found) {
                OnInhaleInterrupt.Invoke(gameObject);
                
                originalLocalScales.Remove(gameObject);
                inhaling.Remove(gameObject);
            }
        }
        
        foreach(Collider2D collider in colliders) {
            GameObject gameObject = collider.gameObject;
            
            if(predicateCollider(gameObject)) {
                if(!inhaling.Contains(gameObject)) {
                    inhaling.Add(gameObject);
                    originalLocalScales.Add(gameObject, gameObject.transform.localScale);
                }
                
                Vector2 direction = origin.position - gameObject.transform.position;
                
                float norm = direction.magnitude;
                
                float progression = norm / transform.localScale.x;
                
                if(progression > 1) {
                    progression = 1;
                }
                
                float newNorm = 0.03125f + Mathf.Pow(progression, 2) * (0.015625f - 0.03125f);
                
                if(norm > 0.0625) {
                    direction.x *= newNorm / norm;
                    direction.y *= newNorm / norm;
                    
                    Vector3 newLocalScale = originalLocalScales[gameObject] * Mathf.Pow(progression, 0.0625f);
                    gameObject.transform.localScale = newLocalScale;
                    gameObject.transform.position += new Vector3(direction.x, direction.y, 0);
                    
                    OnInhaleStart.Invoke(gameObject);
                    
                    Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
                    
                    if(rigidbody != null) {
                        Destroy(rigidbody);
                    }
                } else {
                    OnInhaleEnd.Invoke(gameObject);
                    
                    originalLocalScales.Remove(gameObject);
                    inhaling.Remove(gameObject);
                    
                    gameObject.transform.position -= new Vector3(0, 0, 999999999);
                    Damageable damageable = gameObject.GetComponent<Damageable>();
                    
                    if(damageable != null) {
                        damageable.hurt(999999999);
                    }
                    
                    GameObject.Destroy(gameObject);
                }
            }
        }
        
    }
    
    bool predicateCollider(GameObject gameObject) {
        return (gameObject.GetComponent<Damageable>() != null && gameObject.GetComponent<Damageable>().inhalable) || gameObject.GetComponent<Inhalable>() != null;
    }
    
    void OnDestroy() {
        foreach(GameObject gameObject in inhaling) {
            gameObject.transform.localScale = originalLocalScales[gameObject];
            
            OnInhaleInterrupt.Invoke(gameObject);
        }
    }
    
    public bool isInhaling() {
        return inhaling.Count > 0;
    }
    
}
