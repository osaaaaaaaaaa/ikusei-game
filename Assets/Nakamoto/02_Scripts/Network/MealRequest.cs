//---------------------------------------------------------------
//
// 食事リクエスト [ MealRequest.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/29
// Update:2024/10/29
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealRequest
{
    /// 育成ID
    [JsonProperty("nurture_id")]
    public int NurtureID { get; set; }

    /// 育成ID
    [JsonProperty("stomach_vol")]
    public int StomachVol { get; set; }

    /// 使用後の値
    [JsonProperty("used_vol")]
    public int UsedVol { get; set; }

    /// 経験値 (現在値 + 獲得量)
    [JsonProperty("exp")]
    public int Exp { get; set; }
}
