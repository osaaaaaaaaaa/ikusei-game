using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class ButtonAnimManager : MonoBehaviour
{

    Animator animator;

    [SerializeField] GameObject TapgGuard;

    [SerializeField] GameObject MenuButton;
    [SerializeField] GameObject CloseButton;

    [SerializeField] GameObject LibButton;
    [SerializeField] GameObject ShopButton;
    [SerializeField] GameObject GrowButton;
    [SerializeField] GameObject MixButton;
    [SerializeField] GameObject ItemButton;


    void Start()
    {
        animator = GetComponent<Animator>();

        TapgGuard.SetActive(false);

        MenuButton.SetActive(true);
        CloseButton.SetActive(false);

        LibButton.SetActive(false);
        ShopButton.SetActive(false);
        GrowButton.SetActive(false);
        MixButton.SetActive(false);
        ItemButton.SetActive(false);
    }

    //------------------------------------------------
    //       メニューバー
    //------------------------------------------------
    public void StartMenuAnim()
    {
        //menu.SetActive(true);
        animator.SetBool("OnClickButton", true);
        CloseButton.SetActive(true);

        TapgGuard.SetActive(true);

        //各種ボタン
        LibButton.SetActive(true);
        ShopButton.SetActive(true);
        GrowButton.SetActive(true);
        MixButton.SetActive(true);
        ItemButton.SetActive(true);
    }

    public void CloseMenuAnim()
    {
        animator.SetBool("OnClickButton", false);
        CloseButton.SetActive(false);
        Invoke("SetOnActiveButton", 0.5f);
    }

    //
    private void SetOnActiveButton()
    {
        TapgGuard.SetActive(false);

        LibButton.SetActive(false);
        ShopButton.SetActive(false);
        GrowButton.SetActive(false);
        MixButton.SetActive(false);
        ItemButton.SetActive(false);
    }

}
