using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using KanKikuchi.AudioManager;

public class GrowManager : MonoBehaviour
{
    [SerializeField] Transform monsterPoint;

    [SerializeField] GameObject effect;
    const float effectMaxPos_X = 1.5f;
    const float effectMaxPos_Y = 2f;

    [SerializeField] HungerGageController gageHunger;
    int hungerAmount;
    const float extraMaxCnt = 5;
    float extraCnt;

    [SerializeField] Text foodVolText;
    int getExp;
    int mealExp;
    int mealCnt;
    const int decFoodVol = 15;
    public int decreaseFoodVol { get; private set; }
    public int nowFoodVol { get; private set; }

    [SerializeField] List<GameObject> throwFoodPrefabs;
    public bool isDragFood { get; private set; }

    public bool isPause { get; private set; }

    public bool isGrow { get; set; } = true;

    SpriteRenderer spriteRendererMonster;
    Color colorCreate;

    private void Start()
    {
        BGMManager.Instance.Play(BGMPath.GROW);

        // カラー作成
        string colorString = "#6967FF";
        ColorUtility.TryParseHtmlString(colorString, out colorCreate);

        extraCnt = 0;
        hungerAmount = NetworkManager.Instance.nurtureInfo.StomachVol;
        gageHunger.UpdateGage(hungerAmount);
        decreaseFoodVol = decFoodVol;
        mealExp = (int)(Math.Pow(NetworkManager.Instance.nurtureInfo.Level + 1, 3) - Math.Pow(NetworkManager.Instance.nurtureInfo.Level, 3)) / 20;
        nowFoodVol = NetworkManager.Instance.userInfo.FoodVol;
        foodVolText.text = nowFoodVol.ToString();

        // モンスター生成処理
        var monster = MonsterController.Instance.GenerateMonster(NetworkManager.Instance.nurtureInfo.MonsterID, monsterPoint);
        spriteRendererMonster = monster.GetComponent<SpriteRenderer>();
        monster.GetComponent<Rigidbody2D>().gravityScale = 0;
        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Idle);

        if(nowFoodVol < decreaseFoodVol)
        {   // 所持数が使用数より少ない時
            isGrow = false;
        }
        else
        {
            GenerateFood();
        }
    }

    private void Update()
    {
        if (!isGrow) return;

        if (!isDragFood && Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit2d)
            {
                GameObject targetObj = hit2d.collider.gameObject;
                if(targetObj.tag == "Food")
                {
                    nowFoodVol = nowFoodVol - decreaseFoodVol;
                    foodVolText.text = nowFoodVol.ToString();
                    isDragFood = true;
                    targetObj.GetComponent<Rigidbody2D>().gravityScale = 1;
                }
            }
        }
    }

    public void GenerateFood()
    {
        isDragFood = false;

        int rndPoint = Random.Range(0, throwFoodPrefabs.Count);
        var food = Instantiate(throwFoodPrefabs[rndPoint], new Vector3(0f,-1.0f,0f), Quaternion.identity);
        food.GetComponent<Rigidbody2D>().gravityScale = 0;
        food.GetComponent<ThrowFood>().GrowManager = this;
    }

    public void AddHungerAmount()
    {
        mealCnt++;

        if (hungerAmount < Constant.hungerMaxAmount)
        {
            var tmp = hungerAmount + Constant.baseHungerIncrease;
            hungerAmount = tmp < Constant.hungerMaxAmount ? tmp : Constant.hungerMaxAmount;
            gageHunger.UpdateGage(hungerAmount);

            if (hungerAmount >= Constant.hungerMaxAmount) GenerateEffects();
        }
        else if (extraCnt < extraMaxCnt)
        {
            float div = extraMaxCnt - extraCnt;
            float r = (1 - colorCreate.r) / div;
            float g = (1 - colorCreate.g) / div;
            float b = (1 - colorCreate.b) / div;
            Color color = new Color(1 - r, 1 - g, 1 - b);
            spriteRendererMonster.color = color;
            extraCnt++;
            if(extraCnt >= extraMaxCnt) MonsterController.Instance.IsMonsterDie = true;
        }
    }

    void GenerateEffects()
    {
        SEManager.Instance.Play(SEPath.FLASH);
        for(int i = 0;i < 3; i++)
        {
            float rndX = (float)Random.Range(-effectMaxPos_X, effectMaxPos_X);
            float rndY = (float)Random.Range(-effectMaxPos_Y, effectMaxPos_Y);
            Instantiate(effect, new Vector3(rndX, rndY, 10f), Quaternion.identity);
        }
    }

    public void OnCancelButton()
    {
        SEManager.Instance.Play(SEPath.BTN_MENU);

        // 満腹値の上限超過時処理
        if (hungerAmount >= Constant.hungerMaxAmount) { hungerAmount = Constant.hungerMaxAmount; }
        gageHunger.UpdateGage(hungerAmount);

        // 経験値の計算
        getExp = NetworkManager.Instance.nurtureInfo.Exp + mealExp * mealCnt;

        StartCoroutine(NetworkManager.Instance.ExeMeal(
            hungerAmount,
            nowFoodVol,
            getExp,
            result =>
            {
                if (result == null)
                {
                    Debug.Log("食事失敗");
                    Initiate.Fade("01_TopScene", Color.white, 1.0f);
                }
                else
                {
                    Debug.Log("食事成功");
                    NetworkManager.Instance.nurtureInfo.Level = result.Level;
                    NetworkManager.Instance.nurtureInfo.Exp = result.Exp;
                    Initiate.Fade("01_TopScene", Color.white, 1.0f);
                }
            }));
    }

    public void TogglePauseFrag(bool frag)
    {
        isPause = frag;
    }
}
