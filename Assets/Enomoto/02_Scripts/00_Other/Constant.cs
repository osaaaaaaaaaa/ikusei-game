using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    const int baseExp = 8;              // �x�[�X�ƂȂ�o���l�l����
    const int baseHungerIncrease = 10;  // �x�[�X�ƂȂ閞���x������
    const int baseHungerDecrease = 20;  // �x�[�X�ƂȂ閞���x������

    // �A�C�e���̍ő及����
    const int itemMaxCnt = 999;
    public int ItemMaxCnt { get { return itemMaxCnt; } }

    /// <summary>
    /// EXP�̊l���ʂ�Ԃ�
    /// </summary>
    public static int GetExp()
    {
        return baseExp;
    }

    /// <summary>
    /// �����x�����ʂ̊l���ʂ�Ԃ�
    /// </summary>
    public static int GetHungerIncrease()
    {
        return baseHungerIncrease;
    }

    /// <summary>
    /// �����x�����ʂ̊l���ʂ�Ԃ�
    /// </summary>
    public static int GetHungerDecrease()
    {
        return baseHungerDecrease;
    }
}
