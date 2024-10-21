using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    MiniGameManager2 manager;
    bool isInit = false;
    float speed;

    private void Update()
    {
        if (!isInit) return;
        if(manager.isGameEnd)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            return;
        }

        transform.Translate(new Vector2(-1 * speed, 0f) * Time.deltaTime, Space.Self);

        if (transform.position.x < -10) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (manager.IsInvincible) return;

        if (collision.transform.tag == "Monster")
        {
            collision.transform.position += Vector3.left * 0.5f;
            GetComponent<BoxCollider2D>().enabled = false;
            manager.HitMonster();
        }
    }

    public void Init(MiniGameManager2 _manager,float _speed)
    {
        manager = _manager;
        speed = _speed;
        isInit = true;
    }
}
