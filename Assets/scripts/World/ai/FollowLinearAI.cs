using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLinearAI : MonoBehaviour {
    
    public float speed = 128;
    Vector2 direction;
    
    void Awake() {
        
    }
    
    void Update() {
        direction = new Vector2(0, 0);
        
        if(GetComponent<Detector>() != null) {
            foreach(GameObject gameObject in GetComponent<Detector>().getObjects()) {
                direction = gameObject.transform.position - transform.position;
            }
        }
        
    }
    
    void FixedUpdate() {
        
        if(direction.magnitude > 0) {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            
            if(rigidbody != null) {
                rigidbody.velocity = direction.normalized * speed * Time.deltaTime;
            }
        }
        
    }
    
}
