//---------------------------------------------------------------
//
// ユーザー登録レスポンスクラス [ StoreUserResponse.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/25
// Update:2024/10/25
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUserResponse
{
    [JsonProperty("token")]
    public string Token { get; set; }
}
