using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    const int baseExp = 8;              // ベースとなる経験値獲得量
    const int baseHungerIncrease = 10;  // ベースとなる満腹度増加量
    const int baseHungerDecrease = 20;  // ベースとなる満腹度減少量

    // アイテムの最大所持数
    const int itemMaxCnt = 999;
    public static int ItemMaxCnt { get { return itemMaxCnt; } }

    public const int hungerMaxAmount = 100;     // 満腹度の上限

    /// <summary>
    /// EXPの獲得量を返す
    /// </summary>
    public static int GetExp()
    {
        return baseExp /*+ 施設レベル*/;
    }

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
