//---------------------------------------------------------------
//
// 全モンスターリストレスポンス [ MonsterListResponse.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/28
// Update:2024/10/28
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterListResponse
{
    /// ID
    [JsonProperty("id")]
    public int ID { get; set; }

    /// 名前
    [JsonProperty("name")]
    public string Name { get; set; }

    /// 説明
    [JsonProperty("text")]
    public string Text { get; set; }

    /// 進化レベル
    [JsonProperty("evo_lv")]
    public int EvoLv { get; set; }

    /// 進化先ID
    [JsonProperty("evo_id")]
    public int EvoID { get; set; }

    /// レア度
    [JsonProperty("rarity")]
    public string Rarity { get; set; }
}
