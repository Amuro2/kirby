using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTrick : GenericAction {
    
    List<GameObject> cards = new List<GameObject>();
    Dictionary<GameObject, Vector3> offsets = new Dictionary<GameObject, Vector3>();
    
    public CardTrick() {
        OnStart.AddListener(() => {
            freezeUserFacingX(true);
        });
        
        OnEnd.AddListener(() => {
            freezeUserFacingX(false);
        });
    }
    
    public override void fixedUpdate() {
        if(fstep == 0/4) {
            animator.SetTrigger("startCardTrick");
            
            airStall();
            
            makeCard(new Vector3(getUserFacingX() * 1, 0.35f, 0));
            makeCard(new Vector3(getUserFacingX() * 1, 0.10f, 0));
            makeCard(new Vector3(getUserFacingX() * 1, -0.15f, 0));
            
            Timeout.setFixed(() => {
                cards[0].GetComponent<Rigidbody2D>().velocity = new Vector2(getUserFacingX() * 16, 0);
            }, 16);
            
            Timeout.setFixed(() => {
                cards[1].GetComponent<Rigidbody2D>().velocity = new Vector2(getUserFacingX() * 16, 0);
            }, 22);
            
            Timeout.setFixed(() => {
                cards[2].GetComponent<Rigidbody2D>().velocity = new Vector2(getUserFacingX() * 16, 0);
            }, 28);
            
        }
        
        if(fstep < 64/4) {
            foreach(GameObject card in cards) {
                if(card != null) {
                    card.transform.position = user.position + offsets[card];
                }
            }
        }
        
        if(fstep == 64/4 && cards[0] != null) {
            airStall();
            
            // cards[0].GetComponent<Rigidbody2D>().velocity = new Vector2(getUserFacingX() * 16, 0);
        }
        
        if(fstep == 88/4 && cards[1] != null) {
            // cards[1].GetComponent<Rigidbody2D>().velocity = new Vector2(getUserFacingX() * 16, 0);
        }
        
        if(fstep == 112/4 && cards[2] != null) {
            // cards[2].GetComponent<Rigidbody2D>().velocity = new Vector2(getUserFacingX() * 16, 0);
        }
        
        if(fstep == 128/4) {
            dispatchEnd();
        }
    }
    
    class MagicWarp : MonoBehaviour {
        
        public Transform user;
        
        Vector3 lastPosition;
        
        void Awake() {
            lastPosition = transform.position;
        }
        
        void Update() {
            lastPosition = transform.position;
        }
        
        void OnDestroy() {
            
            // Debug.Log("Destroy -- " + lastPosition + " -- " + transform.position);
            
            Vector2 position = (transform.position + lastPosition) / 2;
            
            if(Controls.getButton("L") && Physics2D.OverlapAreaAll(position + new Vector2(0.03125f, 0.03125f), position - new Vector2(0.03125f, 0.03125f), LayerMask.GetMask("Ground")).Length == 0) {
                user.transform.position = lastPosition;
                // Debug.Log("Warped here: " + user.transform.position);
            }
            
        }
    }
    
    void makeCard(Vector3 position) {
        GameObject card = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/Card"));
        
        card.GetComponent<Hitbox>().whiteList.Add(user.gameObject);
        
        card.GetComponent<Hitbox>().OnHit.AddListener((GameObject collider) => {
            
            GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>("effects/SmokeExplosion2"));
            effect.transform.position = (card.transform.position + collider.transform.position) / 2;
            effect.SetActive(true);
            
            PersistentStuff.fillAbilityGauge("Magic", 0.0625);
            
            // if(Controls.getButton("L") && Physics2D.OverlapAreaAll(card.transform.position + new Vector3(0.25f, 0.25f), card.transform.position - new Vector3(0.25f, 0.25f), LayerMask.GetMask("Ground")).Length == 0) {
                // user.transform.position = card.transform.position;
            // }
            
        });
        
        // card.AddComponent<MagicWarp>().user = user;
        
        // card.GetComponent<ContactVanish>().OnVanish.AddListener(() => {
            
            // if(Controls.getButton("L") && Physics2D.OverlapAreaAll(card.transform.position + new Vector3(0.25f, 0.25f), card.transform.position - new Vector3(0.25f, 0.25f), LayerMask.GetMask("Ground")).Length == 0) {
                // user.transform.position = card.transform.position;
            // }
            
        // });
        
        card.transform.SetParent(user.parent);
        
        card.transform.position = user.position + position;
        
        card.SetActive(true);
        
        cards.Add(card);
        offsets.Add(card, position);
    }
    
}
