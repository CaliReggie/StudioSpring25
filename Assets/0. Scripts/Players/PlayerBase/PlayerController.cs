using System;
using System.Collections;
using Scripts;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Collision = Scripts.Collision;
using AnimationScript = Scripts.AnimationScript;

public class PlayerController : MonoBehaviour
{
    // System
    private Rigidbody2D _rb;
    private Animator Anim;
    private Collider2D _col;
    private Health _health;
    
    //FSM
    private enum State
    {
         Idle, Walking, Spawning, OnAir
    }

    [SerializeField] private State state = State.Spawning;
    // Other state
    /// <summary>
    /// How much have the player jumped
    /// </summary>
    private int jumped = 0;
    /// <summary>
    /// For coyote time, how long the player can still jump after leaving the ground
    /// </summary>
    private int hangTimeCountdown;
    /// <summary>
    /// If the player is on the ground or nah
    /// </summary>
    public bool onGround;
    /// <summary>
    /// If the player is currently animating
    /// </summary>
    public bool animating;
    /// <summary>
    /// Timer to check whether to stop checking the player if they are on the ground
    /// </summary>
    private int stopGroundCheck;
    /// <summary>
    /// Player's initial gravity scale
    /// </summary>
    private float initGravScale;
    /// <summary>
    /// The direction that the player is facing
    /// </summary>
    [HideInInspector] public int direction = 1;

    // Keyboard input
    private PlayerInput _input;
    
    private InputAction _jumpAction;
    private InputAction _abilityAction;
    private InputAction _switchChar1;
    private InputAction _switchChar2;
    private InputAction _switchChar3;
    private InputAction _switchChar4;
    private InputAction _moveAction;
    private InputAction _togglePause;
    
    [Header("Player's properties")]
    public PlayerStats _playerStats;

    public float wishVel_x;
    private int jmpLimit;
    private int jmpHeight;
    private int djmpHeight;

    [SerializeField] private float fallScale = 2.5f;
    
    /*[Header("Debug Texts")]
    [SerializeField] private TMP_Text txtState;
    [SerializeField] private TMP_Text txtJumped;
    [SerializeField] private TMP_Text txtHangtimeCountdown;
    [SerializeField] private TMP_Text txtHDirection;*/

    [Header("Polish")] 
    [SerializeField] private int timeToStopGroundCheck;
    [SerializeField] private float jmpGravScale;
    [SerializeField] private int hangTime;
    [SerializeField] private float decelAmount;
    [SerializeField] private float accel_x;
    [SerializeField] private float minimumRunSpeed;
    [SerializeField] private float wishJumpExpirationDuration;
    
    [Header("Hitstun")]
    public float hitStunTime = 1f;
    public float hitStunForce = 10f;

    // Key input
    public int hDirection;
    
    // Other script
    private Collision _sColl; 
    private AnimationScript _sAnim;
    private PlayerLoader _sLoader;
    
    // Active players
    private GameObject _activePlayer;
    public int activePlayerID;
    private IPlayerAction _activePlayerAction;

    public Action OnGrounded;
    private bool _isGrounded;
    public Action OnFirstJump;
    
    // Health
    [Header("Sound effects")]
    [SerializeField] private PlaySoundOnce SpawnSFX;
    [SerializeField] private PlaySoundOnce JumpSFX;
    [SerializeField] private PlaySoundOnce DjumpSFX;
    [SerializeField] private PlaySoundOnce CancelSFX;
    [SerializeField] private int FrogID;

    private bool wishJump;
    private float wishJumpExpiration;
    
    #region Unity Events
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
        _input = GetComponent<PlayerInput>();
        //_col = GetComponent<BoxCollider2D>();
        _sColl = GetComponent<Collision>();
        _sAnim = GetComponentInChildren<AnimationScript>();
        _sLoader = GetComponent<PlayerLoader>();
        _health = GetComponent<Health>();
        
