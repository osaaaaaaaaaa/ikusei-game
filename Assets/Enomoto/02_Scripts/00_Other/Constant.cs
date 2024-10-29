using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    public const int hungerMaxAmount = 100;      // �����x�̏��
    public const int baseHungerIncrease = 20;    // �x�[�X�ƂȂ閞���x������
    public const int baseHungerDecrease = 10;    // �x�[�X�ƂȂ閞���x������
    public const int itemMaxCnt = 999;           // �A�C�e���̍ő及����

    enum EGG_HACHING_TIME_RARITY
    {
        SSR = 4200,
        SR = 600,
        R = 30,
        N = 10
    }

    /// <summary>
    /// �z�����鎞�Ԃ��擾
    /// </summary>
    /// <returns></returns>
    public static int GetEggHachingTimer(string rarity)
    {
        switch (rarity)
        {
            case "SSR":
                return (int)EGG_HACHING_TIME_RARITY.SSR;
            case "SR":
                return (int)EGG_HACHING_TIME_RARITY.SR;
            case "R":
                return (int)EGG_HACHING_TIME_RARITY.R;
            case "N":
                return (int)EGG_HACHING_TIME_RARITY.N;
            default:
                return 0;
        }
    }
}
