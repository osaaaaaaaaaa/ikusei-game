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

    const float feverAddAmount = 0.05f;
    float feverAmount = 0;
    public bool isFever { get; private set; }

    private int foodCnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        isFever = false;

        colorGageDefault = feverGage.color;
        textFoodCnt.text = "�~" + foodCnt;
        InvokeRepeating("GenerateFood", 1f, 2f);
    }

    /// <summary>
    /// �H�ו��𐶐�����
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
        if (foodCnt < Constant.ItemMaxCnt)
        {
            foodCnt++;
            textFoodCnt.text = "�~" + foodCnt;
        }

        if (!isFever)
        {
            feverAmount += feverAddAmount;
            feverGage.fillAmount = feverAmount;
            if (feverAmount >= 1)
            {
                isFever = true;

                // �t�B�[�o�[��Ԃ̃A�j���[�V�����Đ�
                CancelInvoke("GenerateFood");
                InvokeRepeating("GenerateFood", 1f, 1f);
                feverGage.DOGradientColor(gradientGage, 1f).SetLoops(20, LoopType.Incremental)
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

        if (foodCnt < Constant.ItemMaxCnt)
        {
            var tmp1 = foodCnt - 5;
            foodCnt = tmp1 <= 0 ? 0 : tmp1;
            textFoodCnt.text = "�~" + foodCnt;

            var tmp2 = feverAmount - feverAddAmount * 5;
            feverAmount = tmp2 <= 0 ? 0 : tmp2;
            feverGage.fillAmount = feverAmount;
        }

    }

    public void OnCancelButton()
    {
        // ���݂̐H���l���X�V
        int foodVol = NetworkManager.Instance.userInfo.FoodVol + foodCnt;

        if (foodVol > Constant.ItemMaxCnt)
        {   // ����l�𒴂��Ȃ��悤�ɂ���
            foodVol = Constant.ItemMaxCnt;
        }

        StartCoroutine(NetworkManager.Instance.SupplyFoods(
                foodVol,
                result =>
                {
                    if (result)
                    {
                        Debug.Log("�H���l�X�V����");
                        SceneManager.LoadScene("01_TopScene");
                    }
                    else
                    {
                        Debug.Log("�H���l�X�V���s");
                        SceneManager.LoadScene("01_TopScene");
                    }
                }));
    }
}
