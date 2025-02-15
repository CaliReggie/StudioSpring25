using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Player_shem : MonoBehaviour
{

    [Header("Inscribed")]
    public float speed = 3.5f;

    //[Header("Dynamic")]

    public Rigidbody2D rb;
    //public Animator anim;

    //private ThirdPersonActionAsset playerActionsAsset;
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction move;

    private void Awake()
    {
        //playerActionsAsset = new ThirdPersonActionAsset();
        inputAsset = gameObject.GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
    }

    private void OnEnable()
    {
        move = player.FindAction("Move");
        player.Enable();
    }

    private void OnDisable()
    {
        player.Disable();
    }

    void Start()
    {
        //StartCoroutine(nameof(Stamina));
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        //anim.speed = 1;
        rb.velocity = new Vector3(0, 0, 0);
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }


    //Shifted movement to separate function to clean up Update
    void Move()
    {
        float horizontal = move.ReadValue<Vector2>().x;

        if (horizontal == 0)
        {
            rb.velocity = new Vector3(0, 0, 0);
            //anim.SetInteger("Move", 0);
            speed = 3.5f;
            return;
        }

        //moving
        Vector3 vel = rb.velocity;
        Vector3 pos = gameObject.transform.position;
        if (horizontal != 0)
        {
            vel.x = horizontal * speed;
        }

        rb.velocity = vel;
        gameObject.transform.position = pos;

        //Animation handling
        /*if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
        {
            if (vertical < 0)
            {
                anim.SetInteger("Move", 1);
            }
            else if (vertical > 0)
            {
                anim.SetInteger("Move", 3);
            }
        }
        else if (Mathf.Abs(horizontal) >= Mathf.Abs(vertical))
        {
            if (horizontal < 0)
            {
                anim.SetInteger("Move", 2);
            }
            else if (horizontal > 0)
            {
                anim.SetInteger("Move", 4);
            }
        }*/
    }
}