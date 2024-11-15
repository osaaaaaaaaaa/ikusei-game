using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Rock")
        {
            collision.transform.GetComponent<Rigidbody2D>().gravityScale = 2;
            // constraintsの制限解除
            collision.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
        if (collision.tag == "Monster")
        {
            // constraintsを回転を固定に上書き
            collision.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            // constraintsを回転とX軸移動を固定に上書き
            collision.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
