using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaddleDeeScript : MonoBehaviour {
    
    public float speed = 128;
    
    float direction = 0;
    
    // Start is called before the first frame update
    void Start() {
        Kirby.current.addPositionDefinedListener(() => {
            direction = Mathf.Sign(Kirby.current.transform.position.x - transform.position.x);
        });
    }
    
    // Update is called once per frame
    void FixedUpdate() {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        
        if(rigidbody != null) {
            Vector2 velocity = rigidbody.velocity;
            
            velocity.x = direction * speed * Time.deltaTime;
            
            rigidbody.velocity = velocity;
        }
    }
    
}
