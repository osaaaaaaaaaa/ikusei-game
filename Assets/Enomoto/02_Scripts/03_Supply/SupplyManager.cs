using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SupplyManager : MonoBehaviour
{
    [SerializeField] Text textFoodCnt;
    [SerializeField] Image feverBG;
    [SerializeField] Image feverGage;
    [SerializeField] Gradient gradientGage;
    Color colorGageDefault;

    [SerializeField] GameObject generatePoint;
    [SerializeField] List<GameObject> foodPrefabs;

    const float feverAddAmount = 0.1f;
    float feverAmount = 0;
    public bool isFever { get; private set; }

#if UNITY_EDITOR
    int testParam_FoodCnt = 0;
#endif

    // Start is called before the first frame update
    void Start()
    {
        isFever = false;

        colorGageDefault = feverGage.color;
        textFoodCnt.text = "×" + testParam_FoodCnt;
        InvokeRepeating("GenerateFood", 1f, 2f);
    }

    /// <summary>
    /// 食べ物を生成する
    /// </summary>
    void GenerateFood()
    {
        int index = (int)Random.Range(0, foodPrefabs.Count);
        var food = Instantiate(foodPrefabs[index], generatePoint.transform).GetComponent<Food>();
        food.Manager = this;

        if (isFever) food.AddSpeed();
    }

    public void AddFoodCnt()
    {
        if (testParam_FoodCnt < Constant.ItemMaxCnt)
        {
            testParam_FoodCnt++;
            textFoodCnt.text = "×" + testParam_FoodCnt;
        }

        if (!isFever)
        {
            feverAmount += feverAddAmount;
            feverGage.fillAmount = feverAmount;
            if (feverAmount >= 1)
            {
                isFever = true;

                // フィーバー状態のアニメーション再生
                CancelInvoke("GenerateFood");
                InvokeRepeating("GenerateFood", 1f, 1f);
                feverGage.DOGradientColor(gradientGage, 1f).SetLoops(10, LoopType.Incremental)
                    .OnComplete(() =>
                    {
                        isFever = false;
                        feverGage.color = colorGageDefault;
                        feverAmount = 0;
                        feverGage.fillAmount = 0;

                        CancelInvoke("GenerateFood");
                        InvokeRepeating("GenerateFood", 1f, 3f);
                    });
            }
        }
    }

    public void SubFoodCnt()
    {
        if (isFever) return;

        if (testParam_FoodCnt < Constant.ItemMaxCnt)
        {
            var tmp1 = testParam_FoodCnt - 5;
            testParam_FoodCnt = tmp1 <= 0 ? 0 : tmp1;
            textFoodCnt.text = "×" + testParam_FoodCnt;

            var tmp2 = feverAmount - feverAddAmount * 5;
            feverAmount = tmp2 <= 0 ? 0 : tmp2;
            feverGage.fillAmount = feverAmount;
        }

    }

    public void OnCancelButton()
    {
        //Initiate.Fade("TopScene", Color.black, 1.0f);
        SceneManager.LoadScene("01_TopScene");
    }
}
