using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityRotate : MonoBehaviour {
    
    public float initialAngle = 0;
    public bool leftFlip;
    
    Quaternion saveRotation;
    Vector3 saveLocalScale;
    
    void Awake() {
        
        saveRotation = transform.rotation;
        saveLocalScale = transform.localScale;
        
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        
        if(rigidbody != null) {
            Vector2 velocity = rigidbody.velocity;
            
            // Update flip x
            
            if(leftFlip) {
                if(velocity.x < 0) {
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    
                    if(spriteRenderer != null) {
                        spriteRenderer.flipX = true;
                    } else {
                        Vector3 localScale = transform.localScale;
                        localScale.x = -Mathf.Abs(localScale.x);
                        transform.localScale = localScale;
                    }
                    
                }
                
                else if(velocity.x > 0) {
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    
                    if(spriteRenderer != null) {
                        spriteRenderer.flipX = false;
                    } else {
                        Vector3 localScale = transform.localScale;
                        localScale.x = +Mathf.Abs(localScale.x);
                        transform.localScale = localScale;
                    }
                }
            }
            
            // Rotate
            
            float norm = velocity.magnitude;
            
            if(norm > 0) {
                // float cos = velocity.x / norm;
                // float sin = velocity.y / norm;
                
                // float angleDeg = Mathf.Atan2(sin, cos) * Mathf.Rad2Deg;
                
                float angleDeg = Vector2.SignedAngle(new Vector2(1, 0), velocity) - initialAngle;
                
                if(leftFlip && (velocity.x < 0 || (velocity.x == 0 && getFacingX() < 0))) {
                    angleDeg -= 180;
                    transform.rotation = Quaternion.Euler(0, 0, angleDeg);
                }
                
                else {
                    transform.rotation = Quaternion.Euler(0, 0, angleDeg);
                }
            }
        }
    }
    
    void OnDestroy() {
        
        transform.localScale = saveLocalScale;
        transform.rotation = saveRotation;
        
    }
    
    int getFacingX() {
        int facingX = +1;
        
        if(transform.localScale.x < 0) {
            facingX *= -1;
        }
        
        if(GetComponent<SpriteRenderer>() != null && GetComponent<SpriteRenderer>().flipX) {
            facingX *= -1;
        }
        
        return facingX;
    }
    
}
