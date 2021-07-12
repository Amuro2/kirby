using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inhale : GenericAction {
    
    Transform inhaleBox;
    
    public Inhale() {
        OnStart.AddListener(() => {
            animator.SetTrigger("startInhaling");
            freezeUserFacingX(true);
            
            inhaleBox = GameObject.Instantiate(Resources.Load<GameObject>("collision_boxes/InhaleBox")).transform;
            
            inhaleBox.gameObject.GetComponent<InhaleBox>().origin = user;
            inhaleBox.gameObject.GetComponent<InhaleBox>().OnInhaleEnd.AddListener((GameObject gameObject) => {
                getUserScript().incBellyCount();
            });
            
            inhaleBox.SetParent(user);
            
            Vector3 position = user.position;
            
            position.x += getUserFacingX() * inhaleBox.localScale.x/2;
            
            inhaleBox.position = position;
        });
        
        OnEnd.AddListener(() => {
            GameObject.Destroy(inhaleBox.gameObject);
            
            setUserStill(false);
            freezeUserFacingX(false);
        });
    }
    
    public override void update() {
        setUserStill(isGrounded());
        
        if(step % 2 == 0) {
            GameObject bubble = GameObject.Instantiate(Resources.Load<GameObject>("effects/InhaleBubble"));
            
            float y = -1 + Random.value * (2);
            
            BubbleBehaviour behaviour = bubble.AddComponent<BubbleBehaviour>();
            behaviour.distY = y;
            behaviour.source = user;
            
            bubble.transform.position = user.position + new Vector3(getUserFacingX() * (2.5f - Mathf.Cos(y * Mathf.PI/2) * 0.25f), y, 0);
            
            float sc = 0.125f/2 + Random.value * (0.25f - 0.125f/2);
            bubble.transform.localScale = new Vector3(sc, sc, sc);
            bubble.GetComponent<InitialScaleTransition>().scaleStart = bubble.transform.localScale;
            
            Color color = new Color(0.5f + Random.value * (1 - 0.5f), 1, 1);
            
            bubble.GetComponent<SpriteRenderer>().color = color;
            
            InitialColorTransition ct = bubble.AddComponent<InitialColorTransition>();
            
            ct.colorStart = color;
            ct.colorEnd = color;
            ct.colorEnd[3] = 0;
            ct.msDuration = (int)bubble.GetComponent<InitialScaleTransition>().msDuration;
            ct.exponent = 1.5;
            
            bubble.transform.SetParent(user);
        }
        
        
    }
    
    class BubbleBehaviour : MonoBehaviour {
        
        public float distY;
        public Transform source;
        
        void FixedUpdate() {
            Vector3 adjustedSourcePosition = source.position;
            adjustedSourcePosition.x += Mathf.Sign(transform.position.x - source.position.x) * 0.25f;
            adjustedSourcePosition.y += 0.03125f;
            
            float progress = GetComponent<InitialScaleTransition>().getProgress();
            
            transform.position = transform.position + (adjustedSourcePosition + new Vector3(0, distY/3, 0) - transform.position) * progress;
        }
        
    }
    
    class PebbleBehaviour : MonoBehaviour {
        
        public Transform source;
        public float angle;
        public float initialDistanceX;
        
        void Start() {
            angle = Random.value * 2*Mathf.PI;
            // Debug.Log(angle);
            
            
            Vector3 adjustedSourcePosition = source.position;
            adjustedSourcePosition.y += 0.03125f;
            
            Vector3 position = transform.position;
            
            position.x = adjustedSourcePosition.x + initialDistanceX;
            position.y = adjustedSourcePosition.y + Mathf.Sin(angle) * 1;
            
            transform.position = position;
        }
        
        void Update() {
            angle += 0.125f;
        }
        
        void FixedUpdate() {
            float progress = getProgress();
            
            Vector3 adjustedSourcePosition = source.position;
            adjustedSourcePosition.y += 0.03125f;
            
            Vector3 position = transform.position;
            
            float initialX = adjustedSourcePosition.x + initialDistanceX;
            
            position.x = initialX + (adjustedSourcePosition.x - initialX) * progress;
            position.x += Mathf.Cos(angle) * 0.5f;
            position.y = adjustedSourcePosition.y + Mathf.Sin(angle) * (1 + progress * (0.125f - 1));
            
            transform.position = position;
        }
        
        float getProgress() {
            InitialScaleTransition st = GetComponent<InitialScaleTransition>();
            
            return st.getProgress();
        }
        
    }
    
    public override void fixedUpdate() {
        if(fstep % 4 == 0) {
            GameObject pebble = GameObject.Instantiate(Resources.Load<GameObject>("effects/InhalePebble"));
            
            PebbleBehaviour behaviour = pebble.AddComponent<PebbleBehaviour>();
            behaviour.source = user;
            behaviour.initialDistanceX = getUserFacingX() * 2.5f;
            
        }
    }
    
    public bool isInhaling() {
        return inhaleBox.gameObject.GetComponent<InhaleBox>().isInhaling();
    }
    
}
