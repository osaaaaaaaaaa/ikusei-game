using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingUIManager : MonoBehaviour
{
    //************************************
    //  パターン１ - グルグル [6～]
    //************************************

    private const float DURATION = 1f;  // ローディングアニメーションの間隔

    void Start()
    {
        Image[] circles = GetComponentsInChildren<Image>(); // 子オブジェクトを取得する

        for (var i = 0; i < circles.Length; i++)
        {// 取得した画像の枚数分ループ
            var angle = -2 * Mathf.PI * i / circles.Length;
            circles[i].rectTransform.anchoredPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 50f;
            circles[i].DOFade(0f, DURATION).SetLoops(-1, LoopType.Yoyo).SetDelay(DURATION * i / circles.Length);
        }
    }


    //************************************
    //  パターン２ - トライアングル [3]
    //************************************

    //private const float DURATION = 1f;

    //void Start()
    //{
    //    Image[] circles = GetComponentsInChildren<Image>();
    //    for (var i = 0; i < circles.Length; i++)
    //    {
    //        circles[i].rectTransform.anchoredPosition = new Vector2((i - circles.Length / 2) * 50f, 0);
    //        Sequence sequence = DOTween.Sequence()
    //            .SetLoops(-1, LoopType.Restart)
    //            .SetDelay((DURATION / 2) * ((float)i / circles.Length))
    //            .Append(circles[i].rectTransform.DOAnchorPosY(30f, DURATION / 4))
    //            .Append(circles[i].rectTransform.DOAnchorPosY(0f, DURATION / 4))
    //            .AppendInterval((DURATION / 2) * ((float)(1 - i) / circles.Length));
    //        sequence.Play();
    //    }
    //}


    //************************************
    //  パターン３ - 広がって集まる [4]
    //************************************

    //private const float DURATION = 0.5f;

    //void Start()
    //{
    //    Image[] circles = GetComponentsInChildren<Image>();
    //    for (var i = 0; i < circles.Length; i++)
    //    {
    //        var angle = -2 * Mathf.PI * i / circles.Length;
    //        circles[i].rectTransform.anchoredPosition = Vector2.zero;
    //        Sequence sequence = DOTween.Sequence()
    //            .SetLoops(-1, LoopType.Yoyo)
    //            .AppendInterval(DURATION / 4)
    //            .Append(circles[i].rectTransform.DOAnchorPos(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 50f, DURATION / 2))
    //            .AppendInterval(DURATION / 4);
    //        sequence.Play();
    //    }

    //    Sequence sequenceParent = DOTween.Sequence()
    //            .SetLoops(-1, LoopType.Incremental)
    //            .Append(transform.DOLocalRotate(Vector3.forward * (180f / circles.Length), DURATION / 4))
    //            .AppendInterval(DURATION / 2)
    //            .Append(transform.DOLocalRotate(Vector3.forward * (180f / circles.Length), DURATION / 4));
    //    sequenceParent.Play();
    //}


    //************************************
    //  パターン４ - 順番に拡大縮小 [3～]
    //************************************

    //private const float DURATION = 1f;

    //void Start()
    //{
    //    Image[] circles = GetComponentsInChildren<Image>();
    //    for (var i = 0; i < circles.Length; i++)
    //    {
    //        circles[i].rectTransform.anchoredPosition = new Vector2((i - circles.Length / 2) * 50f, 0);
    //        Sequence sequence = DOTween.Sequence()
    //            .SetLoops(-1, LoopType.Restart)
    //            .SetDelay((DURATION / 2) * ((float)i / circles.Length))
    //            .Append(circles[i].rectTransform.DOScale(1.5f, DURATION / 4))
    //            .Append(circles[i].rectTransform.DOScale(1f, DURATION / 4))
    //            .AppendInterval((DURATION / 2) * ((float)(1 - i) / circles.Length));
    //        sequence.Play();
    //    }
    //}


    //************************************
    //  パターン５ - 順にフェード [3～]
    //************************************

    //private const float DURATION = 1f;

    //void Start()
    //{
    //    Image[] circles = GetComponentsInChildren<Image>();
    //    for (var i = 0; i < circles.Length; i++)
    //    {
    //        circles[i].rectTransform.anchoredPosition = new Vector2((i - circles.Length / 2) * 50f, 0);
    //        Sequence sequence = DOTween.Sequence()
    //            .SetLoops(-1, LoopType.Restart)
    //            .SetDelay((DURATION / 2) * ((float)i / circles.Length))
    //            .Append(circles[i].DOFade(0f, DURATION / 4))
    //            .Append(circles[i].DOFade(1f, DURATION / 4))
    //            .AppendInterval((DURATION / 2) * ((float)(1 - i) / circles.Length));
    //        sequence.Play();
    //    }
    //}

    //***************************************
    //  パターン６ - 回転してフェード [3～]
    //***************************************

    //private const float DURATION = 0.5f;

    //void Start()
    //{
    //    Image[] circles = GetComponentsInChildren<Image>();
    //    for (var i = 0; i < circles.Length; i++)
    //    {
    //        var angle = 2 * Mathf.PI * i / circles.Length;
    //        circles[i].rectTransform.anchoredPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 50f;
    //        Sequence sequence = DOTween.Sequence()
    //            .SetLoops(-1, LoopType.Yoyo)
    //            .AppendInterval(DURATION / 4)
    //            .Append(circles[i].DOFade(0f, DURATION / 2))
    //            .AppendInterval(DURATION / 4);
    //        sequence.Play();
    //    }

    //    Sequence sequenceParent = DOTween.Sequence()
    //            .SetLoops(-1, LoopType.Incremental)
    //            .Append(transform.DOLocalRotate(Vector3.forward * (360f / circles.Length), DURATION / 4))
    //            .AppendInterval(DURATION / 2)
    //            .Append(transform.DOLocalRotate(Vector3.forward * (360f / circles.Length), DURATION / 4));
    //    sequenceParent.Play();
    //}


    //***************************************
    //  パターン７ - 3D空間を回転する [3～]
    //***************************************

    //private const float DURATION = 1f;

    //void Start()
    //{
    //    Image[] circles = GetComponentsInChildren<Image>();
    //    for (var i = 0; i < circles.Length; i++)
    //    {
    //        var angle = 2 * Mathf.PI * i / circles.Length;
    //        circles[i].rectTransform.anchoredPosition3D = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * 50f;
    //        circles[i].rectTransform.DOLocalRotate(-Vector3.up * (360f / circles.Length), DURATION).SetLoops(-1);
    //        circles[i].color = new Color(1f, 1f, 1f, 0.7f);
    //    }

    //    GetComponent<Transform>().DOLocalRotate(Vector3.up * (360f / circles.Length), DURATION).SetLoops(-1);
    //}


    //***************************************
    //  パターン８ - 順に跳ぶ [3～]
    //***************************************

    //private const float DURATION = 1f;

    //void Start()
    //{
    //    Image[] circles = GetComponentsInChildren<Image>();
    //    for (var i = 0; i < circles.Length; i++)
    //    {
    //        circles[i].rectTransform.anchoredPosition = new Vector2((i - circles.Length / 2) * 50f, 0);
    //        Sequence sequence = DOTween.Sequence()
    //            .SetLoops(-1, LoopType.Restart)
    //            .SetDelay((DURATION / 2) * ((float)i / circles.Length))
    //            .Append(circles[i].rectTransform.DOAnchorPosY(30f, DURATION / 4))
    //            .Append(circles[i].rectTransform.DOAnchorPosY(0f, DURATION / 4))
    //            .AppendInterval((DURATION / 2) * ((float)(1 - i) / circles.Length));
    //        sequence.Play();
    //    }
    //}
}

