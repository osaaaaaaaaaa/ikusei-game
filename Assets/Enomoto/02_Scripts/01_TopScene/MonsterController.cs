using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;

public class MonsterController : MonoBehaviour
{
    [SerializeField] GameObject rainFallingParticle;
    [SerializeField] GameObject evolutionEffectPrefab;
    [SerializeField] GameObject typeMonsterImagePrefab;
    [SerializeField] List<GameObject> monsterPrefabs;
    GameObject monster;
    bool isPlayEvolutionAnim;
    bool isPlayKillAnim;

    // �C���X�^���X�쐬
    static MonsterController instance;
    public static MonsterController Instance { get { return instance; } }

    private void Awake()
    {
        isPlayEvolutionAnim = false;
        isPlayKillAnim = false;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    PlayKillAnim();
        //}
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
    /// �A�j���[�V�����Đ��J�n
    /// </summary>
    public void PlayStartAnim()
    {
        monster.GetComponent<Animator>().enabled = true;
    }

    /// <summary>
    /// �W�����v�A�j���[�V�����Đ�
    /// </summary>
    public void PlayJumpAnim()
    {
        monster.GetComponent<Animator>().Play("MonsterJump");
    }

    /// <summary>
    /// �����X�^�[���|���A�j���[�V����
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
    /// �i������A�j���[�V�����Đ�
    /// </summary>
    public void PlayEvolutionAnim()
    {
        if (isPlayEvolutionAnim) return;
        isPlayEvolutionAnim = true;
        bool isPlaingAnim = monster.GetComponent<Animator>().enabled;
        monster.GetComponent<Animator>().enabled = false;

        // �G�t�F�N�g����
        GameObject effect1 = Instantiate(typeMonsterImagePrefab);
        GameObject effect2 = Instantiate(evolutionEffectPrefab);
        effect1.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y,5);
        effect1.GetComponent<SpriteRenderer>().sprite = monster.GetComponent<SpriteRenderer>().sprite;

        // �A�j���[�V�����Đ�
        var sequence = DOTween.Sequence();
        sequence.Append(effect1.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f).SetEase(Ease.Linear).SetLoops(2,LoopType.Restart))
            .Join(effect1.GetComponent<SpriteRenderer>().DOFade(0f, 1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Restart))
            .AppendInterval(1f)
            .Join(effect2.GetComponent<SpriteRenderer>().DOFade(1f, 1.5f).SetEase(Ease.InFlash))
            .AppendInterval(1f)
            .Append(effect2.GetComponent<SpriteRenderer>().DOFade(0f, 1f).SetEase(Ease.Linear).OnComplete(()=> 
            {
                monster.GetComponent<Animator>().enabled = isPlaingAnim;
                Destroy(effect1.gameObject);
                Destroy(effect2.gameObject);
            }));
    }

    /// <summary>
    /// ���S����A�j���[�V�����Đ�
    /// </summary>
    public void PlayKillAnim()
    {
        if (isPlayKillAnim) return;
        isPlayKillAnim = true;
        monster.GetComponent<Animator>().enabled = false;

        // �J���[�쐬
        string colorString = "#6967FF";
        Color createColor;
        ColorUtility.TryParseHtmlString(colorString, out createColor);

        // �����X�^�[�̏�����Ԑݒ�
        monster.transform.localEulerAngles = new Vector3(0f,0f,90f);
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
            .OnComplete(()=> 
            {
                Destroy(effect.gameObject);
                Instantiate(rainFallingParticle);
            }));
        sequence.Play();
    }
}
