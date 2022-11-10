using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4;
    private float jumpForce = 6;
    private float grav = 1f;
    private float acceleration;
    private Rigidbody2D playerRb;

    public float horizontalInput;
    public float verticalInput;
    public KeyCode jump = KeyCode.Space;

    public MoveState playerMove;
    public AirState playerAir;

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
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        float zDegrees = Vector2.SignedAngle(new Vector2(0, 1), new Vector2(horizontalInput, verticalInput));

        if (Mathf.Abs(verticalInput) >= 0.5f || Mathf.Abs(horizontalInput) >= 0.5f)
        {
            if (playerMove == MoveState.Stationary)
            {
                transform.eulerAngles = new Vector3(0, 0, zDegrees);
                playerRb.velocity += new Vector2(transform.up.x, transform.up.y).normalized * speed;
                playerMove = MoveState.Moving;
            }
        }
        else
        {
            if (playerMove == MoveState.Moving)
            {
                playerRb.velocity -= new Vector2(transform.up.x, transform.up.y).normalized*speed;
                playerMove = MoveState.Stationary;
            }
        }

        if (playerAir == AirState.Grounded)
        {
            playerRb.gravityScale = 0;
            if (Input.GetKeyDown(jump))
            {
                playerRb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                playerRb.gravityScale = grav;
                playerAir = AirState.Rising;
                //Change Layer
            }
        }
        else if (playerAir == AirState.Rising)
        {
            jumpTime += Time.deltaTime;
            Debug.Log(jumpTime);
            if (jumpTime > 1)
            {
                jumpTime = 0;
                playerAir = AirState.Airborne;
            }
        }
        else if (playerAir == AirState.Airborne || playerAir == AirState.Gliding)
        {
            if (Input.GetKey(jump))
            {
                playerRb.gravityScale = grav/2;
                playerAir = AirState.Gliding;
            }
            else
            {
                playerRb.gravityScale = grav;
                playerAir = AirState.Airborne;
            }
        }
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

    //Layer Trigger should also set state to grounded.
}
