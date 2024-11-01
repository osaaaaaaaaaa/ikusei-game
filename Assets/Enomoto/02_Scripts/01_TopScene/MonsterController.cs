using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;
using KanKikuchi.AudioManager;

public class MonsterController : MonoBehaviour
{
    #region 進化関係
    [SerializeField] GameObject lightEffectPrefab;
    [SerializeField] GameObject areaEffectPrefab;
    GameObject areaEffect;
    #endregion

    #region 死亡関係
    [SerializeField] GameObject rainFallingParticle;
    #endregion

    #region 配合関係
    [SerializeField] GameObject mixEffectPrefab;
    [SerializeField] GameObject mixDoneEffectPrefab;
    #endregion

    #region 孵化関係
    [SerializeField] GameObject hachingHitEffect;
    [SerializeField] GameObject hachingGlowEffect;
    [SerializeField] List<Sprite> eggSprites;
    #endregion

    [SerializeField] GameObject typeMonsterImagePrefab;
    [SerializeField] List<Sprite> spriteCenteredPivot;
    [SerializeField] List<GameObject> monsterPrefabs;
    public GameObject monster { get; private set; }
    Transform monsterPoint;

    // モンスターが進化できるかどうか
    bool isMonsterEvolution;
    public bool IsMonsterEvolution { get { return isMonsterEvolution; } set { isMonsterEvolution = value; } }

    // モンスターが破棄されるかどうか
    bool isMonsterDie;
    public bool IsMonsterDie { get { return isMonsterDie; } set { isMonsterDie = value; } }

    // 特殊アニメーションを再生中かどうか
    public bool isSpecialAnim { get; private set; }

    // インスタンス作成
    static MonsterController instance;
    public static MonsterController Instance { get { return instance; } }

    // アニメーションID
    public enum ANIM_ID
    {
        Idle = 0,       // Idle(Animator)
        Jump,           // ジャンプ(Animator)
        Glad,           // ミニゲームクリア(Animator)
        Fall,           // 倒れる
        EvolutioinWait, // 進化待機
        Evolutioin,     // 進化
        Die,            // 死ぬ
        Mix,            // 配合
        Hatching,       // 卵から孵化(Animator)
    }

#if UNITY_EDITOR
    public int TEST_monsterID;
#endif

