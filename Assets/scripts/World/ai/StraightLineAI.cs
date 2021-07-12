using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLineAI : MonoBehaviour {
    
    public float speed = 60;
    public float direction = 0;
    public bool headingPlayer = true;
    
    // Start is called before the first frame update
    void Start() {
        
        if(headingPlayer) {
            direction = Kirby.current.transform.position.x - transform.position.x;
        }
        
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
    void FixedUpdate() {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        
        if(rigidbody != null) {
            Vector2 velocity = rigidbody.velocity;
            
            velocity.x = direction;
            
            if(rigidbody.gravityScale == 0) {
                velocity = velocity.normalized * speed * Time.deltaTime;
            } else {
                velocity.x = Mathf.Sign(velocity.x) * speed * Time.deltaTime;
            }
            
            rigidbody.velocity = velocity;
        }
    }
    
}
