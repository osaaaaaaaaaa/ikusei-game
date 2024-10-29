using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GrowManager : MonoBehaviour
{
    [SerializeField] GameObject effect;
    const float effectMaxPos_X = 1.5f;
    const float effectMaxPos_Y = 2f;

    [SerializeField] HungerGageController gageHunger;
    int hungerAmount;
    const float extraMaxCnt = 5;
    float extraCnt;

    [SerializeField] List<GameObject> throwFoodPrefabs;
    public bool isDragFood { get; private set; }

    public bool isPause { get; private set; }

    SpriteRenderer spriteRendererMonster;
    Color colorCreate;

    private void Start()
    {
        // カラー作成
        string colorString = "#6967FF";
        ColorUtility.TryParseHtmlString(colorString, out colorCreate);

        extraCnt = 0;
        hungerAmount = 0;
        gageHunger.UpdateGage(hungerAmount);

        // モンスター生成処理
        var monster = MonsterController.Instance.GenerateMonster(new Vector2(0f, -1f));
        spriteRendererMonster = monster.GetComponent<SpriteRenderer>();
        monster.GetComponent<Rigidbody2D>().gravityScale = 0;
        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Idle);

        GenerateFood();
    }

    private void Update()
    {
        if (!isDragFood && Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit2d)
            {
                GameObject targetObj = hit2d.collider.gameObject;
                if(targetObj.tag == "Food")
                {
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
        var food = Instantiate(throwFoodPrefabs[rndPoint], new Vector3(0f,-2f,0f), Quaternion.identity);
        food.GetComponent<Rigidbody2D>().gravityScale = 0;
        food.GetComponent<ThrowFood>().MealManager = this;
    }

    public void AddHungerAmount()
    {
        if (hungerAmount < Constant.hungerMaxAmount)
        {
            var tmp = hungerAmount + Constant.GetHungerIncrease();
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
        for(int i = 0;i < 3; i++)
        {
            float rndX = (float)Random.Range(-effectMaxPos_X, effectMaxPos_X);
            float rndY = (float)Random.Range(-effectMaxPos_Y, effectMaxPos_Y);
            Instantiate(effect, new Vector3(rndX, rndY, 10f), Quaternion.identity);
        }
    }

    public void OnCancelButton()
    {
        Initiate.Fade("01_TopScene", Color.white, 1.0f);
    }

    public void TogglePauseFrag(bool frag)
    {
        isPause = frag;
    }
}
