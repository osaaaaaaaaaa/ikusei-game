using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;
using KanKikuchi.AudioManager;

public class MiniGameManager2 : MonoBehaviour
{
    #region �}�l�[�W���[�֌W
    [SerializeField] JumpController jumpController;
    [SerializeField] CountDown countDown;
    [SerializeField] InvincibleController invincibleController;
    #endregion

    #region �����X�^�[�֌W
    [SerializeField] Transform monsterPoint;
    GameObject monster;
    int monsterHitCnt;
    bool isInvincible;
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    #endregion

    #region �X�e�[�W�֌W
    [SerializeField] GameObject rock;
    [SerializeField] Transform stageParent;
    [SerializeField] List<GameObject> obstractPrefabs;
    int baseExp;
    float addSpeed;
    float currentTimeObstracle;
    float triggerTimeObstracle;
    const int gameOverCount = 3;
    const int confExp = 3;
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

    bool isGameEnd;
    public bool isGameOver { get; private set; }
    public bool isGameClear { get; private set; }

    // �Q�[������
    float currentTime;
    const float timeMax = 2;

    void Start()
    {
        monsterHitCnt = 0;
        addSpeed = 0;
        currentTimeObstracle = 0;
        triggerTimeObstracle = 2;
        currentTime = 0;
        isGameEnd = false;
        isGameOver = false;
        isGameClear = false;
        baseExp = (int)(Math.Pow(NetworkManager.Instance.nurtureInfo.Level + 1, 3) - Math.Pow(NetworkManager.Instance.nurtureInfo.Level, 3)) / 3;

        // �����X�^�[��������
        monster = MonsterController.Instance.GenerateMonster(NetworkManager.Instance.nurtureInfo.MonsterID, monsterPoint);
        // �W�����v�R���g���[���[�̏���������
        jumpController.Init(monster, monster.GetComponent<Rigidbody2D>());
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && isGameEnd && !resultUI.activeSelf)
        {
            // �Q�[���N���A���̃A�j���[�V�����X�L�b�v�p
            ShowResult();
        }
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
                HitMonster(gameOverCount);
            }
            else
            {
                // �o�ߎ��Ԃ��ݒ肵�����Ԃ𒴂����ꍇ
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
            addSpeed += 0.15f;
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

        SEManager.Instance.Play(SEPath.DAMAGE);

        monsterHitCnt += damageAmount;
        if(monsterHitCnt >= gameOverCount)
        {
            isGameOver = true;
            SEManager.Instance.Stop(SEPath.ROCK_ROTATE);
            MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Fall);
            Invoke("PlayGameoverSE", 2f);
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
            // �Q�[���N���A����
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
        if (resultUI.activeSelf) return;  // 2��ȏ㏈�������̂�h��
        resultUI.SetActive(true);

        if (monsterHitCnt >= gameOverCount) { monsterHitCnt = gameOverCount; }
        int exp = (int)(baseExp / gameOverCount);
        if(exp < confExp) {  exp = confExp; }
        exp = exp * (gameOverCount + 2 - monsterHitCnt);

        // �o���l�擾
        StartCoroutine(NetworkManager.Instance.ExeExercise(
            NetworkManager.Instance.nurtureInfo.StomachVol - Constant.baseHungerDecrease,
            NetworkManager.Instance.nurtureInfo.Exp + exp,
            result =>
            {
                if (result != null)
                {
                    NetworkManager.Instance.nurtureInfo.Level = result.Level;
                    NetworkManager.Instance.nurtureInfo.Exp = result.Exp;
                    Debug.Log("�o���l�X�V����");
                }
                else
                {
                    Debug.Log("�o���l�X�V���s");
                }
            }));

        Debug.Log("�o���l�F" + exp);

    }

    void GameClear()
    {
        isGameEnd = true;
        isGameClear = true;
        monsterPoint.Rotate(new Vector3(0f, 0f, -10));
        MonsterController.Instance.ChangeCenteredPivotSprite();
        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Glad);
        Invoke("ShowResult", 4f);
    }

    void PlayGameoverSE()
    {
        SEManager.Instance.Play(SEPath.FAILURE);
    }
}
