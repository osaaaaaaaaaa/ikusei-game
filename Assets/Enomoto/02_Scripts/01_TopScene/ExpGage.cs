using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpGage : MonoBehaviour
{
    [SerializeField] Image imgGage;
    [SerializeField] Text textLevel;
    [SerializeField] Text textExp;

#if UNITY_EDITOR
    public int currentExp;
    public int maxExp;
    public int currentLevel;
#endif

    public void UpdateGage(int currentExp,int maxExp,int currentLevel)
    {
        imgGage.fillAmount = (float)currentExp / (float)maxExp;
        textLevel.text = "ƒŒƒxƒ‹\n" + currentLevel;
        textExp.text = currentExp + "/" + maxExp;
    }
}
