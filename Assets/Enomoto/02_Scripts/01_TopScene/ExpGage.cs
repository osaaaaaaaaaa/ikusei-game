using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpGage : MonoBehaviour
{
    [SerializeField] Image imgGage;
    [SerializeField] Text textLevel;
    [SerializeField] Text textMaxExp;
    [SerializeField] Text textCurrentExp;

    public void UpdateGage(int currentExp,int maxExp,int currentLevel)
    {
        imgGage.fillAmount = (float)currentExp / (float)maxExp;
        textLevel.text = "ƒŒƒxƒ‹\n<size=78>" + currentLevel + "</size>";
        textMaxExp.text = maxExp.ToString();
        textCurrentExp.text = NetworkManager.Instance.nurtureInfo.Exp.ToString();
    }
}
