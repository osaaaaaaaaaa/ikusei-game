using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class TopSceneManager : MonoBehaviour
{
    #region �^�C�g���֌W
    [SerializeField] GameObject titleSet;
    #endregion

    #region ���j���[��ʊ֌W
    [SerializeField] GameObject menuSet;
    #endregion

    #region �g�b�v��ʊ֌W
    [SerializeField] GameObject topSet;
    #endregion

    [SerializeField] List<GameObject> monsterPrefabs;
    GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        titleSet.SetActive(true);
        menuSet.SetActive(false);
        topSet.SetActive(false);

        GenerateMonster();
    }

    // Update is called once per frame
    void Update()
    {
        if (titleSet.activeSelf && Input.GetMouseButtonDown(0))
        {
            titleSet.SetActive(false);
            ToggleTopVisibility(true);
        }
    }

    /// <summary>
    /// �����X�^�[��������
    /// </summary>
    void GenerateMonster()
    {
        monster = Instantiate(monsterPrefabs[0]);
        monster.GetComponent<Rigidbody2D>().gravityScale = 0;
        monster.transform.position = new Vector2(0f, -1f);
    }

    /// <summary>
    /// �����X�^�[�̎��S����
    /// </summary>
    void KillMonster()
    {

    }

    /// <summary>
    /// ���j���[UI�̕\���E��\��
    /// </summary>
    /// <param name="isVisibility"></param>
    public void ToggleMenuVisibility(bool isVisibility)
    {
        menuSet.SetActive(isVisibility);
    }

    /// <summary>
    /// �g�b�v��ʂ̕\���E��\��
    /// </summary>
    /// <param name="isVisibility"></param>
    public void ToggleTopVisibility(bool isVisibility)
    {
        topSet.SetActive(isVisibility);
    }

    public void OnTrainingButton()
    {
        switch(Random.Range(1, 4))
        {
            case 1:
                Initiate.Fade("GameScene1", Color.black, 1.0f);
                break;
            case 2:
                Initiate.Fade("GameScene2", Color.black, 1.0f);
                break;
            case 3:
                Initiate.Fade("GameScene3", Color.black, 1.0f);
                break;
        }
    }

    public void OnSupplyButton()
    {
        SceneManager.LoadScene("SupplyScene");
    }

    public void OnMealButton()
    {
        Initiate.Fade("MealScene", Color.white, 1.0f);
    }
}
