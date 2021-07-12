using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class InitialScaleTransition : MonoBehaviour {
    
    public Vector3 scaleStart = new Vector3(1, 1, 1);
    public Vector3 scaleEnd = new Vector3(0, 0, 0);
    public float msDuration = 1000;
    public float exponent = 1;
    public float msStartup = 0;
    
    public UnityEvent OnEnd = new UnityEvent();
    
    float step = 0;
    
    bool ended;
    bool shouldEnd;
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(shouldEnd && !ended) {
            ended = true;
            OnEnd.Invoke();
        }
        
        step += Time.deltaTime * 1000;
        
        float progress = getProgress();
        
        if(progress < 0) {
            progress = 0;
        }
        
        if(progress > 1) {
            progress = 1;
            shouldEnd = true;
        }
        
        progress = Mathf.Pow(progress, exponent);
        
        transform.localScale = getScaleAt(progress);
    }
    
    Vector3 getScaleAt(float progress) {
        return scaleStart + (scaleEnd - scaleStart) * progress;
    }
    
    public float getProgress() {
        return (step - msStartup) / msDuration;
    }
    
    public void destroy() {
        Destroy(gameObject);
    }
    
    public void deactivate() {
        gameObject.SetActive(false);
    }
    
    public void removeComponent() {
        Destroy(this);
    }
    
}
