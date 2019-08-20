﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float horizontalMove;
    private float verticalMove;
    private float actuellSpeedHorizontal;
    private float actuellSpeedVertical;
    private float accelerationHorizonatl;
    private float accelerationVertical;

    [Header("Speed for the Movement")]
    [SerializeField] float movementSpeed;
    [SerializeField] float acceleration;


    [Header("Settings for the dash")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] GameObject dashParticlesPrefab;

    Vector3 heading;
    Vector3 dashdirection;
    Vector3 moveDirection;

    bool dashing = false;
    bool dash = false;

    public static bool show = false;
    public static bool attack = false;
    public static bool dancing = false;
    public static bool attack1DONE;

    private PlayerController DeadDisable;
    private Respawn respawn;
    
    Animator anim;
    Rigidbody rigi;
    public Timer dashTimer = new Timer();

    [Header("Life setting")]
    public int life = 3;

    AudioSource my_audioSource; 

    [Header("Several Audiofiles")]
    public AudioClip dashClip;
    public AudioClip slashClip;

    //gibt an, welcher Dancemove abgespielt werden soll
    private int dancemove;
    
    void Start()
    {
        respawn = GameObject.FindWithTag("Respawn").GetComponent<Respawn>();

        dashing = false;
        dash = false;
        show = false;
        attack = false;
        dancing = false;
        attack1DONE = false;

        dashTimer.Start(dashTime);

        dancemove = 0;

        rigi = gameObject.GetComponent<Rigidbody>();

        anim = gameObject.GetComponent<Animator>();

        DeadDisable = gameObject.GetComponent<PlayerController>();

        my_audioSource = GetComponent<AudioSource>(); 

    }

    void Update()
    {

        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        anim.SetFloat("PosX", horizontalMove);
        anim.SetFloat("PosY", verticalMove);

        if(Input.GetButtonDown("Attack") && !attack && !dancing && life > 0)
        {
            if (attack1DONE == false)
            {
                anim.Play("Attack", 0, 0);
                attack = true;
                my_audioSource.clip = slashClip;
                my_audioSource.Play();
                attack1DONE = true;
            }
            else 
            {
                anim.Play("Attack2", 0, 0);
                attack = true;
                my_audioSource.clip = slashClip;
                my_audioSource.Play();
                attack1DONE = false;
            }
        }
        if(!dash && Input.GetButtonDown("Dash") && !dancing)
        {
            anim.Play("Dashing");
            my_audioSource.clip = dashClip; 
            my_audioSource.Play(); 
            dash = true;
        }
        if(Input.GetButtonDown("Dance") && !dancing && !attack && !dash)
        {

            if (dancemove == 0)
            {
                anim.SetBool("dance", true);
                dancing = true;
                dancemove = 1;
            }
            else if (dancemove == 1)
            {
                anim.SetBool("dance2", true);
                dancing = true;
                dancemove = 2;
            }
            else
            {
                anim.SetBool("dance3", true);
                dancing = true;
                dancemove = 0;
            }

        }
        if(life <= 0)
        {
                anim.Play("Death");
                DeadDisable.enabled = false;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(!show)
            {
                show = true;
            }
            else if(show)
            {
                show = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if(!IsGrounded.isGrounded)
        {
            Gravity();
        }
        if (!dashing && !dancing && IsGrounded.isGrounded)
        {
            Move();
        }
        if ((Input.GetAxisRaw("Horizontal") >= 0.1 ||
             Input.GetAxisRaw("Horizontal") <= -0.1 ||
             Input.GetAxisRaw("Vertical") >= 0.1 ||
             Input.GetAxisRaw("Vertical") <= -0.1) &&
             !dashing &&
             !dancing)
        {
            heading = new Vector3(Input.GetAxisRaw("Horizontal"),
                                  0,
                                  Input.GetAxisRaw("Vertical"));
            anim.SetBool("running", true);
            Turn();
        }
        else
        {
            anim.SetBool("running", false);
        }        
        Dashing();
    }

    /// <summary>
    /// Function for the Movement
    /// </summary>
    void Move()
    {

        actuellSpeedHorizontal = acceleration * horizontalMove;
        actuellSpeedVertical = acceleration * verticalMove;


        if (actuellSpeedHorizontal >= movementSpeed)
        {
            actuellSpeedHorizontal = movementSpeed;
        }
        else if (actuellSpeedHorizontal <= -movementSpeed)
        {
            actuellSpeedHorizontal = -movementSpeed;
        }
        if (actuellSpeedVertical >= movementSpeed)
        {
            actuellSpeedVertical = movementSpeed;
        }
        else if (actuellSpeedVertical <= -movementSpeed)
        {
            actuellSpeedVertical = -movementSpeed;
        }

        rigi.velocity = new Vector3(actuellSpeedHorizontal,
                                    0,
                                    actuellSpeedVertical);
        
    }
    void Gravity()
    {
        rigi.velocity = new Vector3(actuellSpeedHorizontal,
                                    -10,
                                    actuellSpeedVertical);
    }
    /// <summary>
    /// Function for dashing in the direction you are facing
    /// </summary>
    void Dashing()
    {
        if (dashing == false)
        {
            if (dash)
            {
                dashdirection = heading;
                dashing = true;
                attack = false;
                SpawnDashParticles();
            }
        }
        else
        {
            if(dashTimer.timeCurrent <= 0)
            {
                dashing = false;
                rigi.velocity = Vector3.zero;
                dashTimer.ResetTimer();
                dash = false;
            }
            else
            {
                dashTimer.Tick();
                rigi.velocity += dashdirection * dashSpeed;
            }
        }
    }
    /// <summary>
    /// Function to set you face direction
    /// </summary>
    void Turn()
    {
        transform.rotation = Quaternion.LookRotation(heading);
    }
    public void AfterAttack()
    {
        attack = false;
        
        BeatStrike.beatAttack = false;
    }
    public void AfterDancing()
    {        
        dancing = false;
        anim.SetBool("dance", false);
        anim.SetBool("dance2", false);
        anim.SetBool("dance3", false);
    }
    public void AfterDash()
    {
        dash = false;
    }
    public void afterdeath()
    {
        respawn.RespawnPlayer();
        life = 3;
        anim.Play("respawn");        
    }
    public void afterrespawn()
    {
        anim.SetBool("backtoidle", true);
        DeadDisable.enabled = true;
    }

    private void SpawnDashParticles()
    {
        Debug.Log("Spawning Dash Particles");
        GameObject particlesInstance = Instantiate(dashParticlesPrefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        particlesInstance.GetComponent<FollowPosition>().followTarget = gameObject.transform;
        particlesInstance.gameObject.transform.Rotate(-90,0,0);
        ParticleSystem parts = particlesInstance.GetComponentInChildren<ParticleSystem>();
        float totalDuration = parts.main.duration + parts.main.startLifetime.constant + parts.main.startDelay.constant;
        Destroy(particlesInstance, totalDuration);
    }
}