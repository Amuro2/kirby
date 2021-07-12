using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
    class CheckEnd : MonoBehaviour {
        
        public AudioSource source;
        public AudioClip clip;
        
        void Update() {
            if(source.time >= clip.length) {
                Destroy(gameObject);
            }
        }
        
    }
    
    public static GameObject play(string audioName) {
        AudioClip clip = Resources.Load<AudioClip>("audio/" + audioName);
        
        GameObject gameObject = new GameObject();
        gameObject.name = "Sound";
        
        AudioSource source = gameObject.AddComponent<AudioSource>();
        
        source.clip = clip;
        
        CheckEnd component = gameObject.AddComponent<CheckEnd>();
        
        component.source = source;
        component.clip = clip;
        
        source.Play();
        
        return gameObject;
    }
    
}
