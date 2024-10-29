using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFood : MonoBehaviour
{
    [SerializeField] GameObject eatingEffect;
    [SerializeField] string colorString;
    Color createColor;
    GrowManager mealManager;
    public GrowManager MealManager { set { mealManager = value; } }
    bool isThrow;
    const float dragSpeed = 100;

    private void Awake()
    {
        isThrow = false;

        // 色を作成
        ColorUtility.TryParseHtmlString(colorString, out createColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (!mealManager.isDragFood) return;
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
            mealManager.GenerateFood();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isThrow && collision.tag == "Monster")
        {
            // 食べるエフェクトを発生させる
            var effect = Instantiate(eatingEffect);
            ParticleSystem.MainModule psmain = effect.GetComponent<ParticleSystem>().main;
            effect.transform.position = new Vector3(transform.position.x,transform.position.y,-8f);
            psmain.startColor = createColor;
            effect.GetComponent<ParticleSystem>().Play();

            mealManager.AddHungerAmount();
            mealManager.GenerateFood();

            Destroy(this.gameObject);
        }
    }
}