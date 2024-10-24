using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MealManager : MonoBehaviour
{
    [SerializeField] List<GameObject> throwFoodPrefabs;
    [SerializeField] List<GameObject> monsterPrefabs;
    GameObject monster;

    public bool isPause { get; private set; }

    private void Start()
    {
        GenerateMonster();
    }

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

    /// <summary>
    /// ÉÇÉìÉXÉ^Å[ê∂ê¨èàóù
    /// </summary>
    void GenerateMonster()
    {
        monster = Instantiate(monsterPrefabs[0]);
        monster.GetComponent<Rigidbody2D>().gravityScale = 0;
        monster.transform.position = new Vector2(0f, -1f);
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
