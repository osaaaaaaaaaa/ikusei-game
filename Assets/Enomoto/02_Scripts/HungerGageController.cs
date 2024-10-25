using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerGageController : MonoBehaviour
{
    [SerializeField] Image hungerGage;

    public void UpdateGage(int currentAmount)
    {
        hungerGage.fillAmount = (float)currentAmount / (float)Constant.hungerMaxAmount;
    }
}
