using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CountDown : MonoBehaviour
{
    [SerializeField] List<GameObject> imgNumberList;
    [SerializeField] List<GameObject> imgTextList;

#if UNITY_EDITOR
    public float animTime;
    public float initScale;
#endif

    public bool isAnimEnd { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isAnimEnd = false;
        InitUI();
        CoutDownAnim();
    }

    /// <summary>
    /// 初期化
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
    /// アニメーション開始
    /// </summary>
    public void CoutDownAnim()
    {
        var _tween = DOTween.Sequence();

        foreach (var img in imgNumberList)
        {
            var sequence = DOTween.Sequence();
            GameObject hideObj = new GameObject();
            hideObj = img;

            sequence.Append(img.transform.DORotate(Vector3.zero, animTime * 2))
                .Join(img.transform.DOScale(new Vector3(1f, 1f, 1f), animTime * 2))
                .Join(img.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), animTime * 2))
                .OnComplete(() => { hideObj.SetActive(false); });

            // sequenceを繋げる
            _tween.Append(sequence);
        }

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

            // sequenceを繋げる
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
