using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;
using KanKikuchi.AudioManager;

public class MonsterController : MonoBehaviour
{
    #region �i���֌W
    [SerializeField] GameObject lightEffectPrefab;
    [SerializeField] GameObject areaEffectPrefab;
    GameObject areaEffect;
    #endregion

    #region ���S�֌W
    [SerializeField] GameObject rainFallingParticle;
    #endregion

    #region �z���֌W
    [SerializeField] GameObject mixEffectPrefab;
    [SerializeField] GameObject mixDoneEffectPrefab;
    #endregion

    #region �z���֌W
    [SerializeField] GameObject hachingHitEffect;
    [SerializeField] GameObject hachingGlowEffect;
    [SerializeField] List<Sprite> eggSprites;
    #endregion

    [SerializeField] GameObject typeMonsterImagePrefab;
    [SerializeField] List<Sprite> spriteCenteredPivot;
    [SerializeField] List<GameObject> monsterPrefabs;
    public GameObject monster { get; private set; }
    Transform monsterPoint;

    // �����X�^�[���i���ł��邩�ǂ���
    bool isMonsterEvolution;
    public bool IsMonsterEvolution { get { return isMonsterEvolution; } set { isMonsterEvolution = value; } }

    // �����X�^�[���j������邩�ǂ���
    bool isMonsterDie;
    public bool IsMonsterDie { get { return isMonsterDie; } set { isMonsterDie = value; } }

    // ����A�j���[�V�������Đ������ǂ���
    public bool isSpecialAnim { get; private set; }

    // �C���X�^���X�쐬
    static MonsterController instance;
    public static MonsterController Instance { get { return instance; } }

    // �A�j���[�V����ID
    public enum ANIM_ID
    {
        Idle = 0,       // Idle(Animator)
        Jump,           // �W�����v(Animator)
        Glad,           // �~�j�Q�[���N���A(Animator)
        Fall,           // �|���
        EvolutioinWait, // �i���ҋ@
        Evolutioin,     // �i��
        Die,            // ����
        Mix,            // �z��
        Hatching,       // ������z��(Animator)
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
    /// �����X�^�[��������
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
    /// �����X�^�[�̃R���C�_�[��Ԃ�
    /// </summary>
    /// <returns></returns>
    public PolygonCollider2D GetCollider()
    {
        return monster.GetComponent<PolygonCollider2D>() ?? null;
    }

    /// <summary>
    /// ���݂̃����X�^�[�̃X�v���C�g���s�{�b�g�����S�̂��̂ɕύX����
    /// </summary>
    public void ChangeCenteredPivotSprite()
    {
        monster.GetComponent<SpriteRenderer>().sprite = spriteCenteredPivot[TEST_monsterID];

        // �����X�^�[�̃|�C���g�̈ʒu���X�V����
        float monsterSizeY = monster.GetComponent<SpriteRenderer>().bounds.size.y;
        monsterPoint.transform.position = new Vector2(monster.transform.position.x, monster.transform.position.y + monsterSizeY / 3);
    }

    /// <summary>
    /// �e�A�j���[�V�����Đ�����
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
    /// �A�C�h���A�j���[�V�����Đ�
    /// </summary>
    void PlayIdleAnim()
    {
        if (!monster.GetComponent<Animator>().enabled) monster.GetComponent<Animator>().enabled = true;
        monster.GetComponent<Animator>().Play("MonsterIdle");
    }

    /// <summary>
    /// �W�����v�A�j���[�V�����Đ�
    /// </summary>
    void PlayJumpAnim()
    {
        if(!monster.GetComponent<Animator>().enabled) monster.GetComponent<Animator>().enabled = true;
        monster.GetComponent<Animator>().Play("MonsterJump");
    }

    /// <summary>
    /// �Q�[���N���A���̃A�j���[�V�����Đ�
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
    /// �����X�^�[���|���A�j���[�V����
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
    /// �i���ҋ@��Ԃ̃A�j���[�V�����Đ�
    /// </summary>
    void PlayWaitForEvolutionAnim()
    {
        isMonsterEvolution = true;

        // �����X�^�[�̃X�v���C�g�̍������擾
        float monsterSizeY = monster.GetComponent<SpriteRenderer>().bounds.size.y;

        // �G�t�F�N�g����
        areaEffect = Instantiate(areaEffectPrefab);
        areaEffect.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y + monsterSizeY / 4, -1f);
    }

