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

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Init(currentExp, maxExp, currentLevel);
#endif
    }

    public void Init(int currentExp,int maxExp,int currentLevel)
    {
        imgGage.fillAmount = (float)currentExp / (float)maxExp;
        textLevel.text = "ƒŒƒxƒ‹\n" + currentLevel;
        textExp.text = currentExp + "/" + maxExp;
    }
}
