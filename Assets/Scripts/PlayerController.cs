using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4;
    private float jumpForce = 4;
    private float grav = 8f;
    private float acceleration;
    public float vertVel;
    private Rigidbody2D playerRb;

    public float horizontalInput;
    public float verticalInput;
    public KeyCode jump = KeyCode.Space;

    public MoveState playerMove;
    public AirState playerAir;
    public AttackState playerAttack;

    public Hitbox beakBox;

    private float jumpTime;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        acceleration = speed * 2;
        playerMove = MoveState.Stationary;
        playerAir = AirState.Grounded;
        jumpTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /* Movement State*/
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        float zDegrees = Vector2.SignedAngle(new Vector2(0, 1), new Vector2(horizontalInput, verticalInput));

        if (Mathf.Abs(verticalInput) >= 0.5f || Mathf.Abs(horizontalInput) >= 0.5f)
        {
            transform.eulerAngles = new Vector3(0, 0, zDegrees);
            playerRb.velocity += new Vector2(transform.up.x, transform.up.y).normalized * speed;

            if (playerRb.velocity.magnitude > speed)
            {
                playerRb.velocity *= speed / playerRb.velocity.magnitude;
            }

            playerMove = MoveState.Moving;
        }
        else
        {
            if (playerMove == MoveState.Moving)
            {
                playerRb.velocity -= new Vector2(transform.up.x, transform.up.y).normalized * speed;
                playerMove = MoveState.Stationary;
            }
        }

        /*Airborne State*/
        if (playerMove == MoveState.Stationary && playerRb.velocity.magnitude > 0.00000000001f) playerRb.velocity *= 0;

        if (playerAir == AirState.Grounded)
        {
            vertVel = 0;
            if (Input.GetKeyDown(jump))
            {
                Debug.Log("In");
                vertVel = jumpForce;
                playerAir = AirState.Rising;
            }
        }
        else if (playerAir == AirState.Rising)
        {
            vertVel -= grav * Time.deltaTime;
            if (vertVel < 0)
            {
                playerAir = AirState.Airborne;
            }
        }
        else if (playerAir == AirState.Airborne || playerAir == AirState.Gliding)
        {
            if (Input.GetKey(jump))
            {
                vertVel -= (grav / 2) * Time.deltaTime;
                playerAir = AirState.Gliding;
            }
            else
            {
                vertVel -= grav * Time.deltaTime;
                playerAir = AirState.Airborne;
            }

            if (vertVel < -jumpForce)
            {
                vertVel = 0;
                playerAir = AirState.Grounded;
            }
        }

        transform.Translate(0, vertVel * Time.deltaTime, 0, Space.World);

        /* Hitbox State */
        
    }

    public enum MoveState
    {
        Stationary,
        Moving
    }

    public enum AirState
    {
        Grounded,
        Rising,
        Airborne,
        Gliding
    }

    public enum AttackState
    {
        Startup,
        Active,
        Cooldown
    }

    //Layer Trigger should also set state to grounded.
    //TODO: On collision with box or ball, if higher, set state to grounded
}
