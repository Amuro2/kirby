using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class ContactVanish : MonoBehaviour {
    
    public Vector2 size = new Vector2(1, 1);
    
    public List<GameObject> blackList = new List<GameObject>();
    public UnityEvent OnVanish = new UnityEvent();
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        Vector3 scale = new Vector3(transform.localScale.x * size.x, transform.localScale.y * size.y, transform.localScale.z);
        
        Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position - scale/2, transform.position + scale/2, LayerMask.GetMask("Ground"));
        List<Collider2D> filtered = new List<Collider2D>();
        
        foreach(Collider2D collider in colliders) {
            if(!blackList.Contains(collider.gameObject) && collider.tag != "Enemy") {
                filtered.Add(collider);
            }
        }
        
        if(filtered.Count > 0) {
            OnVanish.Invoke();
            Destroy(gameObject);
        }
    }
    
    public void makeEffectOnSelf(string effectName) {
        
        GameObject prefab = Resources.Load<GameObject>("effects/" + effectName);
        
        if(prefab != null) {
            GameObject effect = GameObject.Instantiate(prefab);
            effect.transform.position = transform.position;
            effect.SetActive(true);
        }
        
    }
    
}
