//---------------------------------------------------------------
//
// 育成行動リクエスト [ ActionRequest.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/28
// Update:2024/10/28
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRequest
{
    /// 育成ID
    [JsonProperty("nurture_id")]
    public int NurtureID { get; set; }

    /// 使用後の値
    [JsonProperty("used_vol")]
    public int UsedVol { get; set; }

    /// 経験値 (現在値 + 獲得量)
    [JsonProperty("exp")]
    public int Exp { get; set; }
}
