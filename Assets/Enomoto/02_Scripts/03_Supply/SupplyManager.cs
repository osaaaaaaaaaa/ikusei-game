using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SupplyManager : MonoBehaviour
{
    [SerializeField] GameObject generatePoint;
    [SerializeField] List<GameObject> foodPrefabs;

#if UNITY_EDITOR
    public float foodSpeed;
#endif

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GenerateFood", 1f, 3f);
    }

    void GenerateFood()
    {
        int index = (int)Random.Range(0, foodPrefabs.Count);
        GameObject food = Instantiate(foodPrefabs[index], generatePoint.transform);
        food.GetComponent<Food>().Speed = foodSpeed;
    }

    public void OnCancelButton()
    {
        //Initiate.Fade("TopScene", Color.black, 1.0f);
        SceneManager.LoadScene("TopScene");
    }
}
