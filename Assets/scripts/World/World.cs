using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {
    
    public static World current;
    
    public List<MonoBehaviour> scriptsToDisable = new List<MonoBehaviour>();
    
    GameObject pauseCanvas;
    
    void Awake() {
        current = this;
        
        PersistentStuff.setRoomName(SceneManager.GetActiveScene().name);
        
        GameObject worldUICanvas = Instantiate(Resources.Load<GameObject>("WorldUICanvas"));
        
        worldUICanvas.transform.SetParent(transform);
        
        pauseCanvas = Instantiate(Resources.Load<GameObject>("PauseCanvas"));
        pauseCanvas.SetActive(false);
        
    }
    
    // Start is called before the first frame update
    void Start() {
        Flags.lowerFlag("inBattle");
        
        Kirby kirby = Kirby.current;
        
        if(kirby != null) {
            
            if(PersistentStuff.isRoomPositionDefined()) {
                // Debug.Log("Room position is defined, Kirby is moved to " + PersistentStuff.getRoomPosition() + ".");
                kirby.transform.position = PersistentStuff.getRoomPosition();
                kirby.setFacingX(PersistentStuff.flipXOnStart());
            } else {
                // Debug.Log("Room position not defined, Kirby is left as is.");
                PersistentStuff.setRoomPosition(kirby.transform.position);
            }
            
            kirby.dispatchPositionDefined();
            
            CameraScript.current.centerOnTargets();
            PersistentStuff.fadeIn();
            
        }
        
        Door.moving = false;
    }
    
    // Update is called once per frame
    void Update() {
        if(Input.GetButtonDown("Pause") && AbilitySplash.current == null && FadeCenterOnKirby.current == null) {
            pauseCanvas.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    
    public void disableScripts() {
        foreach(MonoBehaviour script in scriptsToDisable) {
            script.enabled = false;
        }
        
        Kirby.current.enabled = false;
        
        this.enabled = false;
    }
    
    public void transitionToRoom(string nextRoom, Vector3 nextPosition, bool nextFlipX) {
        disableScripts();
        
        PersistentStuff.transitionToRoom(nextRoom, nextPosition, nextFlipX);
    }
    
    public void transitionToRoom() {
        transitionToRoom(SceneManager.GetActiveScene().name, PersistentStuff.roomPosition, PersistentStuff.flipXOnStart());
    }
    
}
