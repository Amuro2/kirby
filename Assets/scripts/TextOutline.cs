using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TextOutline : MonoBehaviour {
    
    public float size = 1;
    public Color color = Color.black;
    
    RectTransform rectTransform;
    Text text;
    string oldText;
    RectTransform outlineSet;
    
    void Awake() {
        rectTransform = gameObject.GetComponent<RectTransform>();
        text = gameObject.GetComponent<Text>();
        
        outlineSet = (new GameObject()).AddComponent<RectTransform>();
        outlineSet.SetParent(transform.parent);
        outlineSet.SetSiblingIndex(transform.GetSiblingIndex());
        outlineSet.sizeDelta = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        outlineSet.anchoredPosition = new Vector2(0, 0);
        outlineSet.gameObject.name = "OutlineSet";
        
        for(int i = 0; i < 8; ++i) {
            GameObject outline = new GameObject();
            outline.name = "Outline (" + i + ")";
            outline.AddComponent<Text>();
            outline.transform.SetParent(outlineSet);
        }
        
        outlineSet.localScale = new Vector3(1, 1, 1);
        
        refreshText();
        
    }
    
    void OnEnable() {
        refreshText();
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(oldText != text.text) {
            refreshText();
            
            oldText = text.text;
        }
    }
    
    public void refreshText() {
        outlineSet.position = transform.position;
        outlineSet.rotation = transform.rotation;
        
        for(int i = 0; i < outlineSet.childCount; ++i) {
            GameObject outline = outlineSet.GetChild(i).gameObject;
            
            Text text = outline.GetComponent<Text>();
            
            text.text = gameObject.GetComponent<Text>().text;
            text.color = color;
            text.font = gameObject.GetComponent<Text>().font;
            text.fontSize = gameObject.GetComponent<Text>().fontSize;
            text.fontStyle = gameObject.GetComponent<Text>().fontStyle;
            text.alignment = gameObject.GetComponent<Text>().alignment;
            
            
            
            float angle = (float)i/8 * 2*Mathf.PI;
            
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * size;
            
            RectTransform rectTransform = outline.GetComponent<RectTransform>();
            
            Vector2 position = gameObject.GetComponent<RectTransform>().anchoredPosition;
            
            position += direction;
            
            // rectTransform.rotation = transform.rotation;
            rectTransform.sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().rect.width, gameObject.GetComponent<RectTransform>().rect.height);
            // rectTransform.anchoredPosition = position;
            // rectTransform.SetPositionAndRotation(position, transform.rotation);
            
            rectTransform.anchoredPosition = direction;
            
        }
    }
    
}
