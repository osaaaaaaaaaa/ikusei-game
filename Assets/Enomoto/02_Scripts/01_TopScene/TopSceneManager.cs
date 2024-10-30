using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Random = UnityEngine.Random;


public class TopSceneManager : MonoBehaviour
{
    #region タイトル関係
    [SerializeField] GameObject titleSet;
    #endregion

    #region トップ画面関係
    [SerializeField] GameObject tapGuard;
    [SerializeField] GameObject topSet;
    [SerializeField] GameObject menuBtn;
    [SerializeField] GameObject levelNum;
    [SerializeField] GameObject ExpGage;
    [SerializeField] GameObject hungerGage;
    [SerializeField] Text nextEvoLevelText;
    [SerializeField] Text monsterNameText;
    [SerializeField] Text foodsCurrentText;
    #endregion

    #region 卵の孵化する時間UI
    [SerializeField] GameObject HachingTimerParent;
    [SerializeField] Text textHachingTimer;
    #endregion

    #region poop関係
    [SerializeField] GameObject poop;
    int poopCnt;
    const float poopMaxPos_X = 0.75f;
    const float poopMinPos_X = -0.75f;
    const float poopMaxPos_Y = 1f;
    const float poopMinPos_Y = -0.35f;
    #endregion

    #region network関係
    NetworkManager networkManager;
    #endregion

    bool isTouchMonster;

#if UNITY_EDITOR
    DateTime TEST_createdTime;
    int testParam_Huger = 40;
    int TEST_monsterState = 0;   // [1:卵]
#endif

    private void Awake()
    {
        string strTime = "2024/10/29 15:00:00";
        TEST_createdTime = DateTime.Parse(strTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.Instance;

        isTouchMonster = false;

        // ユーザー設定
        nextEvoLevelText.text = networkManager.userInfo.Name;
        monsterNameText.text = networkManager.nurtureInfo.Name;
        foodsCurrentText.text = networkManager.userInfo.FoodVol.ToString();

        // ゲージ関係のパラメータ設定
        hungerGage.GetComponent<HungerGageController>().UpdateGage(NetworkManager.Instance.nurtureInfo.StomachVol);
        ExpGage.GetComponent<ExpGage>().UpdateGage(NetworkManager.Instance.nurtureInfo.Exp,
                                                   (int)(Math.Pow(NetworkManager.Instance.nurtureInfo.Level + 1, 3) - Math.Pow(NetworkManager.Instance.nurtureInfo.Level, 3)), 
                                                   NetworkManager.Instance.nurtureInfo.Level);

        // モンスター生成処理
        MonsterController.Instance.GenerateMonster(MonsterController.Instance.TEST_monsterID,new Vector2(0f, -1.5f)).GetComponent<Rigidbody2D>().gravityScale = 0;
        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Idle);

        // モンスターの死亡チェック
        if (MonsterController.Instance.IsMonsterDie || testParam_Huger <= 0)
        {
            menuBtn.SetActive(false);
            MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Die);
        }
        else
        {
            // 確率でpoopを生成する
            GeneratePoop();
        }

