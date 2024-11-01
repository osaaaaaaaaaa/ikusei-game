//---------------------------------------------------------------------
//
// 進化リクエスト [ EvolutionMonsterRequest.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/30
// Update:2024/10/30
//
//---------------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionMonsterRequest
{
    /// 育成ID
    [JsonProperty("id")]
    public int ID { get; set; }

    /// 育成中のモンスターID
    [JsonProperty("monster_id")]
    public int MonsterID { get; set; }

    /// 名前
    [JsonProperty("name")]
    public string Name { get; set; }
}

