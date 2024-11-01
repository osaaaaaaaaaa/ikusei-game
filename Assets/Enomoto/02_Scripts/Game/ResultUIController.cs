using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using KanKikuchi.AudioManager;

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
        // 100‚ÉŒ‹‰Ê‚Ì’l‚ğ‘ã“ü‚·‚é,‘ã“ü‚µ‚½‚ç‚±‚ÌƒRƒƒ“ƒgÁ‚µ‚Æ‚¢‚Ä
        textExp.DOCounter(0, 100, 1f).SetEase(Ease.Linear);
        textHunger.DOCounter(0, 100, 1f).SetEase(Ease.Linear);
    }

    public void OnOKButton()
    {
        BGMManager.Instance.Stop();
        SEManager.Instance.Stop();
        Initiate.Fade("01_TopScene", Color.black, 1.0f);
    }
}
