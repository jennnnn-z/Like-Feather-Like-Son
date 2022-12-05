using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D ballRB;
    // Start is called before the first frame update
    void Start()
    {
        ballRB = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Roll (float power,Vector2 direction)
    {
        ballRB.AddForce(direction * power * ballRB.mass, ForceMode2D.Impulse);
    }
}
