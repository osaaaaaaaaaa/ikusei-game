using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixManager : MonoBehaviour
{
    [SerializeField] GameObject MenuButtons;

    [SerializeField] GameObject MixList;
    [SerializeField] GameObject MajicalList;


    void Start()
    {
        MenuButtons.SetActive(true);

        MixList.SetActive(false);
        MajicalList.SetActive(false);
    }

    public void OnClickNormalMix()
    {
        MenuButtons.SetActive(false);
        MixList.SetActive(true);
    }

    public void OnClickMajicalMix()
    {
        MenuButtons.SetActive(false);
        MajicalList.SetActive(true);
    }


    public void OnClickBackButton()
    {
        MenuButtons.SetActive(true);

        MixList.SetActive(false);
        MajicalList.SetActive(false);
    }
}
