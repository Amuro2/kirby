using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOrder : MonoBehaviour {
    
    public string debug;
    public Color color;
    
    // Start is called before the first frame update
    void Start() {
        Debug.Log(debug);
    }
    
    // Update is called once per frame
    void Update() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(spriteRenderer != null) {
            spriteRenderer.color = color;
        }
    }
    
}
