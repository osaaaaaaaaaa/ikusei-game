using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateRock : MonoBehaviour
{
    public float addAngle;
    void Update()
    {
        transform.Rotate(0f, 0f, addAngle * Time.deltaTime);
    }
}
