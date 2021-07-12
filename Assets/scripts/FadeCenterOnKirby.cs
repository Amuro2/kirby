using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCenterOnKirby : MonoBehaviour {
    
    public static FadeCenterOnKirby current;
    
    Transform kirbyTransform;
    Transform cameraTransform;
    CameraScript cameraScript;
    RectTransform canvasTransform;
    
    // Start is called before the first frame update
    void Start() {
        
        current = this;
        
        // Debug.Log(GetComponent<RectTransform>().anchoredPosition);
        
        if(Kirby.current != null) {
            kirbyTransform = Kirby.current.transform;
        }
        
        if(CameraScript.current != null) {
            cameraScript = CameraScript.current;
            cameraTransform = cameraScript.transform;
        }
        
        if(transform.parent == null) {
            Debug.Log("Canvas transform is null");
        } else {
            canvasTransform = transform.parent.gameObject.GetComponent<RectTransform>();
        }
        
        centerOnKirby();
    }
    
    // Update is called once per frame
    void Update() {
        centerOnKirby();
    }
    
    void centerOnKirby() {
        if(kirbyTransform != null && cameraTransform != null && canvasTransform != null) {
            
            Vector2 vector = kirbyTransform.position - cameraTransform.position;
            Vector2 frustumSize = cameraScript.getFrustumSize();
            
            vector.x /= frustumSize.x/2;
            vector.y /= frustumSize.y/2;
            
            if(vector.x < -1) { vector.x = -1; }
            if(vector.x > +1) { vector.x = +1; }
            if(vector.y < -1) { vector.y = -1; }
            if(vector.y > +1) { vector.y = +1; }
            
            vector.x *= canvasTransform.rect.width/2;
            vector.y *= canvasTransform.rect.height/2;
            
            GetComponent<RectTransform>().anchoredPosition = vector;
            
        }
    }
    
    void OnDestroy() {
        current = null;
    }
    
}
