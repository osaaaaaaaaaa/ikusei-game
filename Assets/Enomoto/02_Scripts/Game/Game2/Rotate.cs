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
        
    }

    void Update()
    {
        transform.Rotate(0f, 0f, addAngle * Time.deltaTime);
    }
}
