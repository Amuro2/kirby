using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class CookPotBox : MonoBehaviour {
    
    public Vector3 origin;
    
    public UnityEvent OnAllIn = new UnityEvent();
    
    HashSet<GameObject> gathering = new HashSet<GameObject>();
    
    public Dictionary<GameObject, Vector3> originalLocalScales = new Dictionary<GameObject, Vector3>();
    
    public int gatheredCount = 0;
    public bool started = false;
    
    void Awake() {
        
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position - transform.localScale/2, transform.position + transform.localScale/2);
        
        foreach(Collider2D collider in colliders) {
            GameObject gameObject = collider.gameObject;
            
            if(predicate(gameObject)) {
                started = true;
                
                if(!gathering.Contains(gameObject)) {
                    gathering.Add(gameObject);
                    originalLocalScales.Add(gameObject, gameObject.transform.localScale);
                }
                
                Vector2 direction = origin - gameObject.transform.position;
                
                float norm = direction.magnitude;
                
                float progression = norm / (transform.localScale.x/2);
                
                if(progression > 1) { progression = 1; }
                
                float newNorm = 0.03125f + Mathf.Pow(progression, 2) * (0.015625f - 0.03125f);
                
                if(norm > 0.0625) {
                    direction.x *= newNorm / norm;
                    direction.y *= newNorm / norm;
                    
                    Vector3 newLocalScale = originalLocalScales[gameObject] * Mathf.Pow(progression, 0.0625f);
                    gameObject.transform.localScale = newLocalScale;
                    gameObject.transform.position += new Vector3(direction.x, direction.y, 0);
                    
                    Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
                    
                    if(rigidbody != null) {
                        Destroy(rigidbody);
                    }
                } else {
                    originalLocalScales.Remove(gameObject);
                    gathering.Remove(gameObject);
                    
                    Damageable damageable = gameObject.GetComponent<Damageable>();
                    
                    if(damageable != null) {
                        damageable.hurt(999999999);
                    }
                    
                    GameObject.Destroy(gameObject);
                    
                    ++gatheredCount;
                }
            }
        }
        
        if(started && gathering.Count == 0) {
            dispatchAllIn();
        }
        
    }
    
    bool predicate(GameObject gameObject) {
        return (gameObject.GetComponent<Damageable>() != null && gameObject.GetComponent<Damageable>().inhalable) || gameObject.GetComponent<Inhalable>() != null;
    }
    
    void OnDestroy() {
        foreach(GameObject gameObject in gathering) {
            gameObject.transform.localScale = originalLocalScales[gameObject];
        }
    }
    
    public void dispatchAllIn() {
        OnAllIn.Invoke();
    }
    
}
