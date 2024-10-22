using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MealManager : MonoBehaviour
{
    [SerializeField] List<GameObject> throwFoodPrefabs;
    public bool isPause { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (isPause) return;

        if (Input.GetMouseButtonDown(0))
        {
            int rndPoint = Random.Range(0, throwFoodPrefabs.Count);
            Instantiate(throwFoodPrefabs[rndPoint], GetTapPosition(), Quaternion.identity);
        }
    }

    Vector2 GetTapPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnCancelButton()
    {
        Initiate.Fade("TopScene", Color.white, 1.0f);
    }

    public void TogglePauseFrag(bool frag)
    {
        isPause = frag;
    }
}
