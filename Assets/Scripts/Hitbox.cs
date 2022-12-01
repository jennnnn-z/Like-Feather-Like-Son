using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float power = 4;
    public PlayerController source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D targetCollider)
    {
        Vector2 direction = (targetCollider.gameObject.transform.position - source.gameObject.transform.position).normalized;
        if (targetCollider.gameObject.GetComponent<Box>() != null) {
            targetCollider.gameObject.GetComponent<Box>().Slide(power, direction);
        }
        else if (targetCollider.gameObject.GetComponent<Ball>() != null) {
            targetCollider.gameObject.GetComponent<Ball>().Roll(power, direction);
        }
    }
}
