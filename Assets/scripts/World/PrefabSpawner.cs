using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
    public void spawn(GameObject prefab) {
        GameObject gameObject = Instantiate(prefab);
        gameObject.transform.position = transform.position;
        gameObject.SetActive(true);
    }
    
    public void spawn(string path) {
        spawn(Resources.Load<GameObject>(path));
    }
    
}
