using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingUIManager : MonoBehaviour
{
    //************************************
    //  �p�^�[���P - �O���O�� [6�`]
    //************************************

    private const float DURATION = 1f;  // ���[�f�B���O�A�j���[�V�����̊Ԋu

    void Start()
    {
        Image[] circles = GetComponentsInChildren<Image>(); // �q�I�u�W�F�N�g���擾����

        for (var i = 0; i < circles.Length; i++)
        {// �擾�����摜�̖��������[�v
            var angle = -2 * Mathf.PI * i / circles.Length;
            circles[i].rectTransform.anchoredPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 50f;
            circles[i].DOFade(0f, DURATION).SetLoops(-1, LoopType.Yoyo).SetDelay(DURATION * i / circles.Length);
        }
    }


    //************************************
    //  �p�^�[���Q - �g���C�A���O�� [3]
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
    //  �p�^�[���R - �L�����ďW�܂� [4]
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
    //  �p�^�[���S - ���ԂɊg��k�� [3�`]
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
    //  �p�^�[���T - ���Ƀt�F�[�h [3�`]
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
    //  �p�^�[���U - ��]���ăt�F�[�h [3�`]
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
    //  �p�^�[���V - 3D��Ԃ���]���� [3�`]
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
    //  �p�^�[���W - ���ɒ��� [3�`]
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