    /// <summary>
    /// �i������A�j���[�V�����Đ�
    /// </summary>
    void PlayEvolutionAnim()
    {
        if (isSpecialAnim) return;
        isSpecialAnim = true;
        bool isPlaingAnim = monster.GetComponent<Animator>().enabled;
        monster.GetComponent<Animator>().Play("MonsterNone");

        // �����X�^�[�̃X�v���C�g�̍������擾
        float monsterSizeY = monster.GetComponent<SpriteRenderer>().bounds.size.y;

        // �G�t�F�N�g����
        GameObject effect1 = Instantiate(typeMonsterImagePrefab);
        GameObject effect2 = Instantiate(lightEffectPrefab);
        effect1.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y, 5);
        effect1.GetComponent<SpriteRenderer>().sprite = monster.GetComponent<SpriteRenderer>().sprite;
        effect2.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y + monsterSizeY / 4, -1f);

        // �A�j���[�V�����Đ�
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

                // �i������
                StartCoroutine(NetworkManager.Instance.ChangeNurtureMonster(
                    NetworkManager.Instance.monsterList[NetworkManager.Instance.nurtureInfo.MonsterID - 1].EvoID,
                    result =>
                    {
                        Debug.Log("�i���I");
                    }));
            });
    }

    /// <summary>
    /// ���S����A�j���[�V�����Đ�
    /// </summary>
    void PlayDieAnim()
    {
        if (isSpecialAnim) return;
        isSpecialAnim = true;
        monster.GetComponent<Animator>().enabled = false;

        Vector3 monsterPos = monster.transform.position;

        // �J���[�쐬
        string colorString = "#6967FF";
        Color createColor;
        ColorUtility.TryParseHtmlString(colorString, out createColor);

        // �����X�^�[�̏�����Ԑݒ�
        monster.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
        monster.transform.position =
            new Vector3(monster.GetComponent<SpriteRenderer>().bounds.size.y / 4, monster.transform.position.y + monster.GetComponent<SpriteRenderer>().bounds.size.y / 4, 0);
        monster.GetComponent<SpriteRenderer>().color = createColor;

        // �G�t�F�N�g����
        GameObject effect = Instantiate(typeMonsterImagePrefab);
        effect.transform.position = new Vector3(0, monster.transform.position.y, -5);
        effect.GetComponent<SpriteRenderer>().sprite = monster.GetComponent<SpriteRenderer>().sprite;
        effect.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        effect.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        SEManager.Instance.Play(SEPath.DIE1);

        // �A�j���[�V�����Đ�
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

                // ���S�o�^�E���������X�^�[�o�^
                StartCoroutine(NetworkManager.Instance.ChangeState(
                    4,
                    result =>
                    {
                        Debug.Log("���S�o�^");
                        StartCoroutine(NetworkManager.Instance.InitMonsterStore(
                            "name",
                            result =>
                            {
                                Debug.Log("���������X�^�[�o�^");
                                Initiate.Fade("01_TopScene", Color.white, 1.0f);

                                IsMonsterDie = false;
                                isSpecialAnim = false;
                            }));

                    }));
            }));
        sequence.Play();
    }

    /// <summary>
    /// �z���̃A�j���[�V�����Đ�
    /// </summary>
    IEnumerator PlayMixAnim()
    {
        if (isSpecialAnim) yield break;
        isSpecialAnim = true;

        // �����X�^�[�̃X�v���C�g�̍����Ȃǂ��擾
        float monsterSizeY = monster.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 monsterPos = monster.transform.position;
        // �G�t�F�N�g����
        Instantiate(mixEffectPrefab, new Vector3(monsterPos.x, monsterPos.y + monsterSizeY / 4, -1f), Quaternion.identity);

        yield return new WaitForSeconds(6f);

        // �G�t�F�N�g����
        Instantiate(mixDoneEffectPrefab, new Vector3(monsterPos.x, monsterPos.y + monsterSizeY / 4, -1f), Quaternion.identity);

        // �琬��Ԃ��u�琬�����v�ɕύX
        StartCoroutine(NetworkManager.Instance.ChangeState(
         3,
         result =>
         {
             Debug.Log("�琬����");
             StartCoroutine(NetworkManager.Instance.MixMiracle(
                result =>
                {
                    if (result)
                    {
                        Debug.Log("�z������");
                        Initiate.DoneFading();
                        Initiate.Fade("01_TopScene", Color.white, 1.0f);
                    }
                }));
         }));


        yield return new WaitForSeconds(5f);
        isSpecialAnim = false;
    }

    /// <summary>
    /// ������z������A�j���[�V����
    /// </summary>
    IEnumerator PlayHatchingAnim()
    {
        if (isSpecialAnim) yield break;
        isSpecialAnim = true;

        // �����X�^�[�̃X�v���C�g�̍����Ȃǂ��擾
        float monsterSizeY = monster.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 monsterPos = monster.transform.position;

        // �Î~������
        monster.GetComponent<Animator>().Play("MonsterNone");

        // ���X�ɂЂт�����
        for (int i = 0; i < eggSprites.Count; i++)
        {
            yield return new WaitForSeconds(1f);
            // �X�v���C�g��ύX���ėh�炷
            monster.GetComponent<SpriteRenderer>().sprite = eggSprites[i];
            monster.GetComponent<Animator>().Play("EggShake");
            Instantiate(hachingHitEffect, new Vector3(monsterPos.x, monsterPos.y + monsterSizeY / 4, -1f), Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);
        monster.GetComponent<Animator>().Play("EggReserveHaching");

        yield return new WaitForSeconds(0.5f);
        // �z������Ƃ��̃G�t�F�N�g����
        var effect = Instantiate(hachingGlowEffect, new Vector3(monsterPos.x, monster.transform.position.y + monsterSizeY / 4, -1f), Quaternion.identity);
        effect.transform.DOScale(new Vector3(10f, 10f, 10f), 2f).SetEase(Ease.OutSine)
            .OnComplete(()=> {
                isSpecialAnim = false;
            });

        yield return new WaitForSeconds(3.2f);
        // �z�����郂���X�^�[��V������������
        GenerateMonster(1, monsterPoint);
        monster.GetComponent<Rigidbody2D>().gravityScale = 0;
        // �z�������Ƃ��̃A�j���[�V�����Đ�
        monster.GetComponent<Animator>().enabled = true;
        monster.GetComponent<Animator>().Play("EggHaching");

        yield return new WaitForSeconds(1f);
        PlayMonsterAnim(ANIM_ID.Idle);
        isSpecialAnim = false;
    }
}
