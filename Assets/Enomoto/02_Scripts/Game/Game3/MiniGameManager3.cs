using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class MiniGameManager3 : MonoBehaviour
{
    [SerializeField] WireController wireController;

    [SerializeField] GameObject particleExplosionPrefab;
    [SerializeField] List<GameObject> bombObjs;

    [SerializeField] GameObject wiresParent;
    [SerializeField] Text textTimer;
    [SerializeField] List<GameObject> hintList;
    [SerializeField] GameObject hintArrowUI;
    [SerializeField] GameObject resultUI;
    Sequence sequenceMonster;
    GameObject monster;

    Vector3 wireParentStartPoint;
    public List<int> colorIndexOrders { get; private set; }  // ���C���[�̐F����Ƃ����A�������؂鏇��

    int baseExp;
    float timer;
    int roundCnt;
    int roundMaxCnt;
    public bool isPause { get; private set; }
    public bool isGameEnd { get; private set; }

    const float defultTimer = 11;

    // Start is called before the first frame update
    void Start()
    {
        // �����X�^�[�𐶐����A�����X�^�[�����������A�A�j���Đ�
        sequenceMonster = DOTween.Sequence();
        monster = MonsterController.Instance.GenerateMonster(Vector2.zero);
        monster.GetComponent<Rigidbody2D>().gravityScale = 0;
        InitMonster();

        // ���C���[�̐e�I�u�W�F�N�g�̏����ʒu�擾
        wireParentStartPoint = wiresParent.transform.position;

        roundCnt = 0;
        roundMaxCnt = 5;
        isPause = true;
        isGameEnd = false;
        baseExp = (int)(Math.Pow(NetworkManager.Instance.nurtureInfo.Level + 1, 3) - Math.Pow(NetworkManager.Instance.nurtureInfo.Level, 3)) / 3;

        Invoke("SetupNextRound", 4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause) return;
        if (isGameEnd)
        {
            monster.transform.Rotate(new Vector3(0f, 0f, -300f) * Time.deltaTime);
            return;
        }

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 0;
            isGameEnd = true;
        }
        UpdateTextTimer();

        if (isGameEnd) GameOver();
    }

    /// <summary>
    /// �^�C�}�[�̃e�L�X�g���X�V
    /// </summary>
    void UpdateTextTimer()
    {
        string text = "" + Mathf.Floor(timer * 100);
        text = text.Length == 3 ? "0" + text : text;
        text = text.Length == 2 ? "00" + text : text;
        text = text.Length == 1 ? "000" + text : text;
        textTimer.text = text.Insert(2, ":");
    }

    /// <summary>
    /// ���C���[�̐F����Ƃ����������؂鏇�Ԃ����߂�
    /// </summary>
    void DrawColorIndexOrder()
    {
        // COLOR_TYPE_ID���Q�Ƃ��ėp��
        colorIndexOrders = new List<int> { 0, 1, 2 };

        // ���X�g�̒��g���V���b�t������(�~��)
        for (int i = colorIndexOrders.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);
            var temp = colorIndexOrders[i];
            colorIndexOrders[i] = colorIndexOrders[j];
            colorIndexOrders[j] = temp;
        }

        SetupHintUI();
    }

    /// <summary>
    /// �q���g������UI�̃Z�b�g�A�b�v
    /// </summary>
    void SetupHintUI()
    {
        int rndPoint = Random.Range(1, 11);
        int mulParam = rndPoint <= 5 ? 1 : -1;
        hintArrowUI.transform.localScale = new Vector3(1 * mulParam, 1, 1);
        for(int i = 0;i < hintList.Count; i++)
        {
            // ���̌����ɂ���ăJ���[��ύX���鏇�Ԃ��ω�����
            Image image;
            if(mulParam == -1)
            {
                image = hintList[hintList.Count - 1 - i].GetComponent<Image>();
            }
            else
            {
                image = hintList[i].GetComponent<Image>();
            }

            // �F�ύX
            switch (colorIndexOrders[i])
            {
                case (int)WireController.COLOR_TYPE_ID.Red:
                    image.color = Color.red;
                    break;
                case (int)WireController.COLOR_TYPE_ID.Blue:
                    image.color = Color.blue;
                    break;
                case (int)WireController.COLOR_TYPE_ID.Green:
                    image.color = Color.green;
                    break;
            }
        }
    }

    /// <summary>
    /// ���̃��E���h����
    /// </summary>
    public void SetupNextRound()
    {
        InitMonster();

        isPause = true;
        roundCnt++;
        if(roundCnt > roundMaxCnt)
        {
            Debug.Log("�Q�[���N���A");
            ShowResult();
        }
        else
        {
            PlaySetupAnim(roundCnt == 1);
        }
    }

    /// <summary>
    /// �Z�b�g�A�b�v�̃A�j���[�V����
    /// </summary>
    void PlaySetupAnim(bool isSkip)
    {
        int alliePoint = 1500;
        float animTime = 0.5f;

        var sequence = DOTween.Sequence();

        if (!isSkip)
        {
            sequence.Append(wiresParent.transform.DOLocalMoveX(-alliePoint, animTime).SetEase(Ease.Linear)
            .OnComplete(() => { 
                // �����ړ�������
                wiresParent.transform.position = new Vector3(alliePoint, wireParentStartPoint.y, wireParentStartPoint.z);

                // ���e��������A�j���[�V����
                PlayDestroyBombAnim();

                // ���C���[�̃Z�b�g�A�b�v����
                wireController.DrawRandomColor();
                DrawColorIndexOrder();
                timer = defultTimer - roundCnt;
                UpdateTextTimer();
            }))
            .AppendInterval(animTime);
        }
        else
        {
            // ���C���[�̃Z�b�g�A�b�v����
            wireController.DrawRandomColor();
            DrawColorIndexOrder();
            timer = defultTimer - roundCnt;
            UpdateTextTimer();
        }

        sequence.Append(wiresParent.transform.DOLocalMoveX(0, animTime).SetEase(Ease.Linear)
            .OnComplete(() => { 
                isPause = false;
                PlayMonsterAnim();
            }));

        sequence.Play();
    }

    /// <summary>
    /// �����X�^�[�̃A�j���[�V�����Đ�
    /// </summary>
    void PlayMonsterAnim()
    {
        // �J���[�쐬
        string colorString = "#6967FF";
        Color createColor;
        ColorUtility.TryParseHtmlString(colorString, out createColor);

        // �A�j���[�V�����Đ�
        sequenceMonster = DOTween.Sequence();
        sequenceMonster.Append(monster.transform.DOShakePosition(1000f, 0.1f, 15, 1, false, true).SetEase(Ease.Linear))
            .Join(monster.GetComponent<SpriteRenderer>().DOColor(createColor, defultTimer - roundCnt));
        sequenceMonster.Play();
    }

    /// <summary>
    /// �����X�^�[�̏���������
    /// </summary>
    void InitMonster()
    {
        sequenceMonster.Kill();
        monster.transform.localPosition = new Vector3(0f, -5f, 0f);
        monster.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    /// <summary>
    /// ���e��j������A�j���[�V����
    /// </summary>
    void PlayDestroyBombAnim()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(bombObjs[0].GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).SetEase(Ease.Linear)
            .OnComplete(() => {
                var tmp = bombObjs[0];
                bombObjs.RemoveAt(0);
                Destroy(tmp.gameObject); 
            }))
            .Join(bombObjs[0].transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).SetEase(Ease.Linear));
    }

    void ShowResult()
    {
        sequenceMonster.Kill();
        resultUI.SetActive(true);
    }

    public void GameOver()
    {
        isGameEnd = true;
        wiresParent.SetActive(false);
        Instantiate(particleExplosionPrefab);
        InitMonster();
        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Fall);

        // ���݂���S�Ă̔��e��j������
        foreach (var bomb in bombObjs)
        {
            Destroy(bomb.gameObject);
        }

        Invoke("ShowResult", 4f);
    }

    public void OnBackButton()
    {
        int exp = baseExp;
        if (baseExp >= roundMaxCnt) { exp = baseExp / roundMaxCnt; }
        exp = exp * roundCnt;

        // �o���l�擾
        StartCoroutine(NetworkManager.Instance.ExeExercise(
            NetworkManager.Instance.nurtureInfo.StomachVol - Constant.BaseHungerDecrease,
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

        Initiate.Fade("01_TopScene", Color.black, 1.0f);
    }
}
