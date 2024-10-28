using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] Sprite spritePoop;
    [SerializeField] GameObject changeParticle;

    [SerializeField] float speed;
    Rigidbody2D rb2d;

    public enum FOOD_ID
    {
        Meat = 1,
        Apple,
        Banana,
        Lettuce,
        Poop = 10
    }

    [SerializeField] int foodID;
    public int FoodID { get { return foodID; } }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(speed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "DeathZone")
        {
            Destroy(this.gameObject);
        }
        else if(collision.tag == "Trigger")
        {
            int rnd = Random.Range(1, 11);
            if (rnd < 3) ObjectToPoop();
        }
    }

    /// <summary>
    /// 自身をpoopに変換する
    /// </summary>
    void ObjectToPoop()
    {
        // パーティクルを生成する
        var particle = Instantiate(changeParticle,this.transform);
        particle.transform.position = this.transform.position;

        //-------------------------------------------------------
        // 動的にPolygonCollider2Dをスプライトの形に変形させる
        //-------------------------------------------------------
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        GetComponent<SpriteRenderer>().sprite = spritePoop;
        var polygonCollider2D = GetComponent<PolygonCollider2D>();
        var physicsShapeCount = spritePoop.GetPhysicsShapeCount();

        polygonCollider2D.pathCount = physicsShapeCount;

        var physicsShape = new List<Vector2>();

        for (var i = 0; i < physicsShapeCount; i++)
        {
            physicsShape.Clear();
            spritePoop.GetPhysicsShape(i, physicsShape);
            var points = physicsShape.ToArray();
            polygonCollider2D.SetPath(i, points);
        }

        foodID = 10;
    }
}
