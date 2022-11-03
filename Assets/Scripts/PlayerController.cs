using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4;
    private float acceleration;
    private Rigidbody2D playerRb;

    public float horizontalInput;
    public float verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        acceleration = speed * 2;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        float zDegrees = Vector2.SignedAngle(new Vector2(0, 1), new Vector2(horizontalInput, verticalInput));
        Debug.Log(zDegrees);

        if (Mathf.Abs(verticalInput) >= 0.5f || Mathf.Abs(horizontalInput) >= 0.5f)
        {
            transform.eulerAngles = new Vector3(0, 0, zDegrees);
            playerRb.velocity = transform.up*speed;
        }
        else
        {
            playerRb.velocity *= 0;
        }
    }
}
