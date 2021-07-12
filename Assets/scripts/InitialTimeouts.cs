using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class InitialTimeouts : MonoBehaviour {
    
    public List<Timeout> timeouts = new List<Timeout>();
    
    [System.Serializable]
    public class Timeout {
        public int msTime;
        public UnityEvent OnTimeout;
    }
    
    int step = 0;
    
    void FixedUpdate() {
        step += (int)(Time.deltaTime * 1000);
        
        List<Timeout> copy = timeouts;
        
        for(int i = 0; i < copy.Count; ++i) {
            Timeout timeout = timeouts[i];
            
            if(timeout.msTime <= step) {
                timeout.OnTimeout.Invoke();
                timeouts.RemoveAt(i);
            }
        }
    }
    
    public void destroy() {
        Destroy(gameObject);
    }
    
    public void deactivate() {
        gameObject.SetActive(false);
    }
    
    public void t0() { Debug.Log("Time 0"); }
    public void t1() { Debug.Log("Time 1"); }
    public void t2() { Debug.Log("Time 2"); }
    public void t3() { Debug.Log("Time 3"); }
    public void t4() { Debug.Log("Time 4"); }
    public void t5() { Debug.Log("Time 5"); }
    public void t6() { Debug.Log("Time 6"); }
    public void t7() { Debug.Log("Time 7"); }
    public void t8() { Debug.Log("Time 8"); }
    public void t9() { Debug.Log("Time 9"); }
    
    public void addTimeout(int msTime, UnityEvent OnTimeout) {
        Timeout timeout = new Timeout();
        timeout.msTime = msTime;
        timeout.OnTimeout = OnTimeout;
        timeouts.Add(timeout);
    }
    
    public void addTimeout(int msTime, UnityAction action) {
        UnityEvent OnTimeout = new UnityEvent();
        
        OnTimeout.AddListener(action);
        
        addTimeout(msTime, OnTimeout);
    }
    
    public static void setTimeout(UnityAction action, int msTime) {
        GameObject gameObject = new GameObject();
        
        InitialTimeouts initialTimeouts = gameObject.AddComponent<InitialTimeouts>();
        
        initialTimeouts.addTimeout(msTime, action);
        initialTimeouts.addTimeout(msTime, () => {
            GameObject.Destroy(gameObject);
        });
    }
    
}
