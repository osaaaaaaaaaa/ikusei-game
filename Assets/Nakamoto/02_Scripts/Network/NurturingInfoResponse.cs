//---------------------------------------------------------------
//
// 育成モンスター情報レスポンス [ NurturingInfoResponse.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/28
// Update:2024/10/28
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NurturingInfoResponse
{
    /// 育成ID
    [JsonProperty("id")]
    public int ID { get; set; }

    /// モンスターID
    [JsonProperty("monster_id")]
    public int MonsterID { get; set; }

    /// 親1
    [JsonProperty("parent1_id")]
    public int Parent1ID { get; set; }

    /// 親2
    [JsonProperty("parent2_id")]
    public int Parent2ID { get; set; }

    /// 名前
    [JsonProperty("name")]
    public string Name { get; set; }

    /// レベル
    [JsonProperty("level")]
    public int Level { get; set; }

    /// 経験値
    [JsonProperty("exp")]
    public string Exp { get; set; }

    /// 満腹度
    [JsonProperty("stomach_vol")]
    public string StomachVol { get; set; }
}
