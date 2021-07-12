using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        foreach(GameObject gameObject in getColliders()) {
            // gameObject.GetComponent<Kirby>().setUnderwater(true);
        }
    }
    
    bool predicateCollider(GameObject gameObject) {
        return gameObject.GetComponent<Kirby>() != null && gameObject.transform.position.y < transform.position.y + transform.localScale.y/2;
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
    
}
