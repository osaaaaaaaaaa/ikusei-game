using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MealManager : MonoBehaviour
{
    [SerializeField] GameObject throwFoodPrefab;
    public bool isPause { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (isPause) return;

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(throwFoodPrefab, GetTapPosition(), Quaternion.identity);
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
