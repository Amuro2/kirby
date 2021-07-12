using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Timeout {
    
    class MsTimeout : MonoBehaviour {
        
        public double ms;
        public Action action;
        
        void Update() {
            if(ms <= 0) {
                action();
                Destroy(gameObject);
            }
            
            ms -= Time.deltaTime * 1000;
        }
        
    }
    
    class FramesTimeout : MonoBehaviour {
        
        public int frames;
        public Action action;
        
        void Update() {
            if(frames <= 0) {
                action();
                Destroy(gameObject);
            }
            
            --frames;
        }
        
    }
    
    class FixedTimeout : MonoBehaviour {
        
        public int frames;
        public Action action;
        
        void FixedUpdate() {
            if(frames <= 0) {
                action();
                Destroy(gameObject);
            }
            
            --frames;
        }
        
    }
    
    public static GameObject setMs(Action action, double ms) {
        if(ms == 0) {
            action();
            return null;
        }
        
        GameObject gameObject = new GameObject();
        gameObject.name = "MsTimeout";
        
        MsTimeout timeout = gameObject.AddComponent<MsTimeout>();
        timeout.ms = ms;
        timeout.action = action;
        
        return gameObject;
    }
    
    public static GameObject setFrames(Action action, int frames) {
        if(frames == 0) {
            action();
            return null;
        }
        
        GameObject gameObject = new GameObject();
        gameObject.name = "FramesTimeout";
        
        FramesTimeout timeout = gameObject.AddComponent<FramesTimeout>();
        timeout.frames = frames;
        timeout.action = action;
        
        return gameObject;
    }
    
    public static GameObject setFixed(Action action, int frames) {
        if(frames == 0) {
            action();
            return null;
        }
        
        GameObject gameObject = new GameObject();
        gameObject.name = "FixedTimeout";
        
        FixedTimeout timeout = gameObject.AddComponent<FixedTimeout>();
        timeout.frames = frames;
        timeout.action = action;
        
        return gameObject;
    }
    
}
