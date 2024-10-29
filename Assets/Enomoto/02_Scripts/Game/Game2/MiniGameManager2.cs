using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MiniGameManager2 : MonoBehaviour
{
    #region �}�l�[�W���[�֌W
    [SerializeField] JumpController jumpController;
    [SerializeField] CountDown countDown;
    [SerializeField] InvincibleController invincibleController;
    #endregion

    #region �����X�^�[�֌W
    GameObject monster;
    int monsterHitCnt;
    bool isInvincible;
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    #endregion

    #region �X�e�[�W�֌W
    [SerializeField] GameObject rock;
    [SerializeField] Transform stageParent;
    [SerializeField] List<GameObject> obstractPrefabs;
    float addSpeed;
    float currentTimeObstracle;
    float triggerTimeObstracle;
    #endregion

    #region �n�ʂƔw�i�֌W
    [SerializeField] LoopScrollImage woodBG;
    [SerializeField] GameObject groundBG;
    [SerializeField] GameObject groundBGCollider;
    [SerializeField] GameObject endGround;
    const float groundSpeed = 4;
    #endregion

    #region UI�֌W
    [SerializeField] Flashing warningUI;
    [SerializeField] GameObject resultUI;
    [SerializeField] Slider slider;
    #endregion

    public bool isGameOver { get; private set; }
    public bool isGameClear { get; private set; }

    // �Q�[������
    float currentTime;
    const float timeMax = 40;

    void Start()
    {
        monsterHitCnt = 0;
        addSpeed = 0;
        currentTimeObstracle = 0;
        triggerTimeObstracle = 3;
        currentTime = 0;
        isGameOver = false;
        isGameClear = false;

        // �����X�^�[��������
        monster = MonsterController.Instance.GenerateMonster(new Vector2(0, -1.8f));
        // �W�����v�R���g���[���[�̏���������
        jumpController.Init(monster, monster.GetComponent<Rigidbody2D>());
    }

    private void Update()
    {
        if(!countDown.isAnimEnd || isGameClear)
        {
            return;
        }
        if (isGameOver)
        {
            monster.transform.Rotate(new Vector3(0f, 0f, -300f) * Time.deltaTime);
            return;
        }

        currentTimeObstracle += Time.deltaTime;
        currentTime += Time.deltaTime;
        if (!isGameClear && currentTime >= timeMax)
        {
            if(monster.transform.localPosition.y <= -6f) 
            {
                // ����~�߂�
                rock.transform.GetComponent<Rotate>().enabled = false;
                rock.transform.GetComponent<CircleCollider2D>().enabled = false;
                rock.transform.GetComponent<Rigidbody2D>().gravityScale = 0;
                rock.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                rock.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                // ��ʊO�ɍs���Ǝ��S
                monster.GetComponent<PolygonCollider2D>().enabled = false;
                monster.transform.localPosition = new Vector2(0, -6f);
                StopScrollBG();
                HitMonster(3);
            }
            else
            {
                // �o�ߎ��Ԃ��������Ԉȏ�̏ꍇ
                MoveGrounds();
            }
        }
        else if (currentTimeObstracle >= triggerTimeObstracle)
        {
            // ���Ԋu�ŏ�Q���𐶐�����
            currentTimeObstracle = 0;
            GenerateObstract();
        }

        // �X���C�_�[���X�V����
        slider.value = currentTime / timeMax;
    }

    /// <summary>
    /// ��Q����������
    /// </summary>
    void GenerateObstract()
    {
        int index = Random.Range(0, obstractPrefabs.Count + 1);
        if (index != 0)
        {
            // ��Q���𐶐����A���X�ɃX�s�[�h�A�b�v������
            var obstacle = Instantiate(obstractPrefabs[index - 1], stageParent);
            obstacle.GetComponent<Obstacle>().Init(this,4 + addSpeed);
            addSpeed += 0.1f;
            if (triggerTimeObstracle > 1f) triggerTimeObstracle -= addSpeed;
            if (triggerTimeObstracle <= 1f) triggerTimeObstracle = 1f;

            // �x���}�[�N��_�ł�����
            warningUI.PlayFlashing();
        }
    }

    /// <summary>
    /// �����X�^�[����Q���Ƀq�b�g�����Ƃ��̏���
    /// </summary>
    public void HitMonster(int damageAmount)
    {
        if (isGameOver || isInvincible) return;

        monsterHitCnt+= damageAmount;
        if(monsterHitCnt >= 3)
        {
            isGameOver = true;
            MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Fall);
            Invoke("ShowResult", 2f);
        }
        else
        {
            invincibleController.PlayInvincibleAnim(monster.GetComponent<SpriteRenderer>());
        }
    }

    /// <summary>
    /// �n�ʂ𓮂�������
    /// </summary>
    void MoveGrounds()
    {
        groundBG.transform.Translate(new Vector2(-1 * groundSpeed, 0f) * Time.deltaTime, Space.Self);
        groundBGCollider.transform.Translate(new Vector2(-1 * groundSpeed, 0f) * Time.deltaTime, Space.Self);
        endGround.transform.Translate(new Vector2(-1 * groundSpeed, 0f) * Time.deltaTime, Space.Self);

        if(endGround.transform.position.x <= -2f)
        {
            isGameClear = true;
            Invoke("GameClear", 2f);
            StopScrollBG();
        }
    }

    /// <summary>
    /// �w�i���~����
    /// </summary>
    void StopScrollBG()
    {
        woodBG.enabled = false;
        groundBG.GetComponent<LoopScrollImage>().enabled = false;
    }

    void ShowResult()
    {
        resultUI.SetActive(true);
    }

    void GameClear()
    {
        isGameClear = true;
        ShowResult();
    }

    public void OnBackButton()
    {
        Initiate.Fade("01_TopScene", Color.black, 1.0f);
    }
}
