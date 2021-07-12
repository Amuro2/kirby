using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuScript : MonoBehaviour {
    
    public Transform[] abilities;
    public Transform[] buttons;
    
    public Transform selectAbilityCursor;
    public Transform selectButtonCursor;
    
    public GameObject blackScreen;
    
    public List<Transform> unlockableAbilities;
    
    public Transform resumeButton;
    
    Transform selectedAbility = null;
    GameObject lastSelectedGameObject;
    
    int a = 0;
    int b = 1;
    int x = 2;
    int y = 3;
    bool subMenu = false;
    
    void Awake() {
        setSlot(buttons[a], Controls.getAbilityForButton("A"));
        setSlot(buttons[b], Controls.getAbilityForButton("B"));
        setSlot(buttons[x], Controls.getAbilityForButton("X"));
        setSlot(buttons[y], Controls.getAbilityForButton("Y"));
    }
    
    void OnEnable() {
        for(int i = 0; i < unlockableAbilities.Count; ++i) {
            Transform ability = unlockableAbilities[i];
            ability.gameObject.SetActive(PersistentStuff.abilities[i]);
            // ability.gameObject.SetActive(true);
        }
        
        if(lastSelectedGameObject == null) {
            lastSelectedGameObject = abilities[0].gameObject;
        }
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(lastSelectedGameObject);
        }
        
        if(!subMenu) {
            
            for(int i = 0; i < buttons.Length; ++i) {
                
                if(!subMenu && buttons[i].gameObject == EventSystem.current.currentSelectedGameObject) {
                    EventSystem.current.SetSelectedGameObject(lastSelectedGameObject);
                    
                    break;
                }
                
            }
            
        }
        
        if(lastSelectedGameObject != EventSystem.current.currentSelectedGameObject) {
            onselectchange(EventSystem.current.currentSelectedGameObject, lastSelectedGameObject);
        }
        
        lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        
        if(subMenu && Input.GetButtonDown("Cancel")) {
            cancelBlackScreen();
        }
    }
    
    Transform getTransform(GameObject gameObject) {
        if(gameObject.transform != null) {
            return gameObject.transform;
        }
        
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        
        if(rectTransform != null) {
            return rectTransform;
        }
        
        return null;
    }
    
    void onselectchange(GameObject newSelected, GameObject oldSelected) {
        // Debug.Log(newSelected + " -- " + oldSelected);
        
        if(isAbility(newSelected) || newSelected.transform == resumeButton) {
            Transform newTransform = getTransform(newSelected);
            
            selectAbilityCursor.SetParent(newTransform);
            selectAbilityCursor.SetSiblingIndex(2);
            selectAbilityCursor.position = newTransform.position;
            // selectAbilityCursor.gameObject.GetComponent<RectTransform>().sizeDelta = newTransform.gameObject.GetComponent<RectTransform>().sizeDelta + new Vector2(16, 16);
            selectAbilityCursor.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(-8, -8);
            selectAbilityCursor.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(+8, +8);
            
            // Debug.Log(newTransform + " -- " + newTransform.gameObject.GetComponent<RectTransform>().sizeDelta);
        }
        
        if(isButton(newSelected)) {
            Transform newTransform = getTransform(newSelected);
            
            selectButtonCursor.SetParent(newTransform);
            selectButtonCursor.SetSiblingIndex(0);
            selectButtonCursor.position = newTransform.position;
            selectButtonCursor.gameObject.SetActive(true);
        }
    }
    
    bool isAbility(Transform transform) {
        for(int i = 0; i < abilities.Length; ++i) {
            if(abilities[i] == transform) {
                return true;
            }
        }
        
        return false;
    }
    
    bool isAbility(GameObject gameObject) {
        return isAbility(getTransform(gameObject));
    }
    
    bool isButton(Transform transform) {
        for(int i = 0; i < buttons.Length; ++i) {
            if(buttons[i] == transform) {
                return true;
            }
        }
        
        return false;
    }
    
    bool isButton(GameObject gameObject) {
        return isButton(getTransform(gameObject));
    }
    
    public void selectAbility(Transform ability) {
        selectedAbility = ability;
        
        EventSystem.current.SetSelectedGameObject(selectButtonCursor.parent.gameObject);
        blackScreen.SetActive(true);
        setSubMenu(true);
    }
    
    void setSlot(Transform buttonSlot, string ability) {
        
        buttonSlot.Find("Text").gameObject.GetComponent<TextOutline>().color = PersistentStuff.abilitiesColorsDim[ability];
        buttonSlot.Find("Text").gameObject.GetComponent<Text>().text = ability;
        // buttonSlot.gameObject.GetComponent<Image>().color = PersistentStuff.abilitiesColors[ability];
        buttonSlot.gameObject.GetComponent<Image>().sprite = PersistentStuff.abilitiesIcons[ability];
        
    }
    
    public void clickSlot(Transform buttonSlot) {
        if(selectedAbility != null) {
            string abilityName = selectedAbility.Find("Text").gameObject.GetComponent<Text>().text;
            setSlot(buttonSlot, abilityName);
            
            if(buttonSlot == buttons[a]) {
                Controls.setAbilityForButton("A", abilityName);
            }
            
            if(buttonSlot == buttons[b]) {
                Controls.setAbilityForButton("B", abilityName);
            }
            
            if(buttonSlot == buttons[x]) {
                Controls.setAbilityForButton("X", abilityName);
            }
            
            if(buttonSlot == buttons[y]) {
                Controls.setAbilityForButton("Y", abilityName);
            }
            
            cancelBlackScreen();
        }
    }
    
    void setSubMenu(bool subMenu) {
        this.subMenu = subMenu;
        PauseCanvasScript.current.subMenu = subMenu;
    }
    
    public void cancelBlackScreen() {
        EventSystem.current.SetSelectedGameObject(selectedAbility.gameObject);
        selectButtonCursor.gameObject.SetActive(false);
        blackScreen.SetActive(false);
        setSubMenu(false);
    }
    
}
