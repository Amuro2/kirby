using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AbilityIconFiller : MonoBehaviour {
    
    public string button;
    public string ability;
    
    public Image backgroundIcon;
    
    float maxHeight;
    RectTransform mask;
    Vector3 center;
    float offsetTop;
    
    static GameObject particlePrefab;
    float particleCountdown = 0;
    int c = 0;
    
    void Awake() {
        if(particlePrefab == null) {
            particlePrefab = Resources.Load<GameObject>("effects/AbilityParticleCanvas");
        }
    }
    
    void OnEnable() {
        string _ability = Controls.getAbilityForButton(button);
        
        if(_ability != null && _ability != "") {
            ability = _ability;
        }
        
        GetComponent<Image>().sprite = PersistentStuff.abilitiesIcons[ability];
        
        if(backgroundIcon != null) {
            backgroundIcon.sprite = PersistentStuff.abilitiesIcons[ability];
        }
    }
    
    // Start is called before the first frame update
    void Start() {
        maxHeight = GetComponent<RectTransform>().rect.height;
        mask = transform.parent.GetComponent<RectTransform>();
        center = transform.position;
        offsetTop = mask.offsetMax.y;
        
        // Debug.Log(maxHeight);
        // Debug.Log(mask.sizeDelta);
        // Debug.Log(mask.offsetMin + " -- " + mask.offsetMax);
    }
    
    // Update is called once per frame
    void Update() {
        
        /**
        
        Color color = gameObject.GetComponent<Image>().color;
        color[3] = (float)PersistentStuff.getAbilityFillRatio(ability);
        gameObject.GetComponent<Image>().color = color;
        
        /**/
        
        float ratio = (float)PersistentStuff.getAbilityFillRatio(ability);
        
        float height = ratio * maxHeight;
        
        // mask.position = center + new Vector3(0, maxHeight/2 - height/2, 0);
        // mask.sizeDelta = new Vector2(mask.sizeDelta.x, height);
        // mask.sizeDelta = new Vector2(0, height);
        mask.offsetMax = new Vector2(mask.offsetMax.x, mask.offsetMin.y + height);
        transform.position = center;
        // Debug.Log(ratio + " -- " +  height);
        
        if(ratio >= 1 && ability != "Inhale" && ability != "Jump") {
            if(particleCountdown <= 0) {
                particleCountdown = 250;
                
                GameObject particle = Instantiate(particlePrefab);
                
                // particle.GetComponent<SpriteRenderer>().color = PersistentStuff.getAbilityColorDim(ability);
                particle.GetComponent<InitialColorTransition>().colorStart = PersistentStuff.getAbilityColorDim(ability);
                particle.GetComponent<InitialColorTransition>().colorEnd = PersistentStuff.getAbilityColorDim(ability);
                particle.GetComponent<InitialColorTransition>().colorEnd[3] = 0;
                
                Vector3 position = transform.position;
                
                position.x += Mathf.Sin(c * Mathf.PI/3) * transform.localScale.x/2;
                position.y -= transform.localScale.y/2;
                position.z += Mathf.Cos(c * Mathf.PI/3) * 0.25f;
                
                particle.transform.position = position;
                
                particle.transform.localScale = new Vector3(0.0625f, 0.0625f, 0.0625f);
                
                particle.transform.SetParent(transform.parent.parent);
                ++c;
            }
        }
        
        particleCountdown -= Time.deltaTime * 1000;
        
    }
    
}