       // 進化待機アニメーション ============================================================================================
       //MonsterController.Instance.IsMonsterEvolution = true;
       //MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.EvolutioinWait);
    }

    // Update is called once per frame
    void Update()
    {
        if (MonsterController.Instance.isSpecialAnim)
        {
            menuBtn.SetActive(false);
            return;
        }
        else
        {
            menuBtn.SetActive(true);
        }

        if (titleSet.activeSelf && Input.GetMouseButtonDown(0))
        {
            titleSet.SetActive(false);
            ToggleTopVisibility(true);
        }

        bool isEggHaching = false;
        if (TEST_monsterState == 1)
        {
            // 卵の状態の場合は孵化できるかどうかチェック
            isEggHaching = IsEggHaching();
        }
        else
        {
            HachingTimerParent.SetActive(false);
        }

        if (!isTouchMonster && Input.GetMouseButtonUp(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit2d)
            {
                GameObject targetObj = hit2d.collider.gameObject;

                // モンスターをタップした場合
                if (!isTouchMonster && targetObj.tag == "Monster")
                {
                    if (MonsterController.Instance.IsMonsterEvolution)
                    {
                        // 進化できる場合は専用のアニメーション再生
                        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Evolutioin);
                    }
                    else if (isEggHaching)
                    {
                        // 孵化できる場合は専用のアニメーション再生
                        HachingTimerParent.SetActive(false);
                        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Hatching);
                    }
                    else
                    {
                    isTouchMonster = true;
                    MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Jump);
                    Invoke("ResetTriggerFrag", 1f);

                    }
                }
                // poopをタップした場合
                else if (targetObj.tag == "Poop")
                {
                    poopCnt--;
                    targetObj.GetComponent<Poop>().Destroy();
                }
            }
        }

    }

    /// <summary>
    /// poopを生成する
    /// </summary>
    void GeneratePoop()
    {
        var rndPoint = Random.Range(1, 4);
        if (rndPoint == 1)
        {
            poopCnt = Random.Range(1, 4);
            for (int i = 0; i < poopCnt; i++)
            {
                float x = (float)Random.Range(poopMinPos_X, poopMaxPos_X);
                float y = (float)Random.Range(poopMinPos_Y, poopMaxPos_Y);
                Instantiate(poop, new Vector2(x, y), Quaternion.identity);
            }
        }
        else
        {
            poopCnt = 0;
        }
    }

    bool IsEggHaching()
    {
        // 卵を生成してからの孵化する残り時間を取得
        var elapsedTime = DateTime.Now - TEST_createdTime;
        TimeSpan hachingTime = new TimeSpan(0, 0, Constant.GetEggHachingTimer("SSR"));
        int totalSeconds = (int)(hachingTime.TotalSeconds - elapsedTime.TotalSeconds) < 0 ? 0 : (int)(hachingTime.TotalSeconds - elapsedTime.TotalSeconds);

        if (totalSeconds == 0)
        {
            textHachingTimer.text = "00：00";
            return true;
        }
        else
        {
            // 残り時間を更新する
            hachingTime = new TimeSpan(0, 0, totalSeconds);
            textHachingTimer.text = hachingTime.Minutes + "：" + hachingTime.Seconds;
            return false;
        }
    }

    void ResetTriggerFrag()
    {
        isTouchMonster = false;
    }

    /// <summary>
    /// トップ画面の表示・非表示
    /// </summary>
    /// <param name="isVisibility"></param>
    public void ToggleTopVisibility(bool isVisibility)
    {
        if (MonsterController.Instance.isSpecialAnim) return;
        topSet.SetActive(isVisibility);
    }

    public void OnTrainingButton()
    {
        if (MonsterController.Instance.isSpecialAnim) return;

        int rnd = 3;//Random.Range(1, 4);
        switch (rnd)
        {
            case 1:
                Initiate.Fade("GameScene1", Color.black, 1.0f);
                break;
            case 2:
                Initiate.Fade("GameScene2", Color.black, 1.0f);
                break;
            case 3:
                Initiate.Fade("GameScene3", Color.black, 1.0f);
                break;
        }
    }

    public void OnGrowButton()
    {
        if (MonsterController.Instance.isSpecialAnim) return;
        if (poopCnt > 0) MonsterController.Instance.IsMonsterDie = true;
        Initiate.Fade("02_GrowScene", Color.white, 1.0f);
    }

    public void OnSupplyButton()
    {
        if (MonsterController.Instance.isSpecialAnim) return;
        if (poopCnt > 0) MonsterController.Instance.IsMonsterDie = true;
        SceneManager.LoadScene("03_SupplyScene");
    }

    public void OnLibraryButton()
    {
        if (MonsterController.Instance.isSpecialAnim) return;
        if (poopCnt > 0) MonsterController.Instance.IsMonsterDie = true;
        SceneManager.LoadScene("04_LibraryScene");
    }

    public void OnMixButton()
    {
        if (MonsterController.Instance.isSpecialAnim) return;
        if (poopCnt > 0) MonsterController.Instance.IsMonsterDie = true;
        SceneManager.LoadScene("05_MixScene");
    }

    public void OnInventoryButton()
    {
        if (MonsterController.Instance.isSpecialAnim) return;
        if (poopCnt > 0) MonsterController.Instance.IsMonsterDie = true;
        SceneManager.LoadScene("05_Inventory");
    }

    public void ToggleTapGuardVisibility(bool visibility)
    {
        if (visibility) Invoke("ShowTapGuard", 1f);
        if (!visibility) Invoke("HideTapGuard", 1f);
    }

    void ShowTapGuard()
    {
        tapGuard.SetActive(true);
    }

    void HideTapGuard()
    {
        tapGuard.SetActive(false);
    }
}
