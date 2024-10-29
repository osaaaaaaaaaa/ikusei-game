using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    const int baseHungerIncrease = 10;  // ベースとなる満腹度増加量
    const int baseHungerDecrease = 25;  // ベースとなる満腹度減少量
    const int itemMaxCnt = 999;         // アイテムの最大所持数

    public static int BaseHungerIncrease { get { return baseHungerIncrease; } }
    public static int BaseHungerDecrease { get { return baseHungerDecrease; } }
    public static int ItemMaxCnt { get { return itemMaxCnt; } }

    public const int hungerMaxAmount = 100;     // 満腹度の上限

    /// <summary>
    /// 満腹度増加量を返す
    /// </summary>
    public static int GetHungerIncrease()
    {
        return baseHungerIncrease /*+ 施設レベル*/;
    }

    /// <summary>
    /// 満腹度減少量を返す
    /// </summary>
    public static int GetHungerDecrease()
    {
        return baseHungerDecrease /*+ 施設レベル*/;
    }
}
