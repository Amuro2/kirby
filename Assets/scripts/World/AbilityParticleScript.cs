using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityParticleScript : MonoBehaviour {
    
    int c;
    Vector2 speed;
    
    // Start is called before the first frame update
    void Start() {
        c = (int)(Random.value * 128);
    }
    
    // Update is called once per frame
    void Update() {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        
        if(rigidbody != null) {
            Vector2 velocity = rigidbody.velocity;
            
            velocity.x = Mathf.Sin(c * Mathf.PI/128) * 2;
            
            rigidbody.velocity = velocity;
        }
        
        else {
            speed.x = Mathf.Sin(c * Mathf.PI/128) * 0.125f;
            speed.y += 0.03125f/32;
            
            transform.position += (Vector3)speed;
        }
        
        ++c;
    }
    
}
