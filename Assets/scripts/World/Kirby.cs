using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kirby : MonoBehaviour {
    
    public static Kirby current;
    public static int currentHealth = -1;
    
    public SpriteRenderer spriteRenderer;
    
    public new Rigidbody2D rigidbody;
    
    public Collider2D groundCollider;
    public Animator animator;
    
    float walkSpeed = 3;
    float runSpeed = 5;
    float swimSpeed = 5;
    
    double horizontalAxis = 0;
    double verticalAxis = 0;
    bool standStill = false;
    bool freezeFace = false;
    public double xSpeed = 0;
    public double ySpeed = 0;
    bool yControl = false;
    public double yForce = 0;
    bool running = false;
    public bool floating = false;
    bool underwater = false;
    bool wasUnderwater = false;
    public int bellyCount = 0;
    public bool bypassKirbies;
    
    GenericAction action;
    
    void Awake() {
        current = this;
        
        // 
        
        Animator animator = GetComponent<Animator>();
        
        if(animator != null) {
            animator.keepAnimatorControllerStateOnDisable = true;
        }
        
        // 
        
        Damageable damageable = gameObject.GetComponent<Damageable>();
        
        damageable.OnDefeat.AddListener(() => {
            if(!bypassKirbies && PersistentStuff.getKirbiesCount() > 0) {
                Sound.play("lose_life");
                damageable.setHealth(1);
            } else {
                Sound.play("lose_game");
                AudioScript.current.setVolume(0.25f);
                
                Timeout.setMs(() => {
                    World.current.transitionToRoom();
                    
                    Timeout.setMs(() => {
                        AudioScript.current.setVolumeTransition(1, 1000);
                    }, 1000);
                }, 2000);
                
                // Defeat animation
                
                CameraScript.current.targets.Clear();
                
                Destroy(gameObject.GetComponent<BoxCollider2D>());
                
                rigidbody.velocity = new Vector2(0, +16);
                rigidbody.gravityScale = 2.5f;
                
                // Prevent various scripts
                
                foreach(MonoBehaviour component in gameObject.GetComponents<MonoBehaviour>()) {
                    if(!(component is Kirby)) {
                        Destroy(component);
                    }
                }
                
                World.current.disableScripts();
                
                this.enabled = false;
            }
            
            PersistentStuff.decreaseKirbies(1);
        });
        
        damageable.OnHealthChange.AddListener((newHealth, oldHealth) => {
            currentHealth = newHealth;
            WorldUI.current.setKirbyHealthRatio(damageable.getHealthRatio());
        });
        
    }
    
    public bool isGrounded() {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        
        Vector3 point1 = transform.position + (Vector3)(collider.offset - collider.size/2);
        Vector3 point2 = transform.position + (Vector3)(collider.offset + collider.size/2);
        
        point1.x += 0.03125f;
        point2.x -= 0.03125f;
        
        point2.y = point1.y + collider.size.y*1/8;
        
        Collider2D[] colliders = Physics2D.OverlapAreaAll(point1, point2, LayerMask.GetMask("Ground", "PlayerWall"));
        
        return colliders.Length > 0;
    }
    
    public bool isRunning() {
        return running;
    }
    
    public bool isFloating() {
        return floating;
    }
    
    public bool isUnderwater() {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        
        Vector2 size = collider.size / 8;
        
        Vector3 point1 = transform.position + (Vector3)(collider.offset - size/2);
        Vector3 point2 = transform.position + (Vector3)(collider.offset + size/2);
        // point2[1] += collider.size.y/2;
        
        Collider2D[] colliders = Physics2D.OverlapAreaAll(point1, point2, LayerMask.GetMask("Water"));
        
        return colliders.Length > 0;
        
        // return underwater;
    }
    
    public bool isAirborne() {
        return !isGrounded() && !isUnderwater();
    }
    
    public float getFacingX() {
        if(spriteRenderer.flipX) {
            return -1;
        }
        
        return +1;
    }
    
    public void setFacingX(bool flipX) {
        spriteRenderer.flipX = flipX;
    }
    
    public void setFacingX(int direction) {
        if(direction < 0) {
            setFacingX(true);
        }
        
        else if(direction > 0) {
            setFacingX(false);
        }
    }
    
    public void setStill(bool still) {
        standStill = still;
    }
    
    public void freezeFacingX(bool freeze = true) {
        freezeFace = freeze;
    }
    
    bool actionHasJustEnded = false;
    
    public void setAction(GenericAction pAction) {
        if(action != null && action != pAction && (action.isCancelableWith(pAction) || pAction is HurtState)) {
            action.dispatchEnd();
            dispatchActionEnd(action);
            action = null;
        }
        
        if(action == null) {
            action = pAction;
            action.user = transform;
            action.animator = animator;
            action.OnEnd.AddListener(() => {
                dispatchActionEnd(action);
                action = null;
                actionHasJustEnded = true;
            });
            
            action.dispatchStart();
            dispatchActionStart(action);
        }
    }
    
    public bool isBellyFull() {
        return bellyCount > 0;
    }
    
    public void setBellyCount(int count) {
        if(bellyCount == 0 && count > 0) {
            animator.SetBool("full", true);
        } else if(bellyCount > 0 && count == 0) {
            animator.SetBool("full", false);
        }
        
        bellyCount = count;
    }
    
    public void incBellyCount() {
        setBellyCount(bellyCount + 1);
    }
    
    public int getBellyCount() {
        return bellyCount;
    }
    
    public bool abilitySpecialFull(string ability) {
        return PersistentStuff.isAbilityFull(ability);
    }
    
    // Start is called before the first frame update
    void Start() {
        
        Damageable damageable = gameObject.GetComponent<Damageable>();
        
        if(currentHealth > 0) {
            damageable.setHealth(currentHealth);
        } else {
            currentHealth = damageable.health;
        }
        
        if(isUnderwater()) {
            wasUnderwater = true;
            
            yControl = true;
            
        } else if(isGrounded()) {
            dispatchLand();
        } else {
            dispatchGroundLeave();
            dispatchFallStart();
        }
        
    }
    
    int countSinceHorizontalRelease = 32;
    int countSinceHorizontalPress = 0;
    bool previouslyGrounded;
    
    bool hasJustLanded() {
        return isGrounded() && !previouslyGrounded;
    }
    
    bool airborneJustNow() {
        return !isGrounded() && previouslyGrounded;
    }
    
    // Update is called once per frame
    void Update() {
        
        // 
        
        horizontalAxis = Controls.getHorizontalAxis();
        verticalAxis = Controls.getVerticalAxis();
        Vector2 moveDirection = new Vector2((float)horizontalAxis, (float)verticalAxis);
        
        // 
        
        if(isGrounded()) {
            xSpeed /= 1.25;
        } else {
            xSpeed /= 1.03125;
        }
        
        if(Mathf.Abs((float)xSpeed) < 0.001) {
            xSpeed = 0;
        }
        
        ySpeed /= 1.03125;
        
        if(Mathf.Abs((float)ySpeed) < 0.001) {
            ySpeed = 0;
        }
        
        //// Gravity update ////
        
        if(isFloating()) {
            if(isUnderwater()) {
                rigidbody.gravityScale = -30f;
            } else {
                rigidbody.gravityScale = 1f;
            }
        } else if(isUnderwater()) {
            rigidbody.gravityScale = 0f;
        } else {
            rigidbody.gravityScale = 2.5f;
        }
        
        //// Fall ////
        
        if(isAirborne() && !isFloating()) {
            setAction(new Fall());
        } else if(action is Fall) {
            action.dispatchEnd();
        }
        
        //// Jump ////
        
        if(Controls.getAbilityDown("Jump")) {
            if(isBellyFull()) {
                if(isGrounded()) {
                    setAction(new Jump());
                }
            } else {
                if(isFloating()) {
                    setAction(new Hover());
                } else if(isUnderwater()) {
                    Torpedo torpedo = new Torpedo();
                    
                    if(moveDirection.x == 0 && moveDirection.y == 0) {
                        torpedo.direction = new Vector2(getFacingX(), 0);
                    } else {
                        torpedo.direction = moveDirection;
                    }
                    
                    setAction(torpedo);
                } else if(isGrounded()) {
                    if(verticalAxis < 0) {
                        setAction(new Slide());
                    } else {
                        setAction(new Jump());
                    }
                } else {
                    setAction(new Hover());
                }
            }
        }
        
        if(isFloating() && Controls.getAbility("Jump")) {
            setAction(new Hover());
        }
        
        if(action is Slide && !isGrounded()) {
            action.dispatchEnd();
        }
        
        if(action is Torpedo) {
            if(GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")) && !isUnderwater()) {
                action.dispatchEnd();
            } else {
                Torpedo torpedo = (Torpedo)action;
                
                if(moveDirection.x == 0 && moveDirection.y == 0) {
                    // torpedo.direction = new Vector2(getFacingX(), 0);
                } else {
                    torpedo.direction = moveDirection;
                }
            }
        }
        
        if((action is Jump || action is Torpedo || action is Hover) && !Controls.getAbility("Jump")) {
            action.dispatchEnd();
        }
        
        if(isFloating() && isGrounded()) {
            setAction(new AirGun());
        }
        
        //// Inhale ////
        
        if(Controls.getAbilityDown("Inhale")) {
            if(isBellyFull()) {
                setAction(new StarSpit());
            } else if(isFloating()) {
                setAction(new AirGun());
            } else {
                setAction(new Inhale());
            }
        }
        
        if(!Controls.getAbility("Inhale")) {
            if(action is Inhale && !((Inhale)(action)).isInhaling()) {
                action.dispatchEnd();
            }
        }
        
        //// Paint ////
        
        if(Controls.getAbilityDown("Paint")) {
            if(isBellyFull()) {
                setAction(new StarSpit());
            } else if(isFloating()) {
                setAction(new AirGun());
            } else {
                if(verticalAxis < 0) {
                    setAction(new Painter());
                } else if(verticalAxis > 0) {
                    setAction(new RisingBrushSlash());
                } else if(isRunning()) {
                    setAction(new PaintSpin());
                } else {
                    if(abilitySpecialFull("Paint")) {
                        GenericAction charge = new PaintCharge();
                        
                        setAction(charge);
                    } else {
                        setAction(new BrushSlash());
                    }
                }
            }
        }
        
        if(!Controls.getAbility("Paint") && action is PaintCharge) {
            int time = ((Charge)action).time;
            
            action.dispatchEnd();
            
            if(time > 128/8) {
                setAction(new PaintOut());
            } else {
                setAction(new BrushSlash());
            }
        }
        
        //// Mike ////
        
        if(Controls.getAbilityDown("Mike")) {
            if(isBellyFull()) {
                setAction(new StarSpit());
            } else if(isFloating()) {
                setAction(new AirGun());
            } else {
                if(isRunning()) {
                    setAction(new AmpedShout());
                } else {
                    if(abilitySpecialFull("Mike")) {
                        setAction(new MikeCharge());
                    } else {
                        NoteShot action = new NoteShot();
                        
                        action.angle = verticalAxis;
                        
                        setAction(action);
                    }
                }
            }
            
            if(action is NoteShot) {
                ((NoteShot)action).cancelEndRequest();
            }
            
        }
        
        if(action is NoteShot) {
            ((NoteShot)action).angle = verticalAxis;
        }
        
        if(action is NoteShot && !Controls.getAbility("Mike")) {
            ((NoteShot)action).requestEnd();
        }
        
        if(!Controls.getAbility("Mike") && action is MikeCharge) {
            int time = ((Charge)action).time;
            
            action.dispatchEnd();
            
            if(time > 128/8) {
                setAction(new Megaphone());
            } else {
                NoteShot action = new NoteShot();
                
                action.angle = verticalAxis;
                action.requestEnd();
                
                setAction(action);
            }
        }
        
        //// Cook ////
        
        if(Controls.getAbilityDown("Cook")) {
            if(isBellyFull()) {
                setAction(new StarSpit());
            } else if(isFloating()) {
                setAction(new AirGun());
            } else {
                if(verticalAxis > 0) {
                    setAction(new UpLadle());
                } else if(isRunning()) {
                    setAction(new Plateware());
                } else {
                    if(abilitySpecialFull("Cook")) {
                        setAction(new CookCharge());
                    } else {
                        setAction(new Ladle());
                    }
                }
            }
        }
        
        if(!Controls.getAbility("Cook") && action is CookCharge) {
            int time = ((Charge)action).time;
            
            action.dispatchEnd();
            
            if(time > 128/8) {
                setAction(new CookPot());
            } else {
                setAction(new Ladle());
            }
        }
        
        //// Magic ////
        
        if(Controls.getAbilityDown("Magic")) {
            if(isBellyFull()) {
                setAction(new StarSpit());
            } else if(isFloating()) {
                setAction(new AirGun());
            } else {
                if(verticalAxis > 0) {
                    setAction(new JackInTheBox());
                } else if(horizontalAxis != 0) {
                    setAction(new CardTrick());
                } else if(abilitySpecialFull("Magic")) {
                    GenericAction action = new MagicCharge();
                    
                    action.OnEnd.AddListener(() => {
                        if(action.step > 128) {
                            setAction(new LuckOfTheDraw());
                        } else {
                            setAction(new Doves());
                        }
                    });
                    
                    setAction(action);
                } else {
                    setAction(new Doves());
                }
            }
        }
        
        if(!Controls.getAbility("Magic") && action is MagicCharge) {
            int time = ((Charge)action).time;
            
            action.dispatchEnd();
            
            if(time > 128/8) {
                setAction(new LuckOfTheDraw());
            } else {
                setAction(new Doves());
            }
        }
        
        ////  ////
        
        if(action != null) {
            action.update();
            
            if(action != null) {
                ++action.step;
            }
        }
        
        //// Walking / Running ////
        
        // Animator stuff
        
        animator.SetBool("airborne", isAirborne());
        animator.SetBool("underwater", isUnderwater());
        
        if(horizontalAxis == 0 || isAirborne() || isUnderwater()) {
            
            animator.SetBool("walking", false);
            animator.SetBool("running", false);
            
        } else {
            
            if(isRunning()) {
                animator.SetBool("running", true);
                animator.SetBool("walking", false);
            } else {
                animator.SetBool("walking", true);
            }
            
        }
        
        if(moveDirection.magnitude > 0 && isUnderwater()) {
            animator.SetBool("swimming", true);
        } else {
            animator.SetBool("swimming", false);
        }
        
        // Running or not
        
        if(!isUnderwater() && isGrounded()) {
            
            if(horizontalAxis == 0) {
                running = false;
            }
            
            else if(!Input.GetButton("Horizontal") && countSinceHorizontalPress < 6) {
                if(Mathf.Abs((float)horizontalAxis) > 0.9) {
                    running = true;
                }
            }
            
            else if(countSinceHorizontalRelease > 0 && countSinceHorizontalRelease < 32) {
                running = true;
            }
            
        }
        
        if(isFloating() || isUnderwater()) {
            running = false;
        }
        
        if(horizontalAxis == 0) {
            countSinceHorizontalPress = 0;
            ++countSinceHorizontalRelease;
        } else {
            countSinceHorizontalRelease = 0;
            ++countSinceHorizontalPress;
        }
        
        // Actually walking / running
        
        if(!isUnderwater()) {
            
            if(horizontalAxis != 0 && !standStill) {
                if(isRunning()) {
                    xSpeed = horizontalAxis * runSpeed;
                } else {
                    xSpeed = horizontalAxis * walkSpeed;
                }
            }
            
        }
        
        // Crouching etc
        
        if(verticalAxis < 0 && isGrounded() && !isUnderwater()) {
            if(isBellyFull()) {
                setBellyCount(0);
            } else {
                setAction(new Crouch());
            }
        } else {
            if(action is Crouch) {
                action.dispatchEnd();
            }
        }
        
        // Swimming
        
        if(isUnderwater() && !standStill) {
            Vector2 direction = moveDirection.normalized * swimSpeed;
            
            if(direction.magnitude > 0) {
                xSpeed = direction.x;
                ySpeed = direction.y;
            }
            
        }
        
        if(!freezeFace) {
            if(horizontalAxis > 0) {
                spriteRenderer.flipX = false;
            }
            
            if(horizontalAxis < 0) {
                spriteRenderer.flipX = true;
            }
        }
        
        //// The rest ////
        
        if(airborneJustNow() || (actionHasJustEnded && !isGrounded())) {
            dispatchFallStart();
        } else if(hasJustLanded()) {
            dispatchLand();
        }
        
        if(!wasUnderwater && isUnderwater()) {
            dispatchWaterEnter();
        }
        
        else if(!isUnderwater() && wasUnderwater) {
            yControl = false;
            dispatchWaterLeave();
        }
        
        actionHasJustEnded = false;
        
        previouslyGrounded = isGrounded();
        wasUnderwater = isUnderwater();
        // underwater = false;
        
    }
    
    Vector3 lastPosition = new Vector3();
    
    void FixedUpdate() {
        
        if(xSpeed != 0) {
            // Debug.Log(xSpeed);
            // Debug.Log(Time.deltaTime * xSpeed);
        }
        
        // transform.position = transform.position + new Vector3((float)xSpeed * Time.deltaTime, 0);
        // rigidbody.AddForce(new Vector2((float)xSpeed, 0f));
        
        {
            
            Vector2 velocity = rigidbody.velocity;
            
            velocity.x = (float)xSpeed * Time.deltaTime * 50;
            
            if(yForce != 0) {
                velocity.y = (float)yForce;
                yForce = 0;
            }
            
            if(yControl) {
                velocity.y = (float)ySpeed * Time.deltaTime * 50;
            }
            
            rigidbody.velocity = velocity;
            
        }
        
        if(!transform.position.Equals(lastPosition)) {
            // Debug.Log(transform.position);
        }
        
        if(action != null) {
            action.fixedUpdate();
            
            if(action != null) {
                ++action.fstep;
            }
        }
        
        lastPosition = transform.position;
        
        // if(WorldUI.current != null) { WorldUI.current.setDebug(action + ""); }
        
        //// Getting crushed between grounds ////
        
        Vector2 point1 = (Vector2)(transform.position - transform.localScale/2) + new Vector2(0.25f, 0.25f);
        Vector2 point2 = (Vector2)(transform.position + transform.localScale/2) - new Vector2(0.25f, 0.25f);
        
        if(Physics2D.OverlapAreaAll(point1, point2, LayerMask.GetMask("Ground")).Length > 0) {
            Hitbox hitbox = GetComponent<Hitbox>();
            
            if(hitbox == null) {
                hitbox = gameObject.AddComponent<Hitbox>();
            }
            
            hitbox.rehitRate = 0;
            hitbox.tagToHit = tag;
            hitbox.whiteList.Clear();
            hitbox.damage = 999999999;
            hitbox.bypassKirbies = true;
            hitbox.bypassInvincibility = true;
            
        }
        
    }
    
    void OnDrawGizmosSelected() {
        Vector3 size = transform.localScale;
        
        size.y /= 8;
        
        Vector3 position = transform.position;
        
        position.y -= transform.localScale.y/2 - size.y/2;
        
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireCube(position, size);
    }
    
    public void dispatchLand() {
        // animator.SetBool("falling", false);
        
        if(!isUnderwater()) {
            makeSmoke(-1.25f);
            makeSmoke(+1.25f);
        }
    }
    
    public void dispatchGroundLeave() {
        
    }
    
    public void dispatchFallStart() {
        // animator.SetTrigger("startFalling");
        // animator.SetBool("falling", true);
    }
    
    public void dispatchWaterEnter() {
        
        // Debug.Log("Entered water");
        
        ySpeed = rigidbody.velocity.y;
        
        Vector2 velocity = rigidbody.velocity;
        velocity.y = 0;
        rigidbody.velocity = velocity;
        
        yControl = true;
        
        // animator.SetBool("underwater", true);
        
        makeWaterSplash(lastPosition - transform.position);
        
    }
    
    public void dispatchWaterLeave() {
        // Debug.Log("Just left water");
        
        // animator.SetBool("underwater", false);
        
        makeWaterSplash(transform.position - lastPosition);
        
    }
    
    public void dispatchMovingStart() {
        
    }
    
    public void dispatchMovingStop() {
        // animator.SetBool("running", false);
        // animator.SetBool("walking", false);
    }
    
    public void dispatchRunStart() {
        // animator.SetBool("running", true);
    }
    
    public void dispatchWalkStart() {
        // animator.SetBool("walking", true);
    }
    
    public void dispatchSwimStart() {
        
    }
    
    public void dispatchActionStart(GenericAction action) {
        // Debug.Log("Started " + action);
        animator.SetBool("usingAction", true);
    }
    
    public void dispatchActionEnd(GenericAction action) {
        // Debug.Log("Ended " + action);
        animator.SetBool("usingAction", false);
    }
    
    public void dispatchBellyFull() {
        
    }
    
    public void dispatchBellyEmpty() {
        
    }
    
    public void dispatchFloatStart() {
        
    }
    
    public void dispatchFloatEnd() {
        
    }
    
    public void makeSmoke(float direction) {
        // GameObject smokeBall = Instantiate(Resources.Load<GameObject>("effects/SmokeBall"));
        
        // smokeBall.transform.position = new Vector3(transform.position.x + direction * 0.25f, transform.position.y - 0.5f, transform.position.z);
        // smokeBall.transform.localScale = new Vector3(0.375f, 0.375f, 0.375f);
        // smokeBall.GetComponent<SpriteRenderer>().flipX = direction < 0;
        
        // smokeBall.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * 1, 0);
        
        // smokeBall.transform.SetParent(transform.parent);
        // smokeBall.SetActive(true);
        
        List<GameObject> objects = (new PrefabExplosion())
        .setCount(16)
        .setCenter(new Vector3(transform.position.x + direction * 0.25f, transform.position.y - 0.5f, transform.position.z))
        .setPrefab(Resources.Load<GameObject>("effects/SmokeBall"))
        .setDirection(new Vector2(direction, 0))
        .setAngleVariation(Mathf.PI/8)
        .setLaunchNorm(2 * Mathf.Abs(direction), 4 * Mathf.Abs(direction))
        .start()
        .getObjects();
        
        foreach(GameObject gameObject in objects) {
            gameObject.transform.SetParent(transform.parent);
        }
    }
    
    public void makeSmokeAuto() {
        makeSmoke(-getFacingX() * 0.5f);
    }
    
    public void makeSmokeRunAuto() {
        makeSmoke(-getFacingX() * 0.75f);
    }
    
    public void makeWaterSplash(Vector3 direction) {
        
        List<GameObject> droplets = (new PrefabExplosion())
        .setCount(8)
        .setPrefab("effects/WaterDroplet")
        .setCenter(transform.position)
        .setDirection(direction)
        .setLaunchNorm(6, 8)
        .setAngleVariation(Mathf.PI/4)
        .start()
        .getObjects();
        
        foreach(GameObject droplet in droplets) {
            float sc = 0.125f + Random.value * (0.25f - 0.125f);
            
            droplet.GetComponent<InitialScaleTransition>().scaleStart = new Vector3(sc, sc, sc);
            droplet.GetComponent<InitialScaleTransition>().msDuration = (int)(500 + Random.value * (1500 - 500));
            
            droplet.transform.SetParent(transform.parent);
        }
        
    }
    
    // 
    
    bool positionDefined;
    List<System.Action> positionDefinedActions = new List<System.Action>();
    
    public void addPositionDefinedListener(System.Action action) {
        if(positionDefined) {
            action();
        } else {
            positionDefinedActions.Add(action);
        }
    }
    
    public void dispatchPositionDefined() {
        List<System.Action> clone = new List<System.Action>(positionDefinedActions);
        
        foreach(System.Action action in clone) {
            action();
            positionDefinedActions.Remove(action);
        }
        
        positionDefined = true;
    }
    
}
