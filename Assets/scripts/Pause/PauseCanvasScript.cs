using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PauseCanvasScript : MonoBehaviour {
    
    public static PauseCanvasScript current;
    
    public List<Transform> unlockablePages;
    public bool subMenu;
    
    int activeChildIndex = 0;
    List<Transform> unlockedPages;
    
    bool allowSwipe = false;
    bool swiping = false;
    
    float saveAudioVolume;
    
    void OnEnable() {
        current = this;
        
        unlockedPages = new List<Transform>();
        
        for(int i = 0; i < transform.Find("Pages").childCount; ++i) {
            Transform child = transform.Find("Pages").GetChild(i);
            
            if(child.gameObject.activeInHierarchy) {
                activeChildIndex = i;
            }
            
            if(unlockablePages.Contains(child)) {
                int index = unlockablePages.IndexOf(child);
                
                if(PersistentStuff.abilities[index]) {
                    unlockedPages.Add(child);
                }
            } else {
                unlockedPages.Add(child);
            }
        }
        
        if(AudioScript.current != null) {
            saveAudioVolume = AudioScript.current.getVolume();
            AudioScript.current.setVolume(0.375f);
        }
        
        allowSwipe = false;
        
        
        
        Color color = PersistentStuff.getLevelColor();
        color[3] = 0.125f;
        
        transform.Find("Panel (Circle)").GetComponent<Image>().color = color;
        
        transform.Find("Panel (Rectangle)").GetComponent<Image>().color = color;
        
    }
    
    // Start is called before the first frame update
    void Start() {
        
    }
    
    // Update is called once per frame
    void Update() {
        if(!subMenu) {
            if(Input.GetAxisRaw("Horizontal") == 0) {
                allowSwipe = true;
            }
            
            if(Input.GetAxisRaw("Horizontal") < 0 && allowSwipe && !swiping) {
                allowSwipe = false;
                swipe(-1);
            }
            
            if(Input.GetAxisRaw("Horizontal") > 0 && allowSwipe && !swiping) {
                allowSwipe = false;
                swipe(+1);
            }
            
            if(Input.GetButtonDown("Pause") || Input.GetButtonDown("Cancel")) {
                resumeGame();
            }
        }
    }
    
    void OnDisable() {
        if(AudioScript.current != null) {
            AudioScript.current.setVolume(saveAudioVolume);
        }
    }
    
    public void swipe(int diff) {
        GameObject page = unlockedPages[activeChildIndex].gameObject;
        
        page.GetComponent<PageSwipe>().swipeOff(new Vector3((float)-diff * 400, 0, 0));
        
        ////  ////
        
        activeChildIndex += diff;
        
        if(activeChildIndex < 0) {
            activeChildIndex = unlockedPages.Count - 1;
        }
        
        else if(activeChildIndex >= unlockedPages.Count) {
            activeChildIndex = 0;
        }
        
        ////  ////
        
        page = unlockedPages[activeChildIndex].gameObject;
        
        page.GetComponent<PageSwipe>().swipeOn(new Vector3((float)-diff * 400, 0, 0));
        page.GetComponent<PageSwipe>().OnSwipeEnd.AddListener(() => {
            swiping = false;
        });
        
        swiping = true;
        
    }
    
    public void resumeGame() {
        World.current.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    
}
