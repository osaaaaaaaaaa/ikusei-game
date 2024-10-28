using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SupplyManager : MonoBehaviour
{
    [SerializeField] Text textFoodCnt;
    [SerializeField] Image gage;

    [SerializeField] GameObject generatePoint;
    [SerializeField] List<GameObject> foodPrefabs;

#if UNITY_EDITOR
    int testParam_FoodCnt = 0;
#endif

    // Start is called before the first frame update
    void Start()
    {
        textFoodCnt.text = "Å~" + testParam_FoodCnt;
        InvokeRepeating("GenerateFood", 1f, 3f);
    }

    void GenerateFood()
    {
        int index = (int)Random.Range(0, foodPrefabs.Count);
        GameObject food = Instantiate(foodPrefabs[index], generatePoint.transform);
    }

    public void AddFoodCnt()
    {
        if(testParam_FoodCnt < Constant.ItemMaxCnt)
        {
            testParam_FoodCnt++;
            textFoodCnt.text = "Å~" + testParam_FoodCnt;
        }
    }

    public void OnCancelButton()
    {
        //Initiate.Fade("TopScene", Color.black, 1.0f);
        SceneManager.LoadScene("01_TopScene");
    }
}
