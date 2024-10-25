//---------------------------------------------------------------
//
// ユーザー情報レスポンス [ UserInfoResponse.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/24
// Update:2024/10/24
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoResponse
{
    /// ユーザー名
    [JsonProperty("name")]
    public int Name { get; set; }

    /// 食料残量
    [JsonProperty("food_vol")]
    public int FoodVol { get; set; }

    /// 施設レベル
    [JsonProperty("facility_lv")]
    public string FacilityLv { get; set; }

    /// リロール回数
    [JsonProperty("reroll_num")]
    public string RerollNum { get; set; }

    /// 所持金
    [JsonProperty("money")]
    public string Money { get; set; }
}
