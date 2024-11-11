using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using KanKikuchi.AudioManager;

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
    const int normalGetVol = 5;
    const int feverGetVol = 15;
    float feverAmount = 0;
    public bool isFever { get; private set; }

    private int foodCnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        isFever = false;

        colorGageDefault = feverGage.color;
        textFoodCnt.text = foodCnt.ToString();
        InvokeRepeating("GenerateFood", 1f, 1.5f);

        BGMManager.Instance.Play(BGMPath.SUPPLY_NORMAL, 1);
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
        if (foodCnt < Constant.itemMaxCnt)
        {
            if(isFever) { foodCnt = foodCnt + feverGetVol; }
            else { foodCnt = foodCnt + normalGetVol; }

            textFoodCnt.text = foodCnt.ToString();
        }

        if (!isFever)
        {
            feverAmount += feverAddAmount;
            feverGage.fillAmount = feverAmount;
            if (feverAmount >= 1)
            {
                isFever = true;

                BGMManager.Instance.Play(BGMPath.SUPPLY_FEVER);

                // フィーバー状態のアニメーション再生
                CancelInvoke("GenerateFood");
                InvokeRepeating("GenerateFood", 1f, 1f);
                feverGage.DOGradientColor(gradientGage, 1f).SetLoops(10, LoopType.Incremental)
                    .OnComplete(() =>
                    {
                        BGMManager.Instance.FadeOut(BGMPath.SUPPLY_FEVER,1);
                        BGMManager.Instance.Play(BGMPath.SUPPLY_NORMAL,1);

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

        if (foodCnt < Constant.itemMaxCnt)
        {
            var tmp1 = foodCnt - 5;
            foodCnt = tmp1 <= 0 ? 0 : tmp1;
            textFoodCnt.text = foodCnt.ToString();

            var tmp2 = feverAmount - feverAddAmount * 5;
            feverAmount = tmp2 <= 0 ? 0 : tmp2;
            feverGage.fillAmount = feverAmount;
        }

    }

    public void OnCancelButton()
    {
        // 現在の食料値を更新
        int foodVol = NetworkManager.Instance.userInfo.FoodVol + foodCnt;

        if (foodVol > Constant.itemMaxCnt)
        {   // 上限値を超えないようにする
            foodVol = Constant.itemMaxCnt;
        }

        StartCoroutine(NetworkManager.Instance.SupplyFoods(
                foodVol,
                result =>
                {
                    if (result)
                    {
                        Debug.Log("食料値更新完了");
                        SceneManager.LoadScene("01_TopScene");
                    }
                    else
                    {
                        Debug.Log("食料値更新失敗");
                        SceneManager.LoadScene("01_TopScene");
                    }
                }));
    }
}
