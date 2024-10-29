using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    const int baseHungerIncrease = 10;  // �x�[�X�ƂȂ閞���x������
    const int baseHungerDecrease = 25;  // �x�[�X�ƂȂ閞���x������
    const int itemMaxCnt = 999;         // �A�C�e���̍ő及����

    public static int BaseHungerIncrease { get { return baseHungerIncrease; } }
    public static int BaseHungerDecrease { get { return baseHungerDecrease; } }
    public static int ItemMaxCnt { get { return itemMaxCnt; } }

    public const int hungerMaxAmount = 100;     // �����x�̏��

    /// <summary>
    /// �����x�����ʂ�Ԃ�
    /// </summary>
    public static int GetHungerIncrease()
    {
        return baseHungerIncrease /*+ �{�݃��x��*/;
    }

    /// <summary>
    /// �����x�����ʂ�Ԃ�
    /// </summary>
    public static int GetHungerDecrease()
    {
        return baseHungerDecrease /*+ �{�݃��x��*/;
    }
}