        stopGroundCheck = 0;
        hangTimeCountdown = hangTime;
        initGravScale = _rb.gravityScale;
        // Check if the character has been loaded (usually yes)
        if(_sLoader.Loaded) FetchPlayerFromLoader();
        // Subscribe to loading event
        // This is the key part to the switching characters feature.
        _sLoader.OnLoaded += FetchPlayerFromLoader;
        
        // Input system
        _jumpAction = _input.actions["Jump"];
        _jumpAction.performed += OnJump;
        /*_abilityAction = _input.actions["action"];
        _abilityAction.performed += OnExecuteMove;*/
        
        _moveAction = _input.actions["Move"];
        _moveAction.started += OnMovement;
        _moveAction.canceled += OnMovement;
        
        _health.OnTakeDamage += OnTakeDamage;
        _health.OnDespawn += OnDespawn;
        //_health.OnRespawn += OnRespawn;

        //_togglePause = _input.actions["pausing"];
        //_togglePause.performed += OnPressingPause;
    }
    
    void Update()
    {
        if(animating) return;
        StateTransition();
        
        _sAnim.SetState((int)state);  
    }
    
    void FixedUpdate()
    {
        // Freeze any movement and ignore inputs
        if (animating) return;

        Falling();

        // Stop the ground collision for a few frames because at the start of the jump the collision check
        // is still at the ground, resetting jump variables while just jumped
        // TODO: is there a better way?
        // UPDATE: this is the way
        if (stopGroundCheck > 0)
        {
            stopGroundCheck--;
        }
        else
        {
            onGround = _sColl.onGround; 
        }

        PerformStateAction(); 
    }

    private void OnDisable()
    {
        _jumpAction.performed -= OnJump;
        _moveAction.started -= OnMovement;
        _moveAction.canceled -= OnMovement;
        _health.OnTakeDamage -= OnTakeDamage;
        _health.OnDespawn -= OnDespawn;
       // _health.OnRespawn -= OnRespawn;
        
        _sLoader.OnLoaded -= FetchPlayerFromLoader;
        //_togglePause.performed -= OnPressingPause;
    }
    
    #endregion

    #region Input Actions
    void OnMovement(InputAction.CallbackContext context)
    {
        if (animating) return;
        Debug.Log("Moving at Direction: " + context.ReadValue<Vector2>());
        hDirection = (int)context.ReadValue<Vector2>().x;
    }
    
    void OnExecuteMove(InputAction.CallbackContext context)
    {
        if (animating) return;

        ExecuteMove();
    }
    
    void OnJump(InputAction.CallbackContext context)
    {
        if (animating) return;
        Jump();
    }
    
    /*
    void OnPressingPause(InputAction.CallbackContext context)
    {
        GameManager.Instance.TogglePause();
    }
    */

    #endregion

    /// <summary>
    /// Take damage event routine
    /// Will performs the take damage animation and knockback
    /// Finishes in *hitStunTime* seconds
    /// </summary>
    private IEnumerator PerformTakeDamage()
    {
        Debug.Log("Player: Taking damage");
        // Change animation
        _sAnim.SetTrigger("damaged");
        
        FreezeMovement();
        hDirection = 0;
        // Knockback
        _rb.velocity = new Vector2(-hitStunForce * direction, hitStunForce);
        // Set timer
        yield return new WaitForSeconds(hitStunTime);
        
        // Reset
        UnfreezeMovement();
    }

    /// <summary>
    /// Event for when OnDespawn is invoked
    /// </summary>
    private void OnDespawn()
    {
        Debug.Log("Despawning");
        // Knockback
        _rb.velocity = new Vector2(-hitStunForce * direction, hitStunForce);
        FreezeMovement();
        _sAnim.SetTrigger("disappearing");
    }

    /// <summary>
    /// Event for when OnRespawn is invoked
    /// Happen after OnDespawn after *respawnTime* seconds
    /// Handled by the health script
    /// </summary>
    ///
    ///  BROKEN DUE TO MISSING GAMEMANAGER
    /*private void OnRespawn()
    {
        Debug.Log("Respawning");
        UnfreezeMovement();
        _rb.velocity = Vector3.zero;
        transform.position = GameManager.Instance.CheckpointLoc;
        TransitionToState(State.Spawning);
    }*/
    
    private void OnTakeDamage()
    {
        StartCoroutine(PerformTakeDamage());
    }

    /// <summary>
    ///  Fetch the player from the loader
    ///  And apply the stats to the current player
    /// </summary>
    void FetchPlayerFromLoader()
    {
        _activePlayer = _sLoader.curCharacter;
        activePlayerID = _sLoader.curCharacterID;
        _activePlayerAction = _activePlayer.GetComponent<IPlayerAction>();

        // Deep copy for ease of prototyping
        // Though I'm not sure if this is a wasteful use of memory...
        // TODO: Check if this is a good idea and how C# handles this
        _playerStats = _sLoader.curPlayerStats.clone();
        
        //_playerStats = _sLoader.curPlayerStats;
        
        jmpLimit = _playerStats.jmpLimit;
        wishVel_x = _playerStats.walkSpeed;
        jmpHeight = _playerStats.jmpHeight;
        djmpHeight = _playerStats.djmpHeight;
        jmpGravScale = _playerStats.jmpGravScale;
        
        Debug.Log("Stats: " +
                  "jmpLimit: " + jmpLimit + "\n" +
                  "wishVel_x: " + wishVel_x + "\n" +
                  "jmpHeight: " + jmpHeight + "\n"
                  );
    }
    
    /// <summary>
    ///  Manage state transitions
    /// </summary>
    void StateTransition()
    {
        if (state == State.Spawning) return;
        if ( ((hDirection == -1 && !_sColl.onLeftWall) ||
             (hDirection == 1  && !_sColl.onRightWall)) && onGround) 
        { TransitionToState(State.Walking);  }
        else if ( ((hDirection == -1 && !_sColl.onLeftWall) ||
                 (hDirection == 1  && !_sColl.onRightWall)) && !onGround) 
            TransitionToState(State.OnAir);
        else TransitionToState(State.Idle);
    }
    
    /// <summary>
    /// Transition to a new state
    /// </summary>
    /// <param name="st">State to transition to</param>
    private void TransitionToState(State st)
    {
        state = st;
        //Debug.Log("Switch to state: " + st.ToString());
    }
    
    /// <summary>
    /// Transition and performs action from that state
    /// </summary>
    private void PerformStateAction()
    {
        switch (state) {
            case State.Spawning:
                StartCoroutine(HandleSpawning());
                break;
            
            case State.Idle:
                // deceleration
                _rb.velocity = Vector2.Lerp(_rb.velocity, new Vector2(0, _rb.velocity.y), decelAmount);
                break;

            case State.Walking:
                GroundMove();
                break;
            
            case State.OnAir:
                AirMove();
                break;
            
            default:
                break;
        }
    }
    
    /// <summary>
    /// Perform character specific action based on the active player
    /// </summary>
    public void ExecuteMove()
    {
        // Execute the action from the current character
        _activePlayerAction.ExecuteMove();
        
        // clean up here
        Debug.Log("PlayerController: Clean up exectuted");
    }

    /// <summary>
    /// Handle the spawning animation
    /// </summary>
    private IEnumerator HandleSpawning()
    {
        Debug.Log("Spawning");
        FreezeMovement();
        //SpawnSFX.PlaySound();
        // Trigger the animation
        _sAnim.SetTrigger("appearing");
        // Wait for animation to finish and then move on to Idle state
        yield return new WaitForSeconds(.5f);

        UnfreezeMovement();
        TransitionToState(State.Idle);
        PerformStateAction();
    }
    
    
    #region Player Actions
    //// MOVEMENT ////
    ///  Falling script
    private void Falling()
    { 
        if (!onGround)
        {
            _isGrounded = false;
            if(_rb.velocity.y >= 0) return; 
            // Falling code

            // COYOTE TIME
            if (hangTimeCountdown == 0) //falling
            {
                if (jumped == 0) jumped = 1;

            } else if (jumped == 0) // Coyote time
            {
                hangTimeCountdown--;
            }
            _sAnim.SetBool("falling", true);
            _rb.velocity +=
                Vector2.up * Physics2D.gravity.y * (fallScale - 1) * Time.deltaTime; // falling acceleration

        }
        else
        {
            // Reset jump variables
            if(!_isGrounded) OnGrounded?.Invoke();
            _isGrounded = true;
            hangTimeCountdown = hangTime;
            jumped = 0;
            _sAnim.SetBool("falling", false);
            if (wishJump && Time.time < wishJumpExpiration)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        // Jump queuing
        if (jumped >= jmpLimit)
        {
            wishJump = true;
            wishJumpExpiration = Time.time + wishJumpExpirationDuration;
            return;
        }

        wishJump = false;
        //_activePlayerAction.ExecuteJump();
        
        if(jumped == 1)
        {
            // Double jump
            //DjumpSFX.PlaySound();
            _rb.velocity = new Vector2(_rb.velocity.x, djmpHeight);
            _sAnim.SetTrigger("djumped");
            
        } else
        {
            // Normal jump
            //if(activePlayerID != 1)JumpSFX.PlaySound();
            _rb.velocity = new Vector2(_rb.velocity.x, jmpHeight);
            _sAnim.SetTrigger("jumped");
            OnFirstJump?.Invoke();
        }
        Debug.Log("jumped");
        jumped++;
        _sAnim.SetBool("falling", false);
        _rb.gravityScale *= jmpGravScale;

        stopGroundCheck = timeToStopGroundCheck;
        onGround = false;
        _rb.gravityScale = initGravScale;
        hangTimeCountdown = 0;
    }

    private void GroundMove()
    {
        if (hDirection == 0) return;
        float currentVel_x = _rb.velocity.x;
        // Adhere to direction the player is facing
        float wishVel_x_dir = wishVel_x * hDirection;
        // If the difference is small enough just set it to the wish velocity
        currentVel_x = Mathf.Abs(wishVel_x_dir - currentVel_x) <= 0.5f
            ? wishVel_x_dir
            : Mathf.Lerp(currentVel_x, wishVel_x_dir, accel_x); // Acceleration
        
        // Set minimum speed
        if( Mathf.Abs(currentVel_x) < minimumRunSpeed ) currentVel_x = hDirection * minimumRunSpeed;
        _rb.velocity = new Vector2(currentVel_x, _rb.velocity.y);
        
        direction = hDirection;
        transform.localScale = new Vector2(direction * 2, 2);  // forgive me
    }

    private void AirMove()
    {
        // This will make it easier to create hard mid air turns
        // To do this, velocity applied at the end will be the scalar times the direction of the key
        if (hDirection == 0) return;
        float currentVel_x = _rb.velocity.x * -hDirection; // Mathf.Abs()
        // Adhere to air scale
        float wishVel_x_air = wishVel_x * 0.75f;
        // If the difference is small enough just set it to the wish velocity
        currentVel_x = currentVel_x - wishVel_x_air <= 0.5f
            ? wishVel_x_air
            : Mathf.Lerp(currentVel_x, wishVel_x_air, accel_x); // Acceleration
        
        // Set minimum speed
        if( currentVel_x < minimumRunSpeed ) currentVel_x =  minimumRunSpeed;
        _rb.velocity = new Vector2(currentVel_x * hDirection, _rb.velocity.y);

        direction = hDirection;
        transform.localScale = new Vector2(direction * 2, 2);  // forgive me
    }
    
    #endregion

    /// <summary>
    /// Freeze all movement and animation
    /// </summary>
    private void FreezeMovement()
    {
        animating = true;
        _rb.velocity = Vector3.zero;
        _rb.gravityScale = 0;
        _sAnim.SetAnimating(animating);
    }

    /// <summary>
    /// Undo FreezeMovement()
    /// </summary>
    private void UnfreezeMovement()
    {
        _rb.gravityScale = initGravScale;
        animating = false;
        _sAnim.SetAnimating(animating);
    }
}