using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : DetectionBox {
    
    public int healValue = 1;
    
    public void Reset() {
        tagToDetect = "Player";
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        bool found = false;
        
        foreach(GameObject gameObject in getObjects()) {
            gameObject.GetComponent<Damageable>().heal(healValue);
            
            found = true;
        }
        
        if(found) {
            GameObject effect = Instantiate(Resources.Load<GameObject>("effects/RisingStar4"));
            effect.transform.SetParent(transform.parent);
            effect.transform.position = transform.position;
            effect.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1);
            effect.SetActive(true);
            
            Destroy(gameObject);
        }
    }
    
    public override bool predicate(GameObject gameObject) {
        return gameObject.GetComponent<Damageable>() != null && base.predicate(gameObject);
    }
    
}
