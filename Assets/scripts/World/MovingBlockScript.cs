using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlockScript : MonoBehaviour {
    
    public Vector3 destination;
    public float moveTime = 2000;
    public float restTime = 1000;
    
    Vector3 source;
    float movingC = 0;
    float restingC = 0;
    bool back = true;
    
    Vector3 lastPosition;
    Vector3 direction;
    
    void Awake() {
        source = transform.position;
        lastPosition = transform.position;
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        Vector2 thisSize = transform.localScale;
        
        if(GetComponent<BoxCollider2D>().autoTiling) {
            thisSize = GetComponent<SpriteRenderer>().size;
        }
        
        foreach(Transform t in transforms) {
            
            float thisDistX = Mathf.Abs(t.position.x - transform.position.x) - thisSize.x/2 - t.localScale.x/2;
            float thisDistY = Mathf.Abs(t.position.y - transform.position.y) - thisSize.y/2 - t.localScale.y/2;
            
            bool thisCollidesInX = thisDistX > thisDistY;
            
            Vector3 position = t.position;
            Vector3 size = t.localScale;
            
            Collider2D[] colliders = Physics2D.OverlapAreaAll(position - size/2, position + size/2, LayerMask.GetMask("Ground"));
            
            foreach(Collider2D collider in colliders) {
                if(collider.gameObject != gameObject) {
                    
                    Vector2 colliderSize = collider.transform.localScale;
                    
                    float distX = Mathf.Abs(t.position.x - collider.transform.position.x) - colliderSize.x/2 - t.localScale.x/2;
                    float distY = Mathf.Abs(t.position.y - collider.transform.position.y) - colliderSize.y/2 - t.localScale.y/2;
                    
                    bool collidesInX = distX > distY;
                    bool crush = false;
                    
                    if(thisCollidesInX && collidesInX) {
                        if(Mathf.Sign(t.position.x - transform.position.x) != Mathf.Sign(t.position.x - collider.transform.position.x)) {
                            crush = true;
                        }
                    }
                    
                    else if(!thisCollidesInX && !collidesInX) {
                        if(Mathf.Sign(t.position.y - transform.position.y) != Mathf.Sign(t.position.y - collider.transform.position.y)) {
                            crush = true;
                        }
                    }
                    
                    if(crush) {
                        
                    }
                    
                }
            }
        }
    }
    
    void FixedUpdate() {
        
        lastPosition = transform.position;
        
        if(restingC == 0) {
            
            // List<Transform> filtered = getFilteredTransforms();
            
            // transform.position = source + (destination - source) * Mathf.Pow(Mathf.Sin(c/time * Mathf.PI), 3);
            
            float progress = (moveTime - movingC) / moveTime;
            if(progress > 1) { progress = 1; }
            progress = progress * 2 - 1;
            progress = Mathf.Sign(progress) * Mathf.Pow(Mathf.Abs(progress), 1f/1f);
            progress = (progress + 1) / 2;
            
            if(back) {
                transform.position = destination + (source - destination) * progress;
            } else {
                transform.position = source + (destination - source) * progress;
            }
            
            // foreach(Transform t in filtered) {
                // Vector3 position = t.position;
                
                // position.y = transform.position.y + transform.localScale.y/2 + t.localScale.y/2;
                
                // t.position = position;
            // }
            
            movingC -= Time.deltaTime * 1000;
            
            if(movingC <= 0) {
                movingC = 0;
                restingC = restTime;
            }
        }
        
        else {
            
            if(back) {
                transform.position = source;
            } else {
                transform.position = destination;
            }
            
            restingC -= Time.deltaTime * 1000;
            
            if(restingC <= 0) {
                restingC = 0;
                movingC = moveTime;
                back = !back;
            }
        }
        
        direction = transform.position - lastPosition;
        
    }
    
    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawLine(transform.position, destination);
        
        if(GetComponent<BoxCollider2D>().autoTiling) {
            Gizmos.DrawWireCube(destination, GetComponent<SpriteRenderer>().size);
        } else {
            Gizmos.DrawWireCube(destination, transform.localScale);
        }
    }
    
    List<Transform> transforms = new List<Transform>();
    
    List<Transform> getFilteredTransforms() {
        List<Transform> filtered = new List<Transform>();
        
        foreach(Transform t in transforms) {
            Rigidbody2D rigidbody = t.GetComponent<Rigidbody2D>();
            
            if(rigidbody != null && rigidbody.gravityScale > 0) {
                float tX1 = t.position.x - t.localScale.x/2;
                float tX2 = t.position.x + t.localScale.x/2;
                float tY1 = t.position.y - t.localScale.y/2;
                float tY2 = t.position.y - t.localScale.y;
                float oX1 = transform.position.x - transform.localScale.x/2;
                float oX2 = transform.position.x + transform.localScale.x/2;
                float oY1 = transform.position.y + transform.localScale.y/2;
                float oY2 = transform.position.y - transform.localScale.y/2;
                
                if(tX1 <= oX2 && tX2 >= oX1 && tY1 >= oY2 && tY2 <= oY1) {
                    filtered.Add(t);
                }
                
            }
            
        }
        
        return filtered;
    }
    
    Dictionary<Transform, Transform> saveParents = new Dictionary<Transform, Transform>();
    
    void OnCollisionEnter2D(Collision2D collision) {
        transforms.Add(collision.transform);
        
        
        Rigidbody2D rigidbody = collision.transform.GetComponent<Rigidbody2D>();
        
        if(rigidbody != null && rigidbody.gravityScale > 0) {
            Vector2 size = transform.localScale;
            
            if(GetComponent<BoxCollider2D>().autoTiling) {
                size = GetComponent<SpriteRenderer>().size;
            }
            
            if(locatesDown(collision.transform.position, collision.transform.localScale, transform.position, size)) {
                saveParents[collision.transform] = collision.transform.parent;
                collision.transform.SetParent(transform);
            }
        }
        
    }
    
    void OnCollisionExit2D(Collision2D collision) {
        transforms.Remove(collision.transform);
        
        if(saveParents.ContainsKey(collision.transform)) {
            collision.transform.SetParent(saveParents[collision.transform]);
            saveParents.Remove(collision.transform);
        }
    }
    
    bool locatesDown(Vector2 tPositionM, Vector2 tSize, Vector2 uPositionM, Vector2 uSize) {
        float tXM = tPositionM.x;
        float tYM = tPositionM.y;
        float tWidth = tSize.x;
        float tHeight = tSize.y;
        float uXM = uPositionM.x;
        float uYM = uPositionM.y;
        float uWidth = uSize.x;
        float uHeight = uSize.y;
        
        float distX = Mathf.Abs(tXM - uXM) - uWidth/2 - tWidth/2;
        float distY = Mathf.Abs(tYM - uYM) - uHeight/2 - tHeight/2;
        
        return distY > distX && tYM > uYM;
    }
    
}