    private void Awake()
    {
        isSpecialAnim = false;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            IsMonsterDie = false;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// モンスター生成処理
    /// </summary>
    public GameObject GenerateMonster(int monsterID,Transform parentObj)
    {
        if (monster != null)
        {
            Destroy(monster.gameObject);
        }

        monsterPoint = parentObj;
        monster = Instantiate(monsterPrefabs[monsterID], parentObj);
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
    /// 現在のモンスターのスプライトをピボットが中心のものに変更する
    /// </summary>
    public void ChangeCenteredPivotSprite()
    {
        monster.GetComponent<SpriteRenderer>().sprite = spriteCenteredPivot[TEST_monsterID];

        // モンスターのポイントの位置を更新する
        float monsterSizeY = monster.GetComponent<SpriteRenderer>().bounds.size.y;
        monsterPoint.transform.position = new Vector2(monster.transform.position.x, monster.transform.position.y + monsterSizeY / 3);
    }

    /// <summary>
    /// 各アニメーション再生処理
    /// </summary>
    /// <param name="id"></param>
    public void PlayMonsterAnim(ANIM_ID id)
    {
        switch (id)
        {
            case ANIM_ID.Idle:
                PlayIdleAnim();
                break;
            case ANIM_ID.Jump:
                PlayJumpAnim();
                break;
            case ANIM_ID.Glad:
                PlayGladAnim();
                break;
            case ANIM_ID.Fall:
                PlayFallAnimMonster();
                break;
            case ANIM_ID.EvolutioinWait:
                PlayWaitForEvolutionAnim();
                break;
            case ANIM_ID.Evolutioin:
                PlayEvolutionAnim();
                break;
            case ANIM_ID.Die:
                PlayDieAnim();
                break;
            case ANIM_ID.Mix:
                StartCoroutine("PlayMixAnim");
                break;
            case ANIM_ID.Hatching:
                StartCoroutine("PlayHatchingAnim");
                break;
        }
    }

    /// <summary>
    /// アイドルアニメーション再生
    /// </summary>
    void PlayIdleAnim()
    {
        if (!monster.GetComponent<Animator>().enabled) monster.GetComponent<Animator>().enabled = true;
        monster.GetComponent<Animator>().Play("MonsterIdle");
    }

    /// <summary>
    /// ジャンプアニメーション再生
    /// </summary>
    void PlayJumpAnim()
    {
        if(!monster.GetComponent<Animator>().enabled) monster.GetComponent<Animator>().enabled = true;
        monster.GetComponent<Animator>().Play("MonsterJump");
    }

    /// <summary>
    /// ゲームクリア時のアニメーション再生
    /// </summary>
    void PlayGladAnim()
    {
        if (isMonsterDie) return;
        if (!monster.GetComponent<Animator>().enabled) monster.GetComponent<Animator>().enabled = true;

        monster.GetComponent<PolygonCollider2D>().enabled = false;
        monster.GetComponent<Rigidbody2D>().gravityScale = 0;
        monster.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        monster.GetComponent<Animator>().Play("MonsterGlad");
    }

    /// <summary>
    /// モンスターが倒れるアニメーション
    /// </summary>
    void PlayFallAnimMonster()
    {
        monster.GetComponent<PolygonCollider2D>().enabled = false;
        var rb2D = monster.GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 3;
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
        rb2D.AddForce(Vector2.up * 14, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 進化待機状態のアニメーション再生
    /// </summary>
    void PlayWaitForEvolutionAnim()
    {
        isMonsterEvolution = true;

        // モンスターのスプライトの高さを取得
        float monsterSizeY = monster.GetComponent<SpriteRenderer>().bounds.size.y;

        // エフェクト生成
        areaEffect = Instantiate(areaEffectPrefab);
        areaEffect.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y + monsterSizeY / 4, -1f);
    }

    /// <summary>
    /// 進化するアニメーション再生
    /// </summary>
    void PlayEvolutionAnim()
    {
        if (isSpecialAnim) return;
        isSpecialAnim = true;
        bool isPlaingAnim = monster.GetComponent<Animator>().enabled;
        monster.GetComponent<Animator>().Play("MonsterNone");

        // モンスターのスプライトの高さを取得
        float monsterSizeY = monster.GetComponent<SpriteRenderer>().bounds.size.y;

        // エフェクト生成
        GameObject effect1 = Instantiate(typeMonsterImagePrefab);
        GameObject effect2 = Instantiate(lightEffectPrefab);
        effect1.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y, 5);
        effect1.GetComponent<SpriteRenderer>().sprite = monster.GetComponent<SpriteRenderer>().sprite;
        effect2.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y + monsterSizeY / 4, -1f);

        // アニメーション再生
        var sequence = DOTween.Sequence();
        sequence.Append(effect1.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Restart))
            .Join(effect1.GetComponent<SpriteRenderer>().DOFade(0f, 1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Restart))
            .AppendInterval(1f)
            .Append(effect2.transform.DOScale(new Vector3(1f, 1f, 1f), 4f).SetEase(Ease.InBounce))
            .OnComplete(() =>
            {
                monster.GetComponent<Animator>().enabled = isPlaingAnim;
                Destroy(areaEffect.gameObject);
                Destroy(effect1.gameObject);

                isMonsterEvolution = false;
                isSpecialAnim = false;

                GenerateMonster(NetworkManager.Instance.monsterList[NetworkManager.Instance.nurtureInfo.MonsterID - 1].EvoID, monsterPoint);
                monster.GetComponent<Rigidbody2D>().gravityScale = 0;

                // 進化処理
                StartCoroutine(NetworkManager.Instance.ChangeNurtureMonster(
                    NetworkManager.Instance.monsterList[NetworkManager.Instance.nurtureInfo.MonsterID - 1].EvoID,
                    result =>
                    {
                        Debug.Log("進化！");
                    }));
            });
    }

