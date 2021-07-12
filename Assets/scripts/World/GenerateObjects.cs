using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObjects : MonoBehaviour {
    
    public GameObject prefab;
    public int maxCount = 16;
    public float minZ;
    public float maxZ;
    public int eachLifespan = 1000;
    public int fadeDuration = 500;
    
    Dictionary<GameObject, int> timeouts = new Dictionary<GameObject, int>();
    
    // Start is called before the first frame update
    void Start() {
        if(prefab != null) {
            for(int i = 0; i < maxCount; ++i) {
                timeouts.Add(makeRandomObject((int)((float)eachLifespan / maxCount * i)), (int)((float)eachLifespan / maxCount * i));
            }
        }
    }
    
    // Update is called once per frame
    void Update() {
        if(prefab != null) {
            
            while(timeouts.Count < maxCount) {
                timeouts.Add(makeRandomObject(eachLifespan), eachLifespan);
            }
            
            List<GameObject> keys = new List<GameObject>(timeouts.Keys);
            
            foreach(GameObject key in keys) {
                if(timeouts[key] > 0) {
                    --timeouts[key];
                } else {
                    Destroy(key);
                    timeouts.Remove(key);
                }
            }
            
        }
    }
    
    GameObject makeRandomObject(int timeout) {
        GameObject res = Instantiate(prefab);
        
        res.transform.localScale = new Vector3(res.transform.localScale.x, res.transform.localScale.y, res.transform.localScale.z);
        
        res.transform.SetParent(transform);
        
        Vector3 localPosition = res.transform.localPosition;
        
        // float halfWidth = transform.localScale.x/2;
        float halfWidth = 0.5f;
        // float halfHeight = transform.localScale.y/2;
        float halfHeight = 0.5f;
        
        localPosition.x = -halfWidth + Random.value * (+halfWidth - -halfWidth);
        localPosition.y = -halfHeight + Random.value * (+halfHeight - -halfHeight);
        localPosition.z = minZ + Random.value * (maxZ - minZ);
        
        res.transform.localPosition = localPosition;
        
        Color color = res.GetComponent<SpriteRenderer>().color;
        
        if(timeout == eachLifespan) {
            res.GetComponent<InitialTimeouts>().addTimeout(0, () => {
                InitialColorTransition.colorStartInit = new Color(color[0], color[1], color[2], 0);
                InitialColorTransition.colorEndInit = color;
                
                InitialColorTransition ict = res.AddComponent<InitialColorTransition>();
                
                ict.msDuration = fadeDuration;
                ict.OnEnd.AddListener(() => {
                    ict.removeComponent();
                });
            });
            
            res.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
        
        res.GetComponent<InitialTimeouts>().addTimeout(timeout - fadeDuration, () => {
            InitialColorTransition.colorStartInit = color;
            InitialColorTransition.colorEndInit = new Color(color[0], color[1], color[2], 0);
            
            InitialColorTransition ict = res.AddComponent<InitialColorTransition>();
            
            ict.msDuration = fadeDuration;
            ict.OnEnd.AddListener(() => {
                Destroy(res);
            });
        });
        
        res.SetActive(true);
        
        return res;
    }
}
