using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultUIController : MonoBehaviour
{
    [SerializeField] Text textExp;
    [SerializeField] Text textHunger;

    private void OnEnable()
    {
        transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
    }

    void SetupText()
    {
        // 100に結果の値を代入する,代入したらこのコメント消しといて
        textExp.DOCounter(0, 100, 1f).SetEase(Ease.Linear);
        textHunger.DOCounter(0, 100, 1f).SetEase(Ease.Linear);
    }

    public void OnOKButton()
    {
        Initiate.Fade("01_TopScene", Color.black, 1.0f);
    }
}
