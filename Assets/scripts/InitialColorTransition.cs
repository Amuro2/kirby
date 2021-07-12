using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;

public class InitialColorTransition : MonoBehaviour {
    
    public static Color colorStartInit = new Color(1, 1, 1, 0);
    public static Color colorEndInit = new Color(1, 1, 1, 1);
    
    public Color colorStart = colorStartInit;
    public Color colorEnd = colorEndInit;
    public int msDuration = 1000;
    public double exponent = 1;
    public int msStartup = 0;
    public bool resetOnEnable = true;
    
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();
    
    int step = 0;
    bool started, ended;
    
    Color getColorAt(double progress) {
        return colorStart + (colorEnd - colorStart) * Mathf.Pow((float)progress, (float)exponent);
    }
    
    void setColor(Color color) {
        Image image = GetComponent<Image>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(image != null) {
            image.color = color;
        }
        
        if(spriteRenderer != null) {
            spriteRenderer.color = color;
        }
    }
    
    void Awake() {
        if(msStartup == 0) {
            setColor(getColorAt(0));
        }
    }
    
    void OnEnable() {
        if(resetOnEnable) {
            step = 0;
            started = false;
            ended = false;
            
            if(msStartup == 0) {
                setColor(getColorAt(0));
            }
        }
    }
    
    void FixedUpdate() {
        step += (int)(Time.deltaTime * 1000);
        
        double progress = getProgress();
        
        if(!started && progress >= 0) {
            started = true;
            
            OnStart.Invoke();
        }
        
        if(progress < 0) { progress = 0; }
        if(progress > 1) {
            progress = 1;
            
            if(!ended) {
                ended = true;
                
                OnEnd.Invoke();
            }
        }
        
        Color currentColor = getColorAt(progress);
        
        setColor(currentColor);
    }
    
    public float getProgress() {
        return (float)(step - msStartup)/msDuration;
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
