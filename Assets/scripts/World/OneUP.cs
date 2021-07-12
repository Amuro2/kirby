using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUP : DetectionBox {
    
    public void Reset() {
        tagToDetect = "Player";
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(detectsObjects()) {
            Sound.play("1up");
            
            GameObject effect = Instantiate(Resources.Load<GameObject>("effects/RisingStar4"));
            effect.transform.SetParent(transform.parent);
            effect.transform.position = transform.position;
            effect.GetComponent<SpriteRenderer>().color = new Color(255, 0, 255, 1);
            effect.SetActive(true);
            
            PersistentStuff.increaseKirbies(1);
            Destroy(gameObject);
        }
    }
    
}