    /// <summary>
    /// 死亡するアニメーション再生
    /// </summary>
    void PlayDieAnim()
    {
        if (isSpecialAnim) return;
        isSpecialAnim = true;
        monster.GetComponent<Animator>().enabled = false;

        Vector3 monsterPos = monster.transform.position;

        // カラー作成
        string colorString = "#6967FF";
        Color createColor;
        ColorUtility.TryParseHtmlString(colorString, out createColor);

        // モンスターの初期状態設定
        monster.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
        monster.transform.position =
            new Vector3(monster.GetComponent<SpriteRenderer>().bounds.size.y / 4, monster.transform.position.y + monster.GetComponent<SpriteRenderer>().bounds.size.y / 4, 0);
        monster.GetComponent<SpriteRenderer>().color = createColor;

        // エフェクト生成
        GameObject effect = Instantiate(typeMonsterImagePrefab);
        effect.transform.position = new Vector3(0, monster.transform.position.y, -5);
        effect.GetComponent<SpriteRenderer>().sprite = monster.GetComponent<SpriteRenderer>().sprite;
        effect.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        effect.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        SEManager.Instance.Play(SEPath.DIE1);

        // アニメーション再生
        var sequence = DOTween.Sequence();
        sequence.Append(monster.transform.DOShakePosition(5f, 0.1f, 15, 1, false, true).SetEase(Ease.Linear))
            .OnComplete(()=> 
            {
                SEManager.Instance.Stop(SEPath.DIE1);
            })
            .AppendInterval(0.5f)
            .Append(effect.GetComponent<SpriteRenderer>().DOFade(0.5f, 0.7f).SetEase(Ease.OutCirc))
            .OnComplete(()=> { SEManager.Instance.Play(SEPath.DIE2); })
            .Append(effect.transform.DOMoveY(1.5f, 3f).SetEase(Ease.OutCirc))
            .Join(effect.GetComponent<SpriteRenderer>().DOFade(0f, 3f).SetEase(Ease.OutCirc)
            .OnComplete(() =>
            {
                Destroy(effect.gameObject);
                Instantiate(rainFallingParticle);

                // 死亡登録・初期モンスター登録
                StartCoroutine(NetworkManager.Instance.ChangeState(
                    4,
                    result =>
                    {
                        Debug.Log("死亡登録");
                        StartCoroutine(NetworkManager.Instance.InitMonsterStore(
                            "name",
                            result =>
                            {
                                Debug.Log("初期モンスター登録");
                                Initiate.Fade("01_TopScene", Color.white, 1.0f);

                                IsMonsterDie = false;
                                isSpecialAnim = false;
                            }));

                    }));
            }));
        sequence.Play();
    }

    /// <summary>
    /// 配合のアニメーション再生
    /// </summary>
    IEnumerator PlayMixAnim()
    {
        if (isSpecialAnim) yield break;
        isSpecialAnim = true;

        // モンスターのスプライトの高さなどを取得
        float monsterSizeY = monster.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 monsterPos = monster.transform.position;
        // エフェクト生成
        Instantiate(mixEffectPrefab, new Vector3(monsterPos.x, monsterPos.y + monsterSizeY / 4, -1f), Quaternion.identity);

        yield return new WaitForSeconds(6f);

        // エフェクト生成
        Instantiate(mixDoneEffectPrefab, new Vector3(monsterPos.x, monsterPos.y + monsterSizeY / 4, -1f), Quaternion.identity);

        // 育成状態を「育成完了」に変更
        StartCoroutine(NetworkManager.Instance.ChangeState(
         3,
         result =>
         {
             Debug.Log("育成完了");
             StartCoroutine(NetworkManager.Instance.MixMiracle(
                result =>
                {
                    if (result)
                    {
                        Debug.Log("配合完了");
                        Initiate.DoneFading();
                        Initiate.Fade("01_TopScene", Color.white, 1.0f);
                    }
                }));
         }));


        yield return new WaitForSeconds(5f);
        isSpecialAnim = false;
    }

    /// <summary>
    /// 卵から孵化するアニメーション
    /// </summary>
    IEnumerator PlayHatchingAnim()
    {
        if (isSpecialAnim) yield break;
        isSpecialAnim = true;

        // モンスターのスプライトの高さなどを取得
        float monsterSizeY = monster.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 monsterPos = monster.transform.position;

        // 静止させる
        monster.GetComponent<Animator>().Play("MonsterNone");

        // 徐々にひびが入る
        for (int i = 0; i < eggSprites.Count; i++)
        {
            yield return new WaitForSeconds(1f);
            // スプライトを変更して揺らす
            monster.GetComponent<SpriteRenderer>().sprite = eggSprites[i];
            monster.GetComponent<Animator>().Play("EggShake");
            Instantiate(hachingHitEffect, new Vector3(monsterPos.x, monsterPos.y + monsterSizeY / 4, -1f), Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);
        monster.GetComponent<Animator>().Play("EggReserveHaching");

        yield return new WaitForSeconds(0.5f);
        // 孵化するときのエフェクト生成
        var effect = Instantiate(hachingGlowEffect, new Vector3(monsterPos.x, monster.transform.position.y + monsterSizeY / 4, -1f), Quaternion.identity);
        effect.transform.DOScale(new Vector3(10f, 10f, 10f), 2f).SetEase(Ease.OutSine)
            .OnComplete(()=> {
                isSpecialAnim = false;
            });

        yield return new WaitForSeconds(3.2f);
        // 孵化するモンスターを新しく生成する
        GenerateMonster(1, monsterPoint);
        monster.GetComponent<Rigidbody2D>().gravityScale = 0;
        // 孵化したときのアニメーション再生
        monster.GetComponent<Animator>().enabled = true;
        monster.GetComponent<Animator>().Play("EggHaching");

        yield return new WaitForSeconds(1f);
        PlayMonsterAnim(ANIM_ID.Idle);
        isSpecialAnim = false;
    }
}
