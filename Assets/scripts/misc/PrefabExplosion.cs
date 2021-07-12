using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PrefabExplosion {
    
    int count = 8;
    GameObject prefab;
    Vector3 center;
    double startAngle = 0;
    double endAngle = 2*Mathf.PI;
    double normMin = 1;
    double normMax = 1;
    double angleVariationMin = 0;
    double angleVariationMax = 0;
    double initialDistanceMin = 0;
    double initialDistanceMax = 0;
    List<GameObject> objects = null;
    List<Action<GameObject>> initListeners = new List<Action<GameObject>>();
    
    
    public PrefabExplosion setCount(int _count) { count = _count; return this; }
    
    
    
    public PrefabExplosion setPrefab(GameObject _prefab) { prefab = _prefab; return this; }
    public PrefabExplosion setPrefab(string prefabResourcePath) { return setPrefab(Resources.Load<GameObject>(prefabResourcePath)); }
    
    
    
    public PrefabExplosion setCenter(Vector3 _center) { center = _center; return this; }
    
    
    
    public PrefabExplosion setLaunchNorm(double _normMin, double _normMax) { normMin = _normMin; normMax = _normMax; return this; }
    public PrefabExplosion setLaunchNorm(double norm) { return setLaunchNorm(norm, norm); }
    
    
    
    public PrefabExplosion setAngleStartEnd(double _startAngle, double _endAngle) { startAngle = _startAngle; endAngle = _endAngle; return this; }
    public PrefabExplosion setInitialAngle(double initialAngle) { return setAngleStartEnd(initialAngle, initialAngle + 2*Mathf.PI); }
    public PrefabExplosion setDirection(Vector2 direction) { setLaunchNorm(direction.magnitude); endAngle = startAngle = Vector2.SignedAngle(new Vector2(1, 0), direction) * Mathf.Deg2Rad; return this; }
    public PrefabExplosion setAngleStartEndRelativeToDirection(Vector2 direction, double _startAngle, double _endAngle) { setLaunchNorm(direction.magnitude); float angle = Vector2.SignedAngle(new Vector2(1, 0), direction) * Mathf.Deg2Rad; startAngle = angle + _startAngle; endAngle = angle + _endAngle; return this; }
    public PrefabExplosion setAngleStartEndRelativeToDirection(Vector2 direction, double _angle) { return setAngleStartEndRelativeToDirection(direction, -Mathf.Abs((float)_angle), +Mathf.Abs((float)_angle)); }
    
    
    
    public PrefabExplosion setAngleVariation(double _angleVariationMin, double _angleVariationMax) { angleVariationMin = _angleVariationMin; angleVariationMax = _angleVariationMax; return this; }
    public PrefabExplosion setAngleVariation(double angleVariation) { return setAngleVariation(-(double)Mathf.Abs((float)angleVariation), (double)Mathf.Abs((float)angleVariation)); }
    public PrefabExplosion setRelativeAngleVariation(double relativeAngleVariationMin, double relativeAngleVariationMax) { setAngleVariation(relativeAngleVariationMin / count * 2*Mathf.PI, relativeAngleVariationMax / count * 2*Mathf.PI); return this; }
    public PrefabExplosion setRelativeAngleVariation(double relativeAngleVariation) { return setRelativeAngleVariation(-(double)Mathf.Abs((float)relativeAngleVariation), +(double)Mathf.Abs((float)relativeAngleVariation)); }
    
    
    
    public PrefabExplosion setInitialDistance(double _initialDistanceMin, double _initialDistanceMax) { initialDistanceMin = _initialDistanceMin; initialDistanceMax = _initialDistanceMax; return this; }
    public PrefabExplosion setInitialDistance(double initialDistance) { return setInitialDistance(initialDistance, initialDistance); }
    
    public PrefabExplosion clearInitListeners() { initListeners.Clear(); return this; }
    public PrefabExplosion addInitListener(Action<GameObject> listener) { initListeners.Add(listener); return this; }
    
    double random(double min, double max) { return min + UnityEngine.Random.value * (max - min); }
    
    public PrefabExplosion start() {
        objects = new List<GameObject>();
        
        for(int i = 0; i < count; ++i) {
            double angleVariation = random(angleVariationMin, angleVariationMax);
            double angle = startAngle + (double)i/count * (endAngle - startAngle) + angleVariation;
            
            double norm = random(normMin, normMax);
            Vector2 direction = new Vector2(Mathf.Cos((float)angle), Mathf.Sin((float)angle)) * (float)norm;
            
            GameObject gameObject = GameObject.Instantiate(prefab);
            
            gameObject.transform.position = center + (Vector3)direction.normalized * (float)random(initialDistanceMin, initialDistanceMax);
            
            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
            
            if(rigidbody == null) {
                rigidbody = gameObject.AddComponent<Rigidbody2D>();
                rigidbody.gravityScale = 0;
            }
            
            if(rigidbody != null) {
                rigidbody.velocity = direction;
            }
            
            foreach(Action<GameObject> action in initListeners) {
                action(gameObject);
            }
            
            gameObject.SetActive(true);
            
            objects.Add(gameObject);
        }
        
        return this;
    }
    
    public List<GameObject> getObjects() {
        return objects;
    }
    
}
