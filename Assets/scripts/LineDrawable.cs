using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawable {
    
    public Queue<Vector3> points = new Queue<Vector3>();
    public GameObject gameObject;
    public LineRenderer lineRenderer;
    
    static Shader shader = null;
    
    bool shouldDestroyWhenEmpty;
    
    public LineDrawable() {
        if(shader == null) {
            
            GameObject dummy = new GameObject();
            
            SpriteRenderer spriteRenderer = dummy.AddComponent<SpriteRenderer>();
            
            shader = spriteRenderer.material.shader;
            
            GameObject.Destroy(dummy);
            
        }
        
        gameObject = new GameObject();
        gameObject.name = "LineDrawable";
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material.shader = shader;
        lineRenderer.useWorldSpace = true;
    }
    
    public void updateLineRenderer() {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
    
    public LineDrawable addPoint(Vector3 point) {
        points.Enqueue(point);
        
        updateLineRenderer();
        
        return this;
    }
    
    public LineDrawable addPointToDequeue(Vector3 point, int timeout) {
        points.Enqueue(point);
        
        updateLineRenderer();
        
        GameObject framesTimeout = Timeout.setFrames(() => {
            points.Dequeue();
            
            updateLineRenderer();
            
            if(shouldDestroyWhenEmpty && points.Count == 0) {
                GameObject.Destroy(gameObject);
            }
            
        }, timeout);
        
        framesTimeout.transform.SetParent(gameObject.transform);
        
        return this;
    }
    
    public LineDrawable setColor(Color color) {
        
        
        return this;
    }
    
    public void destroyWhenEmpty() {
        shouldDestroyWhenEmpty = true;
    }
    
}
