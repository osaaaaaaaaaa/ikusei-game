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
            // constraints�̐�������
            collision.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
        if (collision.tag == "Monster")
        {
            // constraints����]���Œ�ɏ㏑��
            collision.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            // constraints����]��X���ړ����Œ�ɏ㏑��
            collision.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
