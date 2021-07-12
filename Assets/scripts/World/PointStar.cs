using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointStar : DetectionBox {
    
    public void Reset() {
        tagToDetect = "Player";
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(detectsObjects()) {
            GameObject effect = Instantiate(Resources.Load<GameObject>("effects/RisingStar4"));
            effect.transform.SetParent(transform.parent);
            effect.transform.position = transform.position;
            effect.GetComponent<SpriteRenderer>().color = new Color(255, 255, 0, 1);
            effect.SetActive(true);
            
            PersistentStuff.increasePointStars(1);
            Destroy(gameObject);
        }
    }
    
}
