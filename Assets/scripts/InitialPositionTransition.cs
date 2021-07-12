using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class InitialPositionTransition : MonoBehaviour {
    
    public List<Vector3> positionStops = new List<Vector3>();
    public float msDuration = 1000;
    public float exponent = 1;
    public float msStartup = 0;
    
    public UnityEvent OnEnd = new UnityEvent();
    
    float step = 0;
    bool shouldEnd;
    bool ended;
    
    void Reset() {
        positionStops.Add(transform.position);
        positionStops.Add(transform.position);
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(shouldEnd && !ended) {
            ended = true;
            OnEnd.Invoke();
            
            return;
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
        
        transform.position = getPositionAt(progress);
    }
    
    float getProgress() {
        return (step - msStartup) / msDuration;
    }
    
    Vector3 getPositionAt(float progress) {
        progress = Mathf.Pow(progress, exponent);
        
        for(int i = 0; i < positionStops.Count-1; ++i) {
            float progress0 = (float)i/(positionStops.Count-1);
            float progress1 = (float)(i+1)/(positionStops.Count-1);
            
            if(progress0 <= progress && progress <= progress1) {
                progress = (progress - progress0) / (progress1 - progress0);
                
                Vector3 position0 = positionStops[i];
                Vector3 position1 = positionStops[i+1];
                
                return position0 + (position1 - position0) * progress;
            }
        }
        
        return positionStops[positionStops.Count-1];
    }
    
    public void setPositionStart(Vector3 positionStart) {
        if(positionStops.Count != 2) {
            positionStops.Clear();
            positionStops.Add(positionStart);
            positionStops.Add(positionStart);
        }
        
        positionStops[0] = positionStart;
    }
    
    public void setPositionEnd(Vector3 positionEnd) {
        if(positionStops.Count != 2) {
            positionStops.Clear();
            positionStops.Add(positionEnd);
            positionStops.Add(positionEnd);
        }
        
        positionStops[positionStops.Count-1] = positionEnd;
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
