//---------------------------------------------------------------
//
// 育成行動レスポンス [ ActionResponse.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/28
// Update:2024/10/28
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionResponse
{
    /// レベル
    [JsonProperty("level")]
    public int Level { get; set; }

    /// 余り経験値
    [JsonProperty("exp")]
    public int Exp { get; set; }
}
