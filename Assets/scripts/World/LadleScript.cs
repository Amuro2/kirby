using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadleScript : MonoBehaviour {
    
    public float minWidth = 12.6f;
    public float maxWidth = 32;
    public float ratio = 0;
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(spriteRenderer != null) {
            spriteRenderer.size = new Vector2(minWidth + ratio * (maxWidth - minWidth), spriteRenderer.size.y);
        }
    }
    
    public void setRatio(float ratio) {
        this.ratio = ratio;
    }
    
}
