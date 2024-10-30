//---------------------------------------------------------------------
//
// 育成モンスターID変更リクエスト [ ChangeNurtureMonsterRequest.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/30
// Update:2024/10/30
//
//---------------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeNurtureMonsterRequest
{
    /// 育成ID
    [JsonProperty("id")]
    public int ID { get; set; }

    /// 育成中のモンスターID
    [JsonProperty("monster_id")]
    public int MonsterID { get; set; }
}

