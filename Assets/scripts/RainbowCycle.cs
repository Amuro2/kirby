using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowCycle : MonoBehaviour {
    
    public double cycleDuration = 1000;
    public double step;
    public float direction = +1;
    
    static Color[] colors = new Color[]{
        new Color(1, 0, 0),
        new Color(1, 0.5f, 0),
        new Color(1, 1, 0),
        new Color(0.5f, 1, 0),
        new Color(0, 1, 0),
        new Color(0, 1, 0.5f),
        new Color(0, 1, 1),
        new Color(0, 0.5f, 1),
        new Color(0, 0, 1),
        new Color(0.5f, 0, 1),
        new Color(1, 0, 1),
        new Color(1, 0, 0.5f),
        new Color(1, 0, 0)
    };
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(spriteRenderer != null) {
            Color color = getColorAt(step / cycleDuration);
            
            color[3] = spriteRenderer.color[3];
            
            spriteRenderer.color = color;
        }
        
        step += (double)Mathf.Sign(direction) * Time.deltaTime * 1000;
        
        if(step < 0 || step >= cycleDuration) {
            step = ((step % cycleDuration) + cycleDuration) % cycleDuration;
        }
        
    }
    
    Color getColorAt(double progress) {
        if(progress < 0) { progress = 0; }
        if(progress > 1) { progress = 1; }
        
        for(int i = 0; i < colors.Length - 1; ++i) {
            Color color0 = colors[i];
            Color color1 = colors[i+1];
            
            double progress0 = (double)i/(colors.Length-1);
            double progress1 = (double)(i+1)/(colors.Length-1);
            
            if(progress0 <= progress && progress <= progress1) {
                progress = (progress - progress0) / (progress1 - progress0);
                
                return color0 + (color1 - color0) * (float)progress;
            }
        }
        
        return Color.black;
    }
    
}
