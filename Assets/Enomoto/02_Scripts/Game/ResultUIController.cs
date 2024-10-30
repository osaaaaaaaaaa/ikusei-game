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
        // 100�Ɍ��ʂ̒l��������,��������炱�̃R�����g�����Ƃ���
        textExp.DOCounter(0, 100, 1f).SetEase(Ease.Linear);
        textHunger.DOCounter(0, 100, 1f).SetEase(Ease.Linear);
    }

    public void OnOKButton()
    {
        Initiate.Fade("01_TopScene", Color.black, 1.0f);
    }
}
