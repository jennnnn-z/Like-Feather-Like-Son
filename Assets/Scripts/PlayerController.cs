using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    [Header("Vertical Movement")]
    [SerializeField] private float grav;
    [SerializeField] private float vertVel;
    [SerializeField] private float height = 0;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;

    [Header("Objects")]
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private Hitbox beakBox;

    [Header("Player States")]
    [SerializeField] private MoveState playerMove;
    [SerializeField] private AirState playerAir;
    [SerializeField] private AttackState playerAttack;

    [Header("Beak Frame Data")]
    [SerializeField] private float startupTime = 0.05f;
    [SerializeField] private float activeTime = 0.1f;
    [SerializeField] private float cooldownTime = 0.4f;
    [SerializeField] private float actionTime = 0;

    [Header("Animation")]
    [SerializeField] private Sprite idle;
    [SerializeField] private Sprite[] walking;
    [SerializeField] private Sprite[] jumping;
    private float direction = 1; //flip between 1 and -1

    /* Keys */
    private KeyCode jump = KeyCode.Space;
    private KeyCode poke = KeyCode.K;
    private Rigidbody2D playerRb;
    private float horizontalInput;
    private float verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerMove = MoveState.Stationary;
        playerAir = AirState.Grounded;
        playerAttack = AttackState.Neutral;
        jumpTime = 0;
        gameObject.layer = 6;
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
            spriteObject.transform.localEulerAngles = new Vector3(0, 0, zDegrees * -1);
            Debug.Log(zDegrees * -1);
            playerRb.velocity += new Vector2(transform.up.x, transform.up.y).normalized * speed;

            if (playerRb.velocity.magnitude > speed)
            {
                playerRb.velocity *= speed / playerRb.velocity.magnitude;
            }
            if (horizontalInput >= 0.5f) {
                direction = 1;
                spriteObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (horizontalInput <= -0.5f) {
                direction = -1;
                spriteObject.GetComponent<SpriteRenderer>().flipX = true;
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
        if (playerMove == MoveState.Stationary && playerRb.velocity.magnitude > 0.00000000000001f) playerRb.velocity *= 0;

        /*Airborne State*/


        if (playerAir == AirState.Grounded)
        {
            vertVel = 0;
            if (Input.GetKeyDown(jump))
            {
                Debug.Log("In");
                vertVel = jumpForce;
                playerAir = AirState.Rising;
                //gameObject.layer++;
                //foreach (Transform child in transform)
                //{
                //    child.gameObject.layer++;
                //}
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
                //gameObject.layer--;
                //foreach (Transform child in transform)
                //{
                //    child.gameObject.layer--;
                //}
            }
        }

        transform.Translate(0, vertVel * Time.deltaTime, 0, Space.World);
        height += vertVel;
        //If height falls below a layer defined threshold, lower my layer and check the area right below me
            //If the layer is lower than my target keep falling
            //Otherwise, land.


        /* Hitbox State */
        switch (playerAttack) {
            case AttackState.Neutral:
                if (Input.GetKeyDown(poke)) {
                    playerAttack = AttackState.Startup;
                    actionTime += Time.deltaTime;
                }
                break;
            case AttackState.Startup:
                if (actionTime >= startupTime) {
                    playerAttack = AttackState.Active;
                    beakBox.gameObject.SetActive(true);
                    actionTime = 0;
                }
                actionTime += Time.deltaTime;
                break;
            case AttackState.Active:
                if (actionTime >= activeTime)
                {
                    playerAttack = AttackState.Cooldown;
                    beakBox.gameObject.SetActive(false);
                    actionTime = 0;
                }
                actionTime += Time.deltaTime;
                break;
            case AttackState.Cooldown:
                if (actionTime >= cooldownTime)
                {
                    playerAttack = AttackState.Neutral;
                    actionTime = 0;
                }
                actionTime += Time.deltaTime;
                break;
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

    public enum AttackState
    {
        Neutral,
        Startup,
        Active,
        Cooldown
    }

    //Layer Trigger should also set state to grounded.
    //TODO: On collision with box or ball, if higher, set state to grounded
}
