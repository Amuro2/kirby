
using System.Collections.Generic;
using UnityEngine;

/**
 * ------------
 * Key: single keyboard key, controller button, …
 * Button: set of keys grouped together.
 * Ability: set of buttons grouped together.
 * ------------
 * Holding a key: holding a single key.
 * Holding a button: holding one of the keys in the set.
 * Holding an ability: holding one of the buttons in the set.
 * ------------
 */

public class Controls {
    
    public static Dictionary<string, string> abilityMap = getDefaultAbilityMap();
    public static Dictionary<string, string> buttonMap = getDefaultButtonMap();
    
    // static Dictionary<string, List<string>> abilityKeys;
    // static Dictionary<string, List<string>> abilityButtons;
    // static Dictionary<string, List<string>> buttonKeys;
    
    public static Dictionary<string, string> getDefaultAbilityMap() {
        Dictionary<string, string> map = new Dictionary<string, string>();
        
        map.Add("A", "Paint");
        map.Add("B", "Inhale");
        map.Add("X", "Jump");
        map.Add("Y", "Magic");
        // map["A"] = "Mike";
        // map["A"] = "Cook";
        
        return map;
    }
    
    public static Dictionary<string, string> getDefaultButtonMap() {
        Dictionary<string, string> map = new Dictionary<string, string>();
        
        map.Add("q", "Left");
        map.Add("a", "Left");
        map.Add("d", "Right");
        map.Add("z", "Up");
        map.Add("w", "Up");
        map.Add("s", "Down");
        
        map.Add("j", "Y");
        map.Add("k", "B");
        map.Add("l", "A");
        map.Add("i", "X");
        map.Add("space", "X");
        map.Add("joystick button 0", "Y");
        map.Add("joystick button 1", "B");
        map.Add("joystick button 2", "A");
        map.Add("joystick button 3", "X");
        map.Add("joystick button 4", "L");
        map.Add("joystick button 5", "R");
        map.Add("joystick button 6", "ZL");
        map.Add("joystick button 7", "ZR");
        
        return map;
    }
    
    public static bool getButtonDown(string button) {
        foreach(string key in buttonMap.Keys) {
            if(buttonMap[key] == button && Input.GetKeyDown(key)) {
                return true;
            }
        }
        
        return false;
    }
    
    public static bool getButtonUp(string button) {
        foreach(string key in buttonMap.Keys) {
            if(buttonMap[key] == button && Input.GetKeyUp(key)) {
                return true;
            }
        }
        
        return false;
    }
    
    public static bool getButton(string button) {
        foreach(string key in buttonMap.Keys) {
            if(buttonMap[key] == button && Input.GetKey(key)) {
                return true;
            }
        }
        
        return false;
    }
    
    public static bool getAbilityDown(string ability) {
        foreach(string button in abilityMap.Keys) {
            if(abilityMap[button] == ability && getButtonDown(button)) {
                return true;
            }
        }
        
        return false;
    }
    
    public static bool getAbilityUp(string ability) {
        foreach(string button in abilityMap.Keys) {
            if(abilityMap[button] == ability && getButtonUp(button)) {
                return true;
            }
        }
        
        return false;
    }
    
    public static bool getAbility(string ability) {
        foreach(string button in abilityMap.Keys) {
            if(abilityMap[button] == ability && getButton(button)) {
                return true;
            }
        }
        
        return false;
    }
    
    public static string getAbilityForButton(string button) {
        if(button != null && abilityMap.ContainsKey(button)) {
            return abilityMap[button];
        }
        
        return null;
    }
    
    public static void setAbilityForButton(string button, string ability) {
        abilityMap[button] = ability;
    }
    
    // public static List<string> getAbilityKeys(string ability) {
        // List<string> keys = new List<string>();
        
        // foreach(string button in abilityMap.Keys) {
            // if(abilityMap[button] == ability) {
                // foreach(string key in buttonMap.Keys) {
                    // if(buttonMap[key] == button) {
                        // keys.Add(key);
                    // }
                // }
            // }
        // }
        
        // return keys;
    // }
    
    public static float getHorizontalAxis() {
        float axis = Input.GetAxisRaw("Horizontal");
        
        if(getButton("Left")) { axis += -1; }
        if(getButton("Right")) { axis += +1; }
        
        if(axis < -1) { axis = -1; }
        if(axis > +1) { axis = +1; }
        
        return axis;
    }
    
    public static float getVerticalAxis() {
        float axis = Input.GetAxisRaw("Vertical");
        
        if(getButton("Up")) { axis += +1; }
        if(getButton("Down")) { axis += -1; }
        
        if(axis < -1) { axis = -1; }
        if(axis > +1) { axis = +1; }
        
        return axis;
    }
    
}
