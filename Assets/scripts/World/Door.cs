using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Door : MonoBehaviour {
    
    public string nextRoom;
    public Vector3 nextPosition;
    public bool nextFlipX = false;
    public bool upDoor;
    
    public UnityEvent OnEnter = new UnityEvent();
    
    public static bool moving;
    
    float c;
    float duration = 1000;
    bool thisMoving;
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(!moving) {
            Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position - transform.localScale/2, transform.position + transform.localScale/2);
            
            foreach(Collider2D collider in colliders) {
                if(collider.tag == "Player" && (!upDoor || Controls.getVerticalAxis() > 0)) {
                    goToNextRoom();
                    moving = true;
                    thisMoving = true;
                    
                    break;
                }
            }
        }
        
        if(upDoor && thisMoving && moving) {
            
            float progress = c / duration;
            
            if(progress > 1) {
                progress = 1;
            }
            
            Transform kirby = Kirby.current.transform;
            
            kirby.GetComponent<Rigidbody2D>().gravityScale = 0;
            
            kirby.position = kirby.position + (transform.position - kirby.position) * progress;
            
            c += Time.deltaTime * 1000;
        }
    }
    
    void goToNextRoom() {
        OnEnter.Invoke();
        World.current.transitionToRoom(nextRoom, nextPosition, nextFlipX);
    }
    
}
