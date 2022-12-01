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

    public void OnTriggerEnter(Collider targetCollider)
    {
        Vector2 direction = (targetCollider.gameObject.transform.position - source.gameObject.transform.position).normalized;
        if (gameObject.GetComponent<Box>() != null) {
            gameObject.GetComponent<Box>().Slide(power, direction);
        }
        else if (gameObject.GetComponent<Ball>() != null) {
            gameObject.GetComponent<Ball>().Roll(power, direction);
        }
    }
}
