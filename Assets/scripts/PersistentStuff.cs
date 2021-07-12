using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersistentStuff {
    
    public static string roomName = "";
    public static Vector3 roomPosition;
    static bool roomPositionDefined;
    static bool startFlipX;
    
    static int kirbies = 13;
    static int pointStars = 0;
    
    public static bool[] abilities = new bool[4]{
        true, true, true, true
        // false, false, false, false
    };
    
    public static int ABI_PAINT = 0;
    public static int ABI_MIKE = 1;
    public static int ABI_COOK = 2;
    public static int ABI_MAGIC = 3;
    
    public static Dictionary<string, double> abilitiesSpecials = defaultAbilitiesSpecials();
    
    static float _5f = 0.37254901960784315f;
    static float _7f = 0.4980392156862745f;
    static float bf = 0.7490196078431373f;
    static float df = 0.8745098039215686f;
    static float ef = 0.9372549019607843f;
    static float ff = 1;
    
    public static Dictionary<string, Color> abilitiesColors = getAbilitiesColors();
    public static Dictionary<string, Color> abilitiesColorsDim = getAbilitiesColorsDim();
    
    public static Dictionary<string, Sprite> abilitiesIcons = getAbilitiesIcons();
    
    static string lastLevel;
    
    public static Dictionary<string, Color> levelsColors = getLevelsColors();
    
    static Dictionary<string, double> defaultAbilitiesSpecials() {
        Dictionary<string, double> map = new Dictionary<string, double>();
        
        map.Add("Inhale", 1);
        map.Add("Jump", 1);
        map.Add("Paint", 1);
        map.Add("Mike", 1);
        map.Add("Cook", 1);
        map.Add("Magic", 1);
        
        return map;
    }
    
    static Dictionary<string, Color> getAbilitiesColors() {
        Dictionary<string, Color> map = new Dictionary<string, Color>();
        
        map.Add("Kirby", new Color(ff, df, ff));
        map.Add("Inhale", new Color(ff, df, ff));
        map.Add("Jump", new Color(df, ff, ff));
        map.Add("Paint", new Color(ff, df, df));
        map.Add("Mike", new Color(ef, df, ff));
        map.Add("Cook", new Color(ff, ef, df));
        map.Add("Magic", new Color(df, df, df));
        
        return map;
    }
    
    static Dictionary<string, Color> getAbilitiesColorsDim() {
        Dictionary<string, Color> map = new Dictionary<string, Color>();
        
        map.Add("Kirby", new Color(ff, _5f, ff));
        map.Add("Inhale", new Color(ff, _5f, ff));
        map.Add("Jump", new Color(0, bf, ff));
        map.Add("Paint", new Color(bf, 0, 0));
        map.Add("Mike", new Color(bf, _7f, ff));
        map.Add("Cook", new Color(ff, df, 0));
        map.Add("Magic", new Color(0, 0, 0));
        
        return map;
    }
    
    static Dictionary<string, Sprite> getAbilitiesIcons() {
        Dictionary<string, Sprite> map = new Dictionary<string, Sprite>();
        
        map.Add("Kirby", Resources.Load<Sprite>("images/icons/kirby"));
        map.Add("Inhale", Resources.Load<Sprite>("images/icons/kirby"));
        map.Add("Jump", Resources.Load<Sprite>("images/icons/jump"));
        map.Add("Paint", Resources.Load<Sprite>("images/icons/paint"));
        map.Add("Mike", Resources.Load<Sprite>("images/icons/mike"));
        map.Add("Cook", Resources.Load<Sprite>("images/icons/cook"));
        map.Add("Magic", Resources.Load<Sprite>("images/icons/magic"));
        
        return map;
    }
    
    static Dictionary<string, Color> getLevelsColors() {
        Dictionary<string, Color> map = new Dictionary<string, Color>();
        
        map.Add("Greenhouse", new Color(1, 1, 0, 1));
        map.Add("Aquarium", new Color(0, 0.75f, 1, 1));
        map.Add("Tower", new Color(0.5f, 1, 0.5f, 1));
        map.Add("Dungeon", new Color(0.75f, 0, 1, 1));
        map.Add("Roof", new Color(0.5f, 0, 0, 1));
        map.Add("End", new Color(0, 1, 1, 1));
        
        return map;
    }
    
    static Dictionary<string, string> getRoomsLevels() {
        Dictionary<string, string> map = new Dictionary<string, string>();
        
        map.Add("Greenhouse1", "Greenhouse");
        map.Add("Greenhouse2", "Greenhouse");
        map.Add("Greenhouse3", "Greenhouse");
        map.Add("GreenhouseBoss", "Greenhouse");
        map.Add("Aquarium1", "Aquarium");
        map.Add("Aquarium2", "Aquarium");
        map.Add("Aquarium3", "Aquarium");
        map.Add("AquariumA", "Aquarium");
        map.Add("AquariumBoss", "Aquarium");
        map.Add("Tower1", "Tower");
        map.Add("Tower2", "Tower");
        map.Add("Tower3", "Tower");
        map.Add("Tower4", "Tower");
        map.Add("Tower5", "Tower");
        map.Add("Tower6", "Tower");
        map.Add("TowerA", "Tower");
        map.Add("TowerBoss", "Tower");
        map.Add("Dungeon1", "Dungeon");
        map.Add("Dungeon2", "Dungeon");
        map.Add("Dungeon3", "Dungeon");
        map.Add("Dungeon4", "Dungeon");
        map.Add("Dungeon5", "Dungeon");
        map.Add("Dungeon6", "Dungeon");
        map.Add("Dungeon7", "Dungeon");
        map.Add("Dungeon8", "Dungeon");
        map.Add("Dungeon9", "Dungeon");
        map.Add("Dungeon10", "Dungeon");
        map.Add("Dungeon11", "Dungeon");
        map.Add("Dungeon12", "Dungeon");
        map.Add("DungeonA", "Dungeon");
        map.Add("DungeonBoss", "Dungeon");
        map.Add("RoofHub", "Roof");
        map.Add("Roof1", "Roof");
        map.Add("Roof2", "Roof");
        map.Add("Roof3", "Roof");
        map.Add("Roof4", "Roof");
        map.Add("RoofBoss", "Roof");
        map.Add("Final", "End");
        
        return map;
    }
    
    public static string getRoomName() { return roomName; }
    
    public static void setRoomName(string pRoomName) {
        roomName = pRoomName;
    }
    
    public static Vector3 getRoomPosition() { return roomPosition; }
    
    public static void setRoomPosition(Vector3 pRoomPosition) {
        roomPosition = pRoomPosition;
        roomPositionDefined = true;
    }
    
    public static bool isRoomPositionDefined() { return roomPositionDefined; }
    
    public static bool[] getAbilities() { return abilities; }
    
    public static void setAbilities(bool[] pAbilities) {
        abilities = pAbilities;
    }
    
    public static void setAllAbilities(bool unlocked) {
        abilities = new bool[]{unlocked, unlocked, unlocked, unlocked};
    }
    
    public static void unlockAbility(int index) {
        abilities[index] = true;
    }
    
    public static void unlockAbility(string ability) {
        abilities[getAbilityIndex(ability)] = true;
    }
    
    public static int getAbilityIndex(string ability) {
        switch(ability) {
            case "Paint": { return 0; }
            case "Mike": { return 1; }
            case "Cook": { return 2; }
            case "Magic": { return 3; }
        }
        
        return -1;
    }
    
    public static bool abilityUnlocked(string ability) {
        int index = getAbilityIndex(ability);
        
        if(index == -1) {
            return true;
        }
        
        return abilities[index];
    }
    
    public static void loadRoom() {
        SceneManager.LoadScene(roomName);
    }
    
    public static void fillAbilityGauge(string ability, double value) {
        abilitiesSpecials[ability] += value;
        
        if(abilitiesSpecials[ability] > 1) {
            abilitiesSpecials[ability] = 1;
        }
    }
    
    public static void emptyAbilityGauge(string ability) {
        abilitiesSpecials[ability] = 0;
    }
    
    public static double getAbilityFillRatio(string ability) {
        return abilitiesSpecials[ability];
    }
    
    public static bool isAbilityFull(string ability) {
        return getAbilityFillRatio(ability) >= 1;
    }
    
    public static Color fadeColor = new Color(1, 0.5f, 1, 1);
    
    public static void transitionToRoom(string nextRoom, Vector3 nextPosition, bool nextFlipX) {
        startFlipX = nextFlipX;
        fadeColor = getLevelColor(getRoomLevel(nextRoom));
        
        lastLevel = getRoomLevel();
        
        GameObject fadeOutCanvas = GameObject.Instantiate(Resources.Load<GameObject>("FadeOutCanvas"));
        
        GameObject fadeOut = fadeOutCanvas.transform.Find("FadeOut").gameObject;
        
        fadeOut.GetComponent<Image>().color = fadeColor;
        
        // fadeOut.GetComponent<InitialColorTransition>().colorStart = fadeColor;
        // fadeOut.GetComponent<InitialColorTransition>().colorStart[3] = 0;
        // fadeOut.GetComponent<InitialColorTransition>().colorEnd = fadeColor;
        
        // fadeOut.GetComponent<InitialColorTransition>().OnEnd.AddListener(() => {
            // setRoomName(nextRoom);
            // setRoomPosition(nextPosition);
            // loadRoom();
        // });
        
        fadeOut.GetComponent<InitialTimeouts>().timeouts[0].OnTimeout.AddListener(() => {
            setRoomName(nextRoom);
            setRoomPosition(nextPosition);
            loadRoom();
        });
        
        fadeOutCanvas.SetActive(true);
        
    }
    
    public static void fadeIn() {
        GameObject fadeInCanvas = GameObject.Instantiate(Resources.Load<GameObject>("FadeInCanvas"));
        
        GameObject fadeIn = fadeInCanvas.transform.Find("FadeIn").gameObject;
        
        fadeIn.GetComponent<Image>().color = fadeColor;
        
        // fadeIn.GetComponent<InitialColorTransition>().colorStart = fadeColor;
        // fadeIn.GetComponent<InitialColorTransition>().colorEnd = fadeColor;
        // fadeIn.GetComponent<InitialColorTransition>().colorEnd[3] = 0;
        
        fadeInCanvas.SetActive(true);
    }
    
    public static int getKirbiesCount() {
        return kirbies;
    }
    
    public static int getPointStarsCount() {
        return pointStars;
    }
    
    public static void increaseKirbies(int amount = 1) {
        kirbies += amount;
        
        if(kirbies > 99) {
            kirbies = 99;
        }
    }
    
    public static void decreaseKirbies(int amount = 1) {
        kirbies -= amount;
        
        if(kirbies < 0) {
            kirbies = 0;
        }
    }
    
    public static void increasePointStars(int amount = 1) {
        pointStars += amount;
        increaseKirbies(pointStars / 20);
        pointStars = pointStars % 20;
    }
    
    public static bool newLevel() {
        return lastLevel != getRoomLevel();
    }
    
    public static string getRoomLevel(string room) {
        Dictionary<string, string> map = getRoomsLevels();
        
        if(map.ContainsKey(room)) {
            return map[room];
        }
        
        return room;
    }
    
    public static Color getLevelColor(string level) {
        Dictionary<string, Color> map = getLevelsColors();
        
        if(map.ContainsKey(level)) {
            Color color = levelsColors[level];
            
            // color[3] = 0;
            
            return color;
        }
        
        return new Color(1, 0.5f, 1, 1);
    }
    
    public static string getRoomLevel() {
        return getRoomLevel(roomName);
    }
    
    public static Color getLevelColor() {
        return getLevelColor(getRoomLevel());
    }
    
    public static Color getAbilityColor(string ability) {
        Dictionary<string, Color> map = getAbilitiesColors();
        
        if(map.ContainsKey(ability)) {
            return map[ability];
        }
        
        return Color.white;
    }
    
    public static Color getAbilityColorDim(string ability) {
        Dictionary<string, Color> map = getAbilitiesColorsDim();
        
        if(map.ContainsKey(ability)) {
            return map[ability];
        }
        
        return Color.white;
    }
    
    public static Sprite getAbilityIcon(string ability) {
        Dictionary<string, Sprite> map = getAbilitiesIcons();
        
        if(map.ContainsKey(ability)) {
            return map[ability];
        }
        
        return map["Kirby"];
    }
    
    public static bool flipXOnStart() {
        return startFlipX;
    }
    
}
