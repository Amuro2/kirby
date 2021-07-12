using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    
    public static CameraScript current;
    
    public List<Transform> targets = new List<Transform>();
    public Transform cameraRoom;
    public List<Transform> walls;
    
    new Camera camera;
    float frustumWidth;
    float frustumHeight;
    
    float zStep;
    float zDuration;
    float sourceZ;
    float destinationZ;
    bool zTransitioning;
    
    Transform cursor;
    
    Vector3 lastPosition;
    
    void Awake() {
        current = this;
        
        camera = gameObject.GetComponent<Camera>();
        
        // camera.transparencySortAxis = new Vector3(0, 0, -1);
        camera.transparencySortMode = TransparencySortMode.Orthographic;
        
        centerOnTargets();
        
        // double verticalAngle = camera.fieldOfView * Mathf.Deg2Rad;
        // double horizontalAngle = Camera.VerticalToHorizontalFieldOfView(camera.fieldOfView, Screen.width / Screen.height) * Mathf.Deg2Rad;
        
        frustumHeight = 2.0f * Mathf.Abs(transform.position.z) * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * camera.aspect;
        
        // Debug.Log(frustumWidth + " -- " + frustumHeight);
        
        
        
        cursor = (new GameObject()).transform;
        
        cursor.gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("images/gordo");
        
        cursor.gameObject.SetActive(false);
        
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
    Vector3 oldNewPosition;
    
    void FixedUpdate() {
        
        lastPosition = transform.position;
        
        string str = "";
        
        if(zTransitioning) {
            float progress = zStep / zDuration;
            
            if(progress > 1) {
                progress = 1;
                zTransitioning = false;
            }
            
            Vector3 position = transform.position;
            
            // position.z = sourceZ + (destinationZ - sourceZ) * Mathf.Pow(progress, 1f/2);
            // position.z = sourceZ + (destinationZ - sourceZ) * Mathf.Pow(progress, 1);
            position.z = position.z + (destinationZ - position.z) * progress;
            
            transform.position = position;
            
            zStep += Time.deltaTime * 1000;
        }
        
        updateFrustumSize();
        
        Vector3 newPosition = transform.position;
        Vector3 unboundedDestination = transform.position;
        
        if(targets.Count > 0) {
            Vector3 direction = getTargetDirection();
            
            // transform.position += direction * 50 / 3 * Time.deltaTime;
            newPosition += direction;
            unboundedDestination += direction;
        }
        
        float finalFrustumWidth = frustumWidth;
        float finalFrustumHeight = frustumHeight;
        
        if(zTransitioning) {
            Vector2 frustumSize = calcFrustumSizeAt(0, destinationZ);
            
            finalFrustumWidth = frustumSize.x;
            finalFrustumHeight = frustumSize.y;
        }
        
        if(cameraRoom != null) {
            float frustumXM = transform.position.x;
            frustumXM = newPosition.x;
            float frustumYM = transform.position.y;
            frustumYM = newPosition.y;
            float frustumX1 = frustumXM - frustumWidth/2;
            float frustumX2 = frustumXM + frustumWidth/2;
            float frustumY1 = frustumYM - frustumHeight/2;
            float frustumY2 = frustumYM + frustumHeight/2;
            
            float roomWidth = cameraRoom.localScale.x;
            float roomHeight = cameraRoom.localScale.y;
            float roomXM = cameraRoom.position.x;
            float roomYM = cameraRoom.position.y;
            float roomX1 = roomXM - roomWidth/2;
            float roomX2 = roomXM + roomWidth/2;
            float roomY1 = roomYM - roomHeight/2;
            float roomY2 = roomYM + roomHeight/2;
            
            Vector3 avg = new Vector3(0, 0, 0);
            int xc = 0, yc = 0;
            
            if(frustumX1 < roomX1) {
                avg.x += roomX1 + frustumWidth/2;
                ++xc;
                
                str += "room left -- ";
            }
            
            if(frustumX2 > roomX2) {
                avg.x += roomX2 - frustumWidth/2;
                ++xc;
                
                str += "room right -- ";
            }
            
            if(frustumY1 < roomY1) {
                avg.y += roomY1 + frustumHeight/2;
                ++yc;
                
                str += "room down -- ";
            }
            
            if(frustumY2 > roomY2) {
                avg.y += roomY2 - frustumHeight/2;
                ++yc;
                
                str += "room up -- ";
            }
            
            Vector3 position = newPosition;
            
            if(xc != 0) {
                position.x = avg.x / xc;
                newPosition.x = avg.x / xc;
            }
            
            if(yc != 0) {
                position.y = avg.y / yc;
                newPosition.y = avg.y / yc;
            }
            
            transform.position = position;
        }
        
        // str = "";
        
        {
            float averageX = 0;
            float averageY = 0;
            float xCount = 0;
            float yCount = 0;
            
            foreach(Transform wall in walls) {
                if(wall.gameObject.activeInHierarchy) {
                    float frustumXM = transform.position.x;
                    frustumXM = newPosition.x;
                    float frustumYM = transform.position.y;
                    frustumYM = newPosition.y;
                    float frustumX1 = frustumXM - finalFrustumWidth/2;
                    float frustumX2 = frustumXM + finalFrustumWidth/2;
                    float frustumY1 = frustumYM - finalFrustumHeight/2;
                    float frustumY2 = frustumYM + finalFrustumHeight/2;
                    
                    float wallWidth = wall.localScale.x;
                    float wallHeight = wall.localScale.y;
                    float wallXM = wall.position.x;
                    float wallYM = wall.position.y;
                    float wallX1 = wallXM - wallWidth/2;
                    float wallX2 = wallXM + wallWidth/2;
                    float wallY1 = wallYM - wallHeight/2;
                    float wallY2 = wallYM + wallHeight/2;
                    
                    if(frustumX1 <= wallX2 && frustumX2 >= wallX1 && frustumY1 <= wallY2 && frustumY2 >= wallY1) {
                        
                        /**/
                        
                        float distX = Mathf.Abs(wallXM - frustumXM) - (wallWidth + finalFrustumWidth) / 2;
                        float distY = Mathf.Abs(wallYM - frustumYM) - (wallHeight + finalFrustumHeight) / 2;
                        
                        // Vector3 position = transform.position;
                        
                        if(distX > distY) {
                            if(frustumXM < wallXM) {
                                // position.x = wallX1 - frustumWidth/2;
                                averageX += wallX1 - finalFrustumWidth/2;
                                
                                str += "wall left -- ";
                            } else {
                                // position.x = wallX2 + frustumWidth/2;
                                averageX += wallX2 + finalFrustumWidth/2;
                                
                                str += "wall right -- ";
                            }
                            
                            ++xCount;
                        }
                        
                        else {
                            if(frustumYM < wallYM) {
                                // position.y = wallY1 - frustumHeight/2;
                                averageY += wallY2 - finalFrustumHeight/2;
                                
                                str += "wall down -- ";
                            } else {
                                // position.y = wallY1 - frustumHeight/2;
                                averageY += wallY1 + finalFrustumHeight/2;
                                
                                str += "wall up -- ";
                            }
                            
                            ++yCount;
                        }
                        
                        // transform.position = position;
                        
                        /**
                        
                        // Vertical
                        
                        if(lastPosition.x - frustumWidth/2 <= wallX2 && lastPosition.x + frustumWidth/2 >= wallX1) {
                            
                            // Up
                            
                            if(lastPosition.y > wallYM) {
                                averageY += wallY1 + finalFrustumHeight/2;
                                
                                str += "wall up -- ";
                            }
                            
                            // Down
                            
                            else {
                                averageY += wallY2 - finalFrustumHeight/2;
                                
                                str += "wall down -- ";
                            }
                            
                            ++yCount;
                            
                        }
                        
                        // Horizontal
                        
                        else {
                            
                            // Left
                            
                            if(lastPosition.x < wallXM) {
                                averageX += wallX1 - finalFrustumWidth/2;
                                
                                str += "wall left -- ";
                            }
                            
                            // Right
                            
                            else {
                                averageX += wallX2 + finalFrustumWidth/2;
                                
                                str += "wall right -- ";
                            }
                            
                            ++xCount;
                            
                        }
                        
                        /**/
                        
                    }
                }
            }
            
            Vector3 position = newPosition;
            
            if(xCount > 0) {
                averageX /= xCount;
                // position.x = position.x + (100 / 3 * Time.deltaTime) * (averageX - position.x);
                position.x = averageX;
                newPosition.x = averageX;
            }
            
            if(yCount > 0) {
                averageY /= yCount;
                // position.y = position.y + (100 / 3 * Time.deltaTime) * (averageY - position.y);
                position.y = averageY;
                newPosition.y = averageY;
            }
            
            transform.position = position;
            
        }
        
        transform.position = transform.position + (newPosition - transform.position) * 50 / 3 * Time.deltaTime;
        // transform.position = newPosition;
        
        if(!newPosition.Equals(oldNewPosition)) {
            
            if(str != "") {
                // Debug.Log(str);
            }
            // Debug.Log(newPosition);
            
        }
        
        oldNewPosition = newPosition;
        
        cursor.position = newPosition + new Vector3(0, 0, -newPosition.z);
        
    }
    
    Vector3 getAverageTargetPosition() {
        Vector3 position = new Vector3(0, 0, 0);
        
        foreach(Transform target in targets) {
            position += target.position;
        }
        
        position /= targets.Count;
        
        return position;
    }
    
    Vector3 getTargetDirection() {
        Vector3 position = getAverageTargetPosition();
        
        return (new Vector3(position.x, position.y, 0) - new Vector3(transform.position.x, transform.position.y, 0));
    }
    
    void OnDrawGizmosSelected() {
        
        camera = gameObject.GetComponent<Camera>();
        
        frustumHeight = 2.0f * Mathf.Abs(transform.position.z) * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * camera.aspect;
        
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(frustumWidth, frustumHeight, 0));
    }
    
    public Vector2 getFrustumSize() {
        return new Vector2(frustumWidth, frustumHeight);
    }
    
    Vector2 calcFrustumSizeAt(float z = 0) {
        float frustumHeight = 2.0f * Mathf.Abs(z - transform.position.z) * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * camera.aspect;
        
        return new Vector2(frustumWidth, frustumHeight);
    }
    
    Vector2 calcFrustumSizeAt(float z, float cameraZ) {
        float frustumHeight = 2.0f * Mathf.Abs(z - cameraZ) * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * camera.aspect;
        
        return new Vector2(frustumWidth, frustumHeight);
    }
    
    void updateFrustumSize() {
        Vector2 frustumSize = calcFrustumSizeAt();
        
        frustumWidth = frustumSize.x;
        frustumHeight = frustumSize.y;
    }
    
    public void centerOnTargets() {
        if(targets.Count > 0) {
            Vector3 position = getAverageTargetPosition();
            
            position.z = transform.position.z;
            
            transform.position = position;
        }
    }
    
    public void moveToZ(float z) {
        sourceZ = transform.position.z;
        destinationZ = z;
        zStep = 0;
        zDuration = 1000;
        zTransitioning = true;
        
        if(destinationZ < sourceZ) {
            zDuration = 2000;
        }
    }
    
    public void clearTargets() {
        // targets.Clear();
    }
    
    public void addTarget(Transform target) {
        // targets.Add(target);
    }
    
}
