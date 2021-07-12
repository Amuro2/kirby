using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Gauge : MonoBehaviour {
    
    public Color barColor = new Color(1, 0.9f, 0);
    public double ratio = 1;
    
    public double paddingTop = 0;
    public double paddingRight = 0;
    public double paddingBottom = 0;
    public double paddingLeft = 0;
    
    public double transitionSpeed = 1;
    public double transitionExponent = (double)1/2;
    
    double saveRatio = 0;
    double saveSaveRatio = 0;
    Color saveBarColor;
    
    RectTransform rectTransform;
    public GameObject bar;
    Image barImage;
    RectTransform barRectTransform;
    
    void Awake() {
        
        rectTransform = gameObject.GetComponent<RectTransform>();
        
        bar = (new GameObject());
        bar.name = "FillBar";
        bar.transform.SetParent(transform);
        bar.AddComponent<CanvasRenderer>();
        barImage = bar.AddComponent<Image>();
        
        barRectTransform = bar.GetComponent<RectTransform>();
        barRectTransform.anchorMin = new Vector2(0, 0);
        barRectTransform.anchorMax = new Vector2(1, 1);
        
        barRectTransform.localScale = new Vector3(1, 1, 1);
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
    void FixedUpdate() {
        if(saveSaveRatio != ratio || !barColor.Equals(saveBarColor)) {
            saveBarColor = barColor;
            
            refreshBar();
            saveSaveRatio = saveRatio;
            saveRatio = saveRatio + Mathf.Pow((float)transitionSpeed, (float)transitionExponent) * (ratio - saveRatio);
        }
    }
    
    void refreshBar() {
        
        if(barImage == null) {
            Debug.Log("Why the hell is barImage null?");
        } else {
        
        barImage.color = barColor;
        }
        
        
        float width = (float)(saveRatio * rectTransform.rect.width);
        
        // barRectTransform.sizeDelta = new Vector2(width, rectTransform.rect.height);
        // barRectTransform.anchoredPosition3D = new Vector3((float)(-rectTransform.rect.width/2 + ratio * (0 + rectTransform.rect.width/2)), 0, 0);
        
        
        
        // float innerRectX1 = rectTransform.rect.position.x - rectTransform.rect.width/2;
        // float innerRectY1 = rectTransform.rect.position.y - rectTransform.rect.height/2;
        // float innerRectX2 = rectTransform.rect.position.x + rectTransform.rect.width/2;
        // float innerRectY2 = rectTransform.rect.position.y + rectTransform.rect.height/2;
        // float innerRectXM = rectTransform.rect.position.x;
        // float innerRectYM = rectTransform.rect.position.y;
        float innerRectWidth = rectTransform.rect.width;
        // float innerRectHeight = rectTransform.rect.height;
        
        // innerRectX1 += (float)paddingLeft;
        // innerRectX2 -= (float)paddingRight;
        // innerRectY1 += (float)paddingBottom;
        // innerRectY2 -= (float)paddingTop;
        innerRectWidth -= (float)(paddingLeft + paddingRight);
        // innerRectHeight -= (float)(paddingTop + paddingBottom);
        
        width = (float)(saveRatio * innerRectWidth);
        
        barRectTransform.offsetMin = new Vector2((float)paddingLeft, (float)paddingBottom);
        barRectTransform.offsetMax = new Vector2((float)-(paddingRight + (innerRectWidth - width)), (float)-paddingTop);
        
        barRectTransform.rotation = transform.rotation;
        
    }
    
    public void setRatio(double ratio) {
        this.ratio = ratio;
        
        refreshBar();
    }
    
}
