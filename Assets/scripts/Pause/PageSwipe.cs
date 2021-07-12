using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using System;

public class PageSwipe : MonoBehaviour {
    
    public UnityEvent OnSwipeEnd = new UnityEvent();
    
    double step = 9999;
    int duration = 250;
    int firstAlpha = 0;
    int lastAlpha = 1;
    bool ended = true;
    float exponent;
    
    Vector3 originPosition;
    Vector3 firstPosition;
    Vector3 lastPosition;
    
    void Awake() {
        originPosition = transform.position;
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(!ended) {
            float progress = ((float)step / duration);
            
            if(progress > 1) {
                progress = 1;
                ended = true;
            }
            
            GetComponent<CanvasGroup>().alpha = firstAlpha + Mathf.Pow(progress, exponent) * (lastAlpha - firstAlpha);
            transform.position = firstPosition + (lastPosition - firstPosition) * progress;
            
            step += Time.deltaTime * 1000;
            
            if(ended) {
                if(GetComponent<CanvasGroup>().alpha == 0) {
                    gameObject.SetActive(false);
                }
                
                OnSwipeEnd.Invoke();
            }
        }
    }
    
    public void swipeOn(Vector3 direction) {
        gameObject.SetActive(true);
        
        step = 0;
        
        firstAlpha = 0;
        lastAlpha = 1;
        firstPosition = originPosition - direction;
        lastPosition = originPosition;
        exponent = 4;
        
        ended = false;
    }
    
    public void swipeOff(Vector3 direction) {
        gameObject.SetActive(true);
        
        step = 0;
        
        firstAlpha = 1;
        lastAlpha = 0;
        firstPosition = originPosition;
        lastPosition = originPosition + direction;
        exponent = 1f/4;
        
        ended = false;
    }
    
}
