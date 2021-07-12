using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class GenericAction {
    
    public Transform user;
    public Animator animator;
    
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();
    public UnityEvent<Transform> OnHit = new UnityEvent<Transform>();
    
    public int step = 0;
    public int fstep = 0;
    
    protected bool stepBetween(int a, int b) {
        if(b < a) {
            return b <= step && step < a;
        }
        
        return a <= step && step < b;
    }
    
    protected float getUserFacingX() {
        return (float)getUserScript().getFacingX();
    }
    
    protected void setUserSpeedX(double speedX) {
        getUserScript().xSpeed = speedX;
    }
    
    protected void setUserSpeedY(double speedY) {
        getUserScript().ySpeed = speedY;
    }
    
    protected void setUserForceY(double forceY) {
        getUserScript().yForce = forceY;
    }
    
    protected void setUserStill(bool still) {
        getUserScript().setStill(still);
    }
    
    protected void freezeUserFacingX(bool freeze = true) {
        getUserScript().freezeFacingX(freeze);
    }
    
    protected bool isGrounded() {
        return getUserScript().isGrounded();
    }
    
    protected Kirby getUserScript() {
        return user.gameObject.GetComponent<Kirby>();
    }
    
    protected void airStall() {
        
        Vector2 velocity = user.gameObject.GetComponent<Rigidbody2D>().velocity;
        if(velocity.y < 0) { velocity.y = 0; }
        user.gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
        
    }
    
    public void dispatchStart() {
        OnStart.Invoke();
    }
    
    public void dispatchEnd() {
        OnEnd.Invoke();
    }
    
    public void dispatchHit(Transform target) {
        OnHit.Invoke(target);
    }
    
    public virtual bool isCancelableWith(GenericAction action) {
        return false;
    }
    
    public virtual void update() {
        
    }
    
    public virtual void fixedUpdate() {
        
    }
    
}
