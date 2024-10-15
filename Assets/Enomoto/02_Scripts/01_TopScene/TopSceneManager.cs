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

    // Start is called before the first frame update
    void Start()
    {
        titleSet.SetActive(true);
        menuSet.SetActive(false);
        topSet.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (titleSet.activeSelf && Input.GetMouseButtonDown(0))
        {
            titleSet.SetActive(false);
            OnToggleTopVisibility(true);
        }
    }

    public void OnToggleMenuVisibility(bool isVisibility)
    {
        menuSet.SetActive(isVisibility);
    }

    public void OnToggleTopVisibility(bool isVisibility)
    {
        topSet.SetActive(isVisibility);
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
