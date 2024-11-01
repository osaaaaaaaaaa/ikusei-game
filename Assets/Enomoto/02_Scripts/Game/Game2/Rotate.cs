using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KanKikuchi.AudioManager;

public class Rotate : MonoBehaviour
{
    public float addAngle;

    private void Start()
    {
        SEManager.Instance.Play(SEPath.ROCK_ROTATE, 1, 0, 1, true);
    }

    void Update()
    {
        transform.Rotate(0f, 0f, addAngle * Time.deltaTime);
    }
}
