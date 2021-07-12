using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags : MonoBehaviour {
    
    public static HashSet<string> flags = new HashSet<string>();
    
    public List<Condition> activeConditions = new List<Condition>();
    
    [System.Serializable]
    public class Condition {
        public string flag;
        public bool up;
    }
    
    static HashSet<Flags> flagObjects = new HashSet<Flags>();
    
    void Awake() {
        flagObjects.Add(this);
    }
    
    // Start is called before the first frame update
    void Start() {
        updateActive();
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
    void OnDestroy() {
        flagObjects.Remove(this);
    }
    
    void updateActive() {
        bool res = true;
        
        foreach(Condition condition in activeConditions) {
            if(isFlagUp(condition.flag) != condition.up) {
                res = false;
                
                break;
            }
        }
        
        gameObject.SetActive(res);
    }
    
    static void updateActiveAll() {
        foreach(Flags flagObject in flagObjects) {
            if(flagObject.enabled) {
                flagObject.updateActive();
            }
        }
    }
    
    public static bool isFlagUp(string name) {
        return flags.Contains(name);
    }
    
    public static void setFlag(string name, bool up) {
        if(up) {
            flags.Add(name);
        } else {
            flags.Remove(name);
        }
        
        updateActiveAll();
    }
    
    public static void raiseFlag(string name) {
        setFlag(name, true);
    }
    
    public static void lowerFlag(string name) {
        setFlag(name, false);
    }
    
    public static void clearFlags() {
        flags.Clear();
        
        updateActiveAll();
    }
    
}
