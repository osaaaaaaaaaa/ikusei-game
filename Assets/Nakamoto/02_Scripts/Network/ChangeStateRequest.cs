//---------------------------------------------------------------
//
// 育成状態変更リクエスト [ MealRequest.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/30
// Update:2024/10/30
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStateRequest
{
    /// 育成モンスターID
    [JsonProperty("id")]
    public int ID { get; set; }

    /// 育成状態
    [JsonProperty("state")]
    public int State { get; set; }
}
