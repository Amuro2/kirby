using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityFlip : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        
        if(rigidbody != null) {
            if(spriteRenderer != null) {
                if(rigidbody.velocity.x > 0) {
                    spriteRenderer.flipX = false;
                }
                
                else if(rigidbody.velocity.x < 0) {
                    spriteRenderer.flipX = true;
                }
                
            }
        }
    }
}
