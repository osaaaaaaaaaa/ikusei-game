using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    public const int hungerMaxAmount = 100;      // 満腹度の上限
    public const int baseHungerIncrease = 20;    // ベースとなる満腹度増加量
    public const int baseHungerDecrease = 10;    // ベースとなる満腹度減少量
    public const int itemMaxCnt = 999;           // アイテムの最大所持数

    enum EGG_HACHING_TIME_RARITY
    {
        SSR = 4200,
        SR = 600,
        R = 30,
        N = 10
    }

    /// <summary>
    /// 孵化する時間を取得
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
