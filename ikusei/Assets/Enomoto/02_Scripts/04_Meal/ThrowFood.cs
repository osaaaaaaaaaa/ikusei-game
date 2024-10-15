using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFood : MonoBehaviour
{
    const float dragSpeed = 100;
    bool isThrow;

    private void Awake()
    {
        isThrow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) isThrow = true;
        if (isThrow) return;

        if (Input.GetMouseButton(0))
        {
            // ドラッグ処理
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (worldPos - transform.position).normalized;
            transform.GetComponent<Rigidbody2D>().velocity = dir * dragSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeathZone")
        {
            Debug.Log("不法投棄不法投棄不法投棄");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isThrow && collision.tag == "Monster")
        {
            Debug.Log("たへた");
            Destroy(this.gameObject);
        }
    }
}
