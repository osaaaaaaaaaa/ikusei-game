using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject breakingEffect;
    [SerializeField] string colorString;
    Color createColor;

    MiniGameManager2 manager;
    float speed;
    bool isInit = false;
    bool isMonsterHit = false;

    private void Start()
    {
        // êFÇçÏê¨
        ColorUtility.TryParseHtmlString(colorString, out createColor);
    }

    private void Update()
    {
        if (!isInit) return;
        if(manager.isGameOver || manager.isGameClear)
        {
            GetComponent<PolygonCollider2D>().enabled = false;
        }

        transform.Translate(new Vector2(-1 * speed, 0f) * Time.deltaTime, Space.Self);

        if (transform.position.x < -10) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMonsterHit && !manager.IsInvincible && collision.transform.tag == "Monster")
        {
            isMonsterHit = true;
            collision.transform.position += Vector3.left * 0.2f;
            manager.HitMonster(1);
        }
        if(collision.transform.tag == "Rock")
        {
            var effect = Instantiate(breakingEffect);
            ParticleSystem.MainModule psmain = effect.GetComponent<ParticleSystem>().main;
            effect.transform.position = this.transform.position;
            psmain.startColor = createColor;
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
