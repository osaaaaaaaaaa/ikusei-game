using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject breakingEffect;
    [SerializeField] string colorString;
    Color createColor;

    MiniGameManager2 manager;
    bool isInit = false;
    bool isMonsterHit = false;
    float speed;

    private void Start()
    {
        // êFÇçÏê¨
        ColorUtility.TryParseHtmlString(colorString, out createColor);
    }

    private void Update()
    {
        if (!isInit) return;
        if(manager.isGameOver)
        {
            GetComponent<PolygonCollider2D>().enabled = false;
            return;
        }

        transform.Translate(new Vector2(-1 * speed, 0f) * Time.deltaTime, Space.Self);

        if (transform.position.x < -10) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMonsterHit && !manager.IsInvincible && collision.transform.tag == "Monster")
        {
            isMonsterHit = true;
            collision.transform.position += Vector3.left * 0.5f;
            manager.HitMonster();
        }
        if(collision.transform.tag == "Rock")
        {
            var effect = Instantiate(breakingEffect);
            effect.transform.position = this.transform.position;
            effect.GetComponent<ParticleSystem>().startColor = createColor;
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(this.gameObject);
        }
    }

    public void Init(MiniGameManager2 _manager,float _speed)
    {
        manager = _manager;
        speed = _speed;
        isInit = true;
    }
}
