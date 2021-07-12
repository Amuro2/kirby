using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class BossArea : DetectionBox {
    
    public string bossName = "Boss";
    
    public List<Damageable> bosses;
    public List<BoxCollider2D> walls;
    public AudioClip audioClip;
    public float audioLoopDuration;
    
    public bool shiftTransformDuringBattle;
    public Vector3 position2;
    public Vector3 scale2;
    
    public UnityEvent OnEnter = new UnityEvent();
    public UnityEvent OnVictory = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();
    
    bool battleStarted = false;
    bool battleEnded = false;
    
    static GameObject spawnEffectPrefab;
    
    double saveCurrentHealth;
    
    void Reset() {
        tagToDetect = "Player";
        position2 = transform.position;
        scale2 = transform.localScale;
    }
    
    void Awake() {
        if(spawnEffectPrefab == null) {
            spawnEffectPrefab = Resources.Load<GameObject>("effects/StarExplosion");
        }
    }
    
    // Start is called before the first frame update
    void Start() {
        
        foreach(Damageable damageable in bosses) {
            // damageable.enabled = false;
        }
        
    }
    
    // Update is called once per frame
    void Update() {
        if(!battleStarted && !battleEnded && detectsObjects()) {
            dispatchAreaEnter();
        }
        
        if(battleStarted && !battleEnded) {
            double currentHealth = 0;
            double maxHealth = 0;
            
            foreach(Damageable damageable in bosses) {
                currentHealth += damageable.getCurrentHealth();
                maxHealth += damageable.getMaxHealth();
            }
            
            if(saveCurrentHealth != currentHealth) {
                dispatchHealthChange();
            }
            
            saveCurrentHealth = currentHealth;
            
            WorldUI.current.setBossHealthRatio(currentHealth / maxHealth);
            
            if(currentHealth <= 0) {
                dispatchVictory();
            }
            
            else if(!detectsObjects()) {
                dispatchAreaExit();
            }
        }
    }
    
    void dispatchAreaEnter() {
        dispatchBattleStart();
    }
    
    void dispatchBattleStart() {
        
        if(shiftTransformDuringBattle) {
            Vector3 position = transform.position;
            Vector3 scale = transform.localScale;
            
            transform.position = position2;
            transform.localScale = scale2;
            
            position2 = position;
            scale2 = scale;
        }
        
        battleStarted = true;
        
        Flags.raiseFlag("inBattle");
        
        OnEnter.Invoke();
        
        foreach(BoxCollider2D wall in walls) {
            wall.enabled = true;
            CameraScript.current.walls.Add(wall.transform);
        }
        
        WorldUI.current.setBossActive(true);
        WorldUI.current.setBossName(bossName);
        
        foreach(Damageable damageable in bosses) {
            makeEffect(damageable.transform.position);
            damageable.gameObject.SetActive(true);
            // damageable.enabled = true;
        }
        
        if(audioClip != null) {
            AudioScript.current.setClip(audioClip);
            AudioScript.current.loopDuration = audioLoopDuration;
        }
        
    }
    
    void dispatchVictory() {
        
        battleEnded = true;
        
        OnVictory.Invoke();
        
        dispatchBattleEnd();
    }
    
    void dispatchAreaExit() {
        dispatchBattleEnd();
    }
    
    void dispatchBattleEnd() {
        
        if(shiftTransformDuringBattle) {
            Vector3 position = transform.position;
            Vector3 scale = transform.localScale;
            
            transform.position = position2;
            transform.localScale = scale2;
            
            position2 = position;
            scale2 = scale;
        }
        
        Flags.lowerFlag("inBattle");
        
        OnEnd.Invoke();
        
        foreach(BoxCollider2D wall in walls) {
            wall.enabled = false;
            CameraScript.current.walls.Remove(wall.transform);
        }
        
        WorldUI.current.setBossActive(false);
        
        battleStarted = false;
    }
    
    void dispatchHealthChange() {
        
    }
    
    public void makeEffect(Vector3 position) {
        if(spawnEffectPrefab != null) {
            GameObject effect = GameObject.Instantiate(spawnEffectPrefab);
            effect.transform.position = position;
            effect.SetActive(true);
        }
    }
    
}
