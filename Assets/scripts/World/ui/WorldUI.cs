using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class WorldUI : MonoBehaviour {
    
    public static WorldUI current;
    
    Text kirbiesCounter;
    Text pointStarsCounter;
    
    Gauge kirbyHealthBar;
    
    GameObject bossStuff;
    Gauge bossBar;
    Text bossName;
    
    bool bossActive;
    
    void Awake() {
        current = this;
        kirbiesCounter = transform.Find("Kirbies").gameObject.GetComponent<Text>();
        pointStarsCounter = transform.Find("PointStars").gameObject.GetComponent<Text>();
        kirbyHealthBar = transform.Find("HealthBar/Gauge").gameObject.GetComponent<Gauge>();
        
        transform.Find("Level/Panel").gameObject.GetComponent<Image>().color = PersistentStuff.getLevelColor();
        transform.Find("Level/LevelName").gameObject.GetComponent<Text>().text = "The " + PersistentStuff.getRoomLevel();
        transform.Find("Level/LevelName").gameObject.GetComponent<TextOutline>().color = PersistentStuff.getLevelColor();
        
        if(PersistentStuff.getRoomLevel() == "Greenhouse") {
            transform.Find("Level/LevelName").gameObject.GetComponent<Text>().color = Color.black;
        } else {
            transform.Find("Level/LevelName").gameObject.GetComponent<Text>().color = Color.white;
        }
        
        transform.Find("Level").gameObject.SetActive(PersistentStuff.newLevel());
        
        bossStuff = transform.Find("BossStuff").gameObject;
        bossBar = transform.Find("BossStuff/BossBar/Gauge").gameObject.GetComponent<Gauge>();
        bossName = transform.Find("BossStuff/BossName").gameObject.GetComponent<Text>();
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(true) {
            kirbiesCounter.text = PersistentStuff.getKirbiesCount() + "";
        }
        
        if(true) {
            pointStarsCounter.text = PersistentStuff.getPointStarsCount() + "";
        }
    }
    
    public void setKirbyHealthRatio(double ratio) {
        if(ratio < kirbyHealthBar.ratio) {
            InitialColorTransition ct = kirbyHealthBar.bar.AddComponent<InitialColorTransition>();
            
            ct.colorStart = new Color(0.75f, 0, 0, 1);
            ct.colorEnd = kirbyHealthBar.barColor;
            ct.exponent = 1f/2;
            
            ct.OnEnd.AddListener(ct.removeComponent);
        }
        
        kirbyHealthBar.setRatio(ratio);
    }
    
    public void setDebug(string text) {
        transform.Find("Debug").gameObject.GetComponent<Text>().text = text;
    }
    
    public void setBossHealthRatio(double ratio) {
        bossBar.setRatio(ratio);
    }
    
    public void setBossName(string name) {
        bossName.text = name;
    }
    
    public void setBossActive(bool active) {
        bossActive = active;
        bossStuff.SetActive(active);
    }
    
}
