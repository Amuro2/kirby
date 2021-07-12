using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {
    
    public static AudioScript current;
    
    public AudioClip clip;
    public float loopDuration = 12.883f;
    // public float loopDuration = 42.682f;
    public bool loop = true;
    
    AudioSource source;
    
    bool volumeTransitioning;
    float volumeStart;
    float volumeEnd;
    float volumeTransitionStep;
    float volumeTransitionDuration;
    
    void Awake() {
        
        source = GetComponent<AudioSource>();
        
        if(source == null) {
            source = gameObject.AddComponent<AudioSource>();
        }
        
        if(clip != null) {
            source.clip = clip;
        }
        
        if(current == null) {
            current = this;
        } else {
            if(current.source.clip != source.clip) {
                Destroy(current.gameObject);
                current = this;
            } else {
                Destroy(gameObject);
                
                return;
            }
        }
        
        transform.SetParent(null);
        
        DontDestroyOnLoad(gameObject);
        
    }
    
    // Start is called before the first frame update
    void Start() {
        source.Play();
    }
    
    // Update is called once per frame
    void Update() {
        
        if(source.clip != null && source.time >= source.clip.length - 1 && loop) {
            // Debug.Log(source.time);
            source.time -= loopDuration;
            source.Stop();
            source.Play();
        }
        
        if(volumeTransitioning) {
            float progress = volumeTransitionStep / volumeTransitionDuration;
            
            setVolume(volumeStart + (volumeEnd - volumeStart) * progress);
            
            volumeTransitionStep += Time.deltaTime * 1000;
            
            if(volumeTransitionStep >= volumeTransitionDuration) {
                volumeTransitioning = false;
            }
            
        }
        
    }
    
    public void setClip(AudioClip clip) {
        this.clip = clip;
        source.clip = clip;
        source.Play();
    }
    
    public float getVolume() {
        return source.volume;
    }
    
    public void setVolume(float volume) {
        source.volume = volume;
    }
    
    public void setVolumeTransition(float volumeEnd, float msDuration) {
        volumeTransitioning = true;
        volumeStart = getVolume();
        this.volumeEnd = volumeEnd;
        volumeTransitionStep = 0;
        volumeTransitionDuration = msDuration;
    }
    
}
