using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;

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

    [SerializeField] GameObject typeMonsterImagePrefab;
    [SerializeField] List<GameObject> monsterPrefabs;
    GameObject monster;

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
        Jump ,          // �W�����v(Animator)
        Fall,           // �|���
        EvolutioinWait, // �i���ҋ@
        Evolutioin,     // �i��
        Die,            // ����
        Mix             // �z��
    }

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
    public GameObject GenerateMonster(Vector2 generatePoint)
    {
        if (monster != null)
        {
            Destroy(monster.gameObject);
        }

        monster = Instantiate(monsterPrefabs[0]);
        monster.transform.localPosition = generatePoint;
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
    /// �e�A�j���[�V�����Đ�����
    /// </summary>
    /// <param name="id"></param>
    public void PlayMonsterAnim(ANIM_ID id)
    {
        switch (id)
        {
            case ANIM_ID.Idle:
                PlayStartAnim();
                break;
            case ANIM_ID.Jump:
                PlayJumpAnim();
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
        }
    }

    /// <summary>
    /// Animator���A�N�e�B�u��
    /// </summary>
    void PlayStartAnim()
    {
        monster.GetComponent<Animator>().enabled = true;
    }

    /// <summary>
    /// �W�����v�A�j���[�V�����Đ�
    /// </summary>
    void PlayJumpAnim()
    {
        monster.GetComponent<Animator>().Play("MonsterJump");
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
        monster.GetComponent<Animator>().enabled = false;

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

        // �A�j���[�V�����Đ�
        var sequence = DOTween.Sequence();
        sequence.Append(monster.transform.DOShakePosition(5f, 0.1f, 15, 1, false, true).SetEase(Ease.Linear))
            .AppendInterval(0.5f)
            .Append(effect.GetComponent<SpriteRenderer>().DOFade(0.5f, 0.7f).SetEase(Ease.OutCirc))
            .Append(effect.transform.DOMoveY(1.5f, 3f).SetEase(Ease.OutCirc))
            .Join(effect.GetComponent<SpriteRenderer>().DOFade(0f, 3f).SetEase(Ease.OutCirc)
            .OnComplete(() =>
            {
                Destroy(effect.gameObject);
                Instantiate(rainFallingParticle);

                IsMonsterDie = false;
                isSpecialAnim = false;
            }));
        sequence.Play();
    }

    /// <summary>
    /// �z���̃A�j���[�V�����Đ�
    /// </summary>
    IEnumerator PlayMixAnim()
    {
        Vector3 monsterPos = monster.transform.position;

        // �����X�^�[�̃X�v���C�g�̍������擾
        float monsterSizeY = monster.GetComponent<SpriteRenderer>().bounds.size.y;
        // �G�t�F�N�g����
        Instantiate(mixEffectPrefab, new Vector3(monsterPos.x, monsterPos.y + monsterSizeY / 4, -1f), Quaternion.identity);

        yield return new WaitForSeconds(6f);

        // �G�t�F�N�g����
        Instantiate(mixDoneEffectPrefab, new Vector3(monsterPos.x, monsterPos.y + monsterSizeY / 4, -1f), Quaternion.identity);
    }
}
