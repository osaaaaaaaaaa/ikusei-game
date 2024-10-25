using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] GameObject typeMonsterImagePrefab;
    [SerializeField] List<GameObject> monsterPrefabs;
    GameObject monster;

    // インスタンス作成
    static MonsterController instance;
    public static MonsterController Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// モンスター生成処理
    /// </summary>
    public GameObject GenerateMonster(Vector2 generatePoint)
    {
        if (monster != null)
        {
            Destroy(monster.gameObject);
        }

        monster = Instantiate(monsterPrefabs[1]);
        monster.transform.localPosition = generatePoint;
        return monster;
    }

    /// <summary>
    /// モンスターのコライダーを返す
    /// </summary>
    /// <returns></returns>
    public PolygonCollider2D GetCollider()
    {
        return monster.GetComponent<PolygonCollider2D>() ?? null;
    }

    /// <summary>
    /// アニメーション再生開始
    /// </summary>
    public void PlayStartAnim()
    {
        monster.GetComponent<Animator>().enabled = true;
    }

    /// <summary>
    /// ジャンプアニメーション再生
    /// </summary>
    public void PlayJumpAnim()
    {
        monster.GetComponent<Animator>().Play("MonsterJump");
    }

    /// <summary>
    /// モンスターが倒れるアニメーション
    /// </summary>
    public void PlayDeathAnimMonster()
    {
        monster.GetComponent<PolygonCollider2D>().enabled = false;
        var rb2D = monster.GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 3;
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
        rb2D.AddForce(Vector2.up * 14, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 進化するアニメーション再生
    /// </summary>
    public void PlayEvolutionAnim()
    {
        GameObject effect = Instantiate(typeMonsterImagePrefab);
        effect.transform.position = monster.transform.position;
    }

    /// <summary>
    /// 死亡するアニメーション再生
    /// </summary>
    public void PlayKillAnim()
    {

    }
}
