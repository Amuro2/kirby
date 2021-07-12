using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {
    
    public string tagToDetect;
    public LayerMask layerMask = Physics2D.DefaultRaycastLayers;
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
    public virtual bool predicate(GameObject gameObject) {
        return (tagToDetect == null || tagToDetect == "" || tagToDetect == gameObject.tag);
    }
    
    public virtual HashSet<GameObject> getObjects() {
        return new HashSet<GameObject>();
    }
    
    public bool detectsObjects() {
        return getObjects().Count > 0;
    }
    
    public bool hasObjects() {
        return getObjects().Count > 0;
    }
    
    public GameObject getRandomObject() {
        HashSet<GameObject> gameObjects = getObjects();
        
        if(gameObjects.Count > 0) {
            int index = (int)(Random.value * gameObjects.Count);
            int i = 0;
            
            foreach(GameObject gameObject in gameObjects) {
                if(i == index) {
                    return gameObject;
                }
                
                ++i;
            }
        }
        
        return null;
    }
    
}
