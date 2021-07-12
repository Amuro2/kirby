using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;
using TMPro;

public class AbilitySplash : MonoBehaviour {
    
    public static AbilitySplash current;
    
    public string ability;
    
    public Text headerText;
    public Text text1;
    public Text text2;
    
    void OnEnable() {
        
        current = this;
        
        // 
        
        recursiveSetText(gameObject.GetComponent<RectTransform>());
        
        headerText.text = ability;
        text1.text = ability;
        text2.text = ability;
        
        Color lightColor = PersistentStuff.getAbilityColor(ability);
        Color dimColor = PersistentStuff.getAbilityColorDim(ability);
        
        headerText.color = dimColor;
        headerText.GetComponent<TextOutline>().color = lightColor;
        
        gameObject.GetComponent<RectTransform>().Find("Icon").gameObject.GetComponent<Image>().sprite = PersistentStuff.getAbilityIcon(ability);
        gameObject.GetComponent<RectTransform>().Find("Background").gameObject.GetComponent<Image>().color = lightColor;
        gameObject.GetComponent<RectTransform>().Find("Border1").gameObject.GetComponent<Image>().color = dimColor;
        gameObject.GetComponent<RectTransform>().Find("Border2").gameObject.GetComponent<Image>().color = dimColor;
        
        text1.color = lightColor + 0.25f * (dimColor - lightColor);
        
        text2.color = lightColor + 0.375f * (dimColor - lightColor);
        
        // 
        
        Sound.play("ability_get");
        
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    double progress = 0;
    bool ending;
    
    // Update is called once per frame
    void Update() {
        if(Input.GetButtonDown("Cancel") && !ending) {
            World.current.gameObject.SetActive(true);
            
            progress = 0;
            ending = true;
            
            Destroy(transform.Find("Camera").gameObject);
        }
    }
    
    void FixedUpdate() {
        if(!ending) {
            gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Pow((float)progress, 1f/2);
            
            progress += 0.0625;
            
            if(progress > 1) {
                progress = 1;
            }
        } else {
            gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Pow((float)(1 - progress), 1f/2);
            
            progress += 0.0625;
            
            if(progress > 1) {
                progress = 1;
                
                gameObject.SetActive(false);
                Destroy(gameObject);
                
                return;
            }
        }
    }
    
    void OnDestroy() {
        current = null;
    }
    
    void recursiveSetText(Transform transform) {
        for(int i = 0; i < transform.childCount; ++i) {
            Transform child = transform.GetChild(i);
            
            Text textComponent = child.gameObject.GetComponent<Text>();
            
            if(textComponent != null) {
                textComponent.text = ability;
            }
            
            TextMeshProUGUI tmpComponent = child.gameObject.GetComponent<TextMeshProUGUI>();
            
            if(tmpComponent != null) {
                tmpComponent.text = ability;
            }
            
            recursiveSetText(child);
        }
        
    }
    
}
