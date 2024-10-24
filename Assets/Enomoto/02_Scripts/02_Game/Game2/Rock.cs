using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Trigger")
        {
            GetComponent<Rigidbody2D>().gravityScale = 2;
        }
    }
}
