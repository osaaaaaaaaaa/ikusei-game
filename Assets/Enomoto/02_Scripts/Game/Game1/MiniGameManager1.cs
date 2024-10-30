using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class MiniGameManager1 : MonoBehaviour
{
    #region ���U���g�֌W
    [SerializeField] GameObject resultUI;
    [SerializeField] Text expText;
    [SerializeField] Text hungerText;
    #endregion

    [SerializeField] CountDown countDown;

    [SerializeField] GameObject gage1;
    [SerializeField] List<GameObject> gage2List;
    [SerializeField] GameObject gage3;
    [SerializeField] GameObject gage3StartPoint;
    public float endTime;
    float[] results = new float[3];
    int baseExp;

    bool isTap;
    bool isPlayTween;
    bool isGameStart;
    bool isGameEnd;

    #region ���j�󂷂�Ƃ��Ɏg��
    public const float totalPowerMax = 3;
    public float totalPower { get; private set; }
    public float jumpPower;
    public float gravity;
    #endregion

    public enum MINIGAME1_STATE
    {
        Opening,
        Gage1,
        Gage2,
        Gage3,
        BreakAnim,
        Result
    }
    public MINIGAME1_STATE state { get; private set; }

    private void Awake()
    {
        isTap = false;
        isPlayTween = false;
        isGameStart = false;
        isGameEnd = false;
        baseExp = (int)(Math.Pow(NetworkManager.Instance.nurtureInfo.Level + 1, 3) - Math.Pow(NetworkManager.Instance.nurtureInfo.Level, 3)) / 3;
        state = MINIGAME1_STATE.Opening;

        // �����X�^�[��������
        MonsterController.Instance.GenerateMonster(MonsterController.Instance.TEST_monsterID,new Vector2(0, -1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameEnd || !countDown.isAnimEnd) return;

        // �Q�[���J�n���̂ݏ���
        if(countDown.isAnimEnd && !isGameStart)
        {
            isGameStart = true;
            Invoke("UpdateGameState", 0.5f);
            return;
        }

        // �Q�[�W�̓������~ && ���̃��[�h�ֈڍs
        if (state != MINIGAME1_STATE.BreakAnim && isGameStart && !isTap && Input.GetMouseButtonDown(0))
        {
            isTap = true;
            if (state == MINIGAME1_STATE.Gage2)
            {
                for (int i = 0; i < gage2List.Count; i++)
                {
                    DOTween.Kill(gage2List[i].transform);
                }
            }
            if(state == MINIGAME1_STATE.Gage3) DOTween.Kill(gage3.transform);

            Invoke("UpdateGameState", 0.5f);
        }
        if (isTap) return;

        // �Q�[�W�̃A�j���[�V�������J�n����
        switch (state)
        {
            case MINIGAME1_STATE.Opening:
                break;
            case MINIGAME1_STATE.Gage1:
                gage1.SetActive(true);
                if (gage1.GetComponent<Slider>().value >= 1) gage1.GetComponent<Slider>().value = 0;
                else gage1.GetComponent<Slider>().value += Time.deltaTime;
                break;
            case MINIGAME1_STATE.Gage2:
                if (isPlayTween) return;
                isPlayTween = true;
                for (int i = 0; i < gage2List.Count; i++)
                {
                    gage2List[i].SetActive(true);

                    float mul = 1.5f;
                    Vector3 startPointGage2 = gage2List[i].transform.localPosition;
                    Vector3[] pathGage2 =
                    {
                        startPointGage2 + (Vector3.right * mul + Vector3.up * mul) * (i == 0 ? 1 : -1),    // �E��or����
                        startPointGage2 + (Vector3.up * mul) * (i == 0 ? 1 : -1),                          // ��or��
                        startPointGage2 + (Vector3.left * mul + Vector3.up * mul) * (i == 0 ? 1 : -1),     // ���� or �E��
                        startPointGage2,
                    };

                    gage2List[i].transform.DOLocalPath(pathGage2, endTime, PathType.CatmullRom).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
                }
                break;
            case MINIGAME1_STATE.Gage3:
                if (isPlayTween) return;
                gage3.SetActive(true);
                gage3StartPoint.SetActive(true);
                isPlayTween = true;
                Vector3 startPointGage3 = gage3.transform.localPosition;
                Vector3[] pathGage3 =
                {
                    new Vector3(1.3f,-1f,0f),
                    new Vector3(2.3f,-0.1f,0f),
                    new Vector3(1.3f,-1f,0f),
                    startPointGage3,
                    new Vector3(-1.3f,-1f,0f),
                    new Vector3(-2.3f,-0.1f,0f),
                    new Vector3(-1.3f,-1f,0f),
                    startPointGage3,
                };

                gage3.transform.DOLocalPath(pathGage3, endTime, PathType.CatmullRom).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

                break;
            case MINIGAME1_STATE.Result:
                ShowResult();
                break;
        }
    }

    public void UpdateGameState()
    {
        isTap = false;
        isPlayTween = false;

        float dis;
        switch (state)
        {
            case MINIGAME1_STATE.Gage1:

                // �Q�[�W���\��
                gage1.SetActive(false);

                // ���ʂ��v�Z
                results[0] = gage1.GetComponent<Slider>().value > 0 ? gage1.GetComponent<Slider>().value : 0;
                Debug.Log("�Q�[�W�P(0~1)�F" + results[0]);

                break;
            case MINIGAME1_STATE.Gage2:

                // �Q�[�W���\��
                for (int i = 0; i < gage2List.Count; i++)
                {
                    gage2List[i].SetActive(false);
                }

                // ���ʂ��v�Z
                dis = Mathf.Abs(Vector3.Distance(gage2List[0].transform.localPosition, gage2List[1].transform.localPosition));
                results[1] = (1 - dis) > 0 ? (1 - dis) : 0;
                Debug.Log("�Q�[�W�Q(0~1)�F" + results[1]);

                break;
            case MINIGAME1_STATE.Gage3:

                // �Q�[�W���\��
                gage3.SetActive(false);
                gage3StartPoint.SetActive(false);

                // ���ʂ��v�Z
                dis = Mathf.Abs(Vector3.Distance(gage3StartPoint.transform.position, gage3.transform.position));
                results[2] = (1 - dis) > 0 ? (1 - dis) : 0;
                Debug.Log("�Q�[�W�R(0~1)�F" + results[2]);

                break;
        }

        if (state < MINIGAME1_STATE.Result) state++;
        if(state == MINIGAME1_STATE.BreakAnim)
        {
            // �g�[�^�����ʂ���
            totalPower = 0;
            for (int i = 0; i < results.Length; i++)
            {
                totalPower += results[i];
            }

            Invoke("JumpMonster", 1f);
        }
    }

    void JumpMonster()
    {
        MonsterController.Instance.monster.GetComponent<Rigidbody2D>().gravityScale = gravity;
        MonsterController.Instance.monster.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    void ShowResult()
    {
        isGameEnd = true;
        resultUI.SetActive(true);

        // �o���l�擾
        int bonusExp = baseExp;
        for (int i = 0; i < results.Length; i++)
        {
            bonusExp = (int)(bonusExp * results[i]);
        }
        int exp = baseExp + bonusExp;

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
}
