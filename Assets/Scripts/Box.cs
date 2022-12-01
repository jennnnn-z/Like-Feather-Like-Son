using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Rigidbody2D boxRB;
    // Start is called before the first frame update
    void Start()
    {
        boxRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Slide(float power, Vector2 direction)
    {
        boxRB.AddForce(direction * power, ForceMode2D.Impulse);
    }
}
