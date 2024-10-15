using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class ButtonAnimManager : MonoBehaviour
{

    Animator animator;

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
    }

    public void CloseMenuAnim()
    {
        animator.SetBool("OnClickButton", false);
        CloseButton.SetActive(false);
    }

    //

}
