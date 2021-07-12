using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : GenericAction {
    
    public Vector2 direction;
    float speed = 10;
    
    VelocityRotate rotateComponent;
    
    GameObject linePrefab = Resources.Load<GameObject>("effects/TorpedoLine");
    LineDrawable lineDrawable1;
    LineDrawable lineDrawable2;
    LineDrawable lineDrawable3;
    
    public Torpedo() {
        OnStart.AddListener(() => {
            setUserStill(true);
            
            animator.SetTrigger("startTorpedo");
            
            // 
            
            rotateComponent = user.gameObject.AddComponent<VelocityRotate>();
            rotateComponent.leftFlip = true;
            
            lineDrawable1 = new LineDrawable();
            lineDrawable1.lineRenderer.startWidth = 0.125f;
            lineDrawable1.lineRenderer.endWidth = 0.125f;
            lineDrawable1.lineRenderer.startColor = new Color(0.5f, 1, 1, 0);
            lineDrawable1.lineRenderer.endColor = new Color(1, 1, 1, 1);
            lineDrawable1.gameObject.transform.SetParent(user.parent);
            lineDrawable1.destroyWhenEmpty();
            
            lineDrawable2 = new LineDrawable();
            lineDrawable2.lineRenderer.startWidth = 0.125f;
            lineDrawable2.lineRenderer.endWidth = 0.125f;
            lineDrawable2.lineRenderer.startColor = new Color(0.5f, 1, 1, 0);
            lineDrawable2.lineRenderer.endColor = new Color(1, 1, 1, 1);
            lineDrawable2.gameObject.transform.SetParent(user.parent);
            lineDrawable2.destroyWhenEmpty();
            
            lineDrawable3 = new LineDrawable();
            lineDrawable3.lineRenderer.startWidth = 1 - (0.125f + 0.125f);
            lineDrawable3.lineRenderer.endWidth = 1 - (0.125f + 0.125f);
            lineDrawable3.lineRenderer.startColor = new Color(0, 1, 1, 0);
            lineDrawable3.lineRenderer.endColor = new Color(0, 1, 1, 1);
            lineDrawable3.gameObject.transform.SetParent(user.parent);
            lineDrawable3.destroyWhenEmpty();
            
        });
        
        OnEnd.AddListener(() => {
            GameObject.Destroy(rotateComponent);
            
            // 
            
            setUserStill(false);
        });
    }
    
    public override void update() {
        
        float norm = direction.magnitude;
        
        if(norm > 0) {
            setUserSpeedX(direction.x * speed / norm);
            setUserSpeedY(direction.y * speed / norm);
        }
        
        ////  ////
        
        float angle = Vector2.SignedAngle(new Vector2(1, 0), user.GetComponent<Rigidbody2D>().velocity) * Mathf.Deg2Rad;
        
        // Line 1
        
        Vector3 position;
        
        angle -= Mathf.PI/2;
        
        position = user.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (0.5f - 0.125f) + new Vector3(0, 0, 1f/4096);
        
        lineDrawable1.addPointToDequeue(position, 48);
        
        // Line 2
        
        angle += Mathf.PI;
        
        position = user.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (0.5f - 0.125f) + new Vector3(0, 0, 1f/4096);
        
        lineDrawable2.addPointToDequeue(position, 48);
        
        // Line 3
        
        lineDrawable3.addPointToDequeue(user.position + new Vector3(0, 0, 1f/4096), 32);
        
    }
    
    float c = 0;
    
    public override void fixedUpdate() {
        if(c >= 125) {
            c = 0;
            
            // GameObject line = GameObject.Instantiate(linePrefab);
            
            // line.GetComponent<Rigidbody2D>().velocity = -user.GetComponent<Rigidbody2D>().velocity.normalized * 2;
            
            // float angle = Random.value * 2*Mathf.PI;
            
            // line.transform.position = user.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 1;
            // line.transform.SetParent(user.parent);
            
        }
        
        c += Time.deltaTime * 1000;
    }
    
}
