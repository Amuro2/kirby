using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionBox : Detector {
    
    public Vector2 offset = new Vector2(0, 0);
    public Vector2 size = new Vector2(1, 1);
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(0.875f, 0.75f, 1);
        // Gizmos.DrawWireCube(transform.position, transform.localScale);
        Gizmos.DrawWireCube(transform.position + (Vector3)offset, new Vector3(transform.localScale.x * size.x, transform.localScale.y * size.y, transform.localScale.z));
    }
    
    public override HashSet<GameObject> getObjects() {
        Vector3 position = transform.position + (Vector3)offset;
        Vector3 scale = new Vector3(transform.localScale.x * size.x, transform.localScale.y * size.y, transform.localScale.z);
        
        Collider2D[] colliders = Physics2D.OverlapAreaAll(position - scale/2, position + scale/2, layerMask);
        
        HashSet<GameObject> filtered = new HashSet<GameObject>();
        
        foreach(Collider2D collider in colliders) {
            if(predicate(collider.gameObject)) {
                filtered.Add(collider.gameObject);
            }
        }
        
        return filtered;
    }
    
}
