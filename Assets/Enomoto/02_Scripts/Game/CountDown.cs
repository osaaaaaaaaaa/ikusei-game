using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CountDown : MonoBehaviour
{
    [SerializeField] List<GameObject> imgNumberList;
    [SerializeField] List<GameObject> imgTextList;
    public float animTime;
    public float initScale;

    public bool isAnimEnd { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isAnimEnd = false;
        InitUI();
        CoutDownAnim();
    }

    /// <summary>
    /// ������
    /// </summary>
    public void InitUI()
    {
        foreach (var img in imgNumberList)
        {
            img.transform.Rotate(new Vector3(0f, 0f, 90f));
            img.transform.localScale = new Vector3(initScale, initScale, 1f);
            img.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }

        foreach (var img in imgTextList)
        {
            img.transform.localScale = new Vector3(initScale * 2, initScale * 2, 1f);
            img.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
    }

    /// <summary>
    /// �A�j���[�V�����J�n
    /// </summary>
    public void CoutDownAnim()
    {
        var _tween = DOTween.Sequence();

        // �i���o�[��Tween
        for (int i = 0; i < imgNumberList.Count; i++)
        {
            var sequence = DOTween.Sequence();
            GameObject hideObj = new GameObject();
            hideObj = imgNumberList[i];

            sequence.Append(imgNumberList[i].transform.DORotate(Vector3.zero, animTime * 2))
                .Join(imgNumberList[i].transform.DOScale(new Vector3(1f, 1f, 1f), animTime * 2))
                .Join(imgNumberList[i].GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), animTime * 2))
                .OnComplete(() => { hideObj.SetActive(false); });

            // sequence���q����
            _tween.Append(sequence);
        }

        // �e�L�X�g��Tween
        for (int i = 0; i < imgTextList.Count; i++)
        {
            var sequence = DOTween.Sequence();
            int curentIndex = new int();
            curentIndex = i;

            sequence.Append(imgTextList[i].transform.DOScale(new Vector3(1f, 1f, 1f), animTime * 2).SetEase(Ease.InOutBack))
                .Join(imgTextList[i].GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), animTime * 2).SetEase(Ease.InOutBack))
                .SetDelay(animTime / 1.5f);

            if (i == imgTextList.Count - 1)
            {
                sequence.OnComplete(() =>
                  {
                      Invoke("HideImgText", animTime);
                  });
            }

            // sequence���q����
            _tween.Join(sequence);
        }

        _tween.OnComplete(() => { isAnimEnd = true; });
        _tween.Play();
    }

    void HideImgText()
    {
        foreach (var img in imgTextList)
        {
            img.SetActive(false);
        }
    }
}
