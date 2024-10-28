//---------------------------------------------------------------
//
// ミラクル配合リクエスト [ MixMiracle.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/28
// Update:2024/10/28
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixMiracleRequest
{
    /// 育成ID
    [JsonProperty("nurture_id")]
    public int NurtureID { get; set; }
}
