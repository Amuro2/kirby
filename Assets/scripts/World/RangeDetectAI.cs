using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetectAI : Detector {
    
    public Vector2 offset = new Vector2(0, 0);
    public double range;
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(0.875f, 0.75f, 1);
        Gizmos.DrawWireSphere(transform.position + (Vector3)offset, (float)range);
    }
    
    public override HashSet<GameObject> getObjects() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + (Vector3)offset, (float)range, layerMask);
        
        HashSet<GameObject> filtered = new HashSet<GameObject>();
        
        foreach(Collider2D collider in colliders) {
            if(predicate(collider.gameObject)) {
                filtered.Add(collider.gameObject);
            }
        }
        
        return filtered;
    }
    
}
