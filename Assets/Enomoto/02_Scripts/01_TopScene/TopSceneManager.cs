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
    #region トップ画面関係
    [SerializeField] GameObject topSet;
    [SerializeField] Button menuBtn;
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

    [SerializeField] Transform monsterPoint;
    [SerializeField] Animator animatorMenuBtn;
    int needEvoLevel;
    bool isTouchMonster;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.Instance;

        isTouchMonster = false;

        needEvoLevel = networkManager.monsterList[networkManager.nurtureInfo.MonsterID - 1].EvoLv - networkManager.nurtureInfo.Level;

        // ユーザー設定
        nextEvoLevelText.text = "進化まで" + needEvoLevel.ToString() + "レベル";
        monsterNameText.text = networkManager.nurtureInfo.Name;
        foodsCurrentText.text = networkManager.userInfo.FoodVol.ToString();

        // ゲージ関係のパラメータ設定
        hungerGage.GetComponent<HungerGageController>().UpdateGage(NetworkManager.Instance.nurtureInfo.StomachVol);
        ExpGage.GetComponent<ExpGage>().UpdateGage(NetworkManager.Instance.nurtureInfo.Exp,
                                                   (int)(Math.Pow(NetworkManager.Instance.nurtureInfo.Level + 1, 3) - Math.Pow(NetworkManager.Instance.nurtureInfo.Level, 3)), 
                                                   NetworkManager.Instance.nurtureInfo.Level);

        // モンスター生成処理
        if (networkManager.nurtureInfo.State == 1)
        {   // 卵の時
            MonsterController.Instance.GenerateMonster(0, monsterPoint).GetComponent<Rigidbody2D>().gravityScale = 0;
            MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Idle);
        }
        else
        {
            MonsterController.Instance.GenerateMonster(NetworkManager.Instance.nurtureInfo.MonsterID, monsterPoint).GetComponent<Rigidbody2D>().gravityScale = 0;
            MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Idle);
        }

        // モンスターの死亡チェック
        if (MonsterController.Instance.IsMonsterDie || networkManager.nurtureInfo.StomachVol <= 0)
        {
            menuBtn.interactable = false;
            MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Die);
        }
        else
        {
            // 確率でpoopを生成する
            GeneratePoop();
        }

        if (needEvoLevel <= 0)
        {
            // 進化処理
            MonsterController.Instance.IsMonsterEvolution = true;
            MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.EvolutioinWait);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isMenuActive = animatorMenuBtn.GetBool("OnClickButton");
        if (isMenuActive) return;

        // 卵の状態の場合は孵化できるかどうかチェック
        bool isEggHaching = false;
        if (networkManager.nurtureInfo.State == 1)
        {
            isEggHaching = IsEggHaching();
        }
        else
        {
            HachingTimerParent.SetActive(false);
        }

        // 特殊アニメーションを再生中はメニューボタンをおせなくする
        if (MonsterController.Instance.isSpecialAnim)
        {
            menuBtn.interactable = false;
            return;
        }
        else if(networkManager.nurtureInfo.State ==1)
        {
            menuBtn.interactable = false;
        }
        else 
        {
            menuBtn.interactable = true;
        }

        if (!isTouchMonster && Input.GetMouseButtonDown(0))
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
                        StartCoroutine(NetworkManager.Instance.ChangeState(
                            2,
                            result =>
                            {
                                if (result) { Debug.Log("孵化完了"); }
                                else { Debug.Log("孵化失敗"); }
                            }));
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
        var rndPoint = networkManager.nurtureInfo.State == 1 ? 0 : Random.Range(1, 4); // 卵の状態の場合は生成しない
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
        var elapsedTime = DateTime.Now - NetworkManager.Instance.nurtureInfo.CreatedAt;
        TimeSpan hachingTime = new TimeSpan(0, 0, Constant.GetEggHachingTimer(networkManager.monsterList[networkManager.nurtureInfo.MonsterID - 1].Rarity));
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
}
