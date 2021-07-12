using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreVelocity : MonoBehaviour {
    
    new Rigidbody2D rigidbody;
    Vector2 saveVelocity;
    
    void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void OnEnable() {
        if(rigidbody == null) {
            rigidbody = GetComponent<Rigidbody2D>();
        }
        
        if(rigidbody != null) {
            rigidbody.velocity = saveVelocity;
        }
    }
    
    void Update() {
        if(rigidbody == null) {
            rigidbody = GetComponent<Rigidbody2D>();
        }
        
        if(rigidbody != null) {
            saveVelocity = rigidbody.velocity;
        }
    }
    
    void OnDisable() {
        
    }
    
}
