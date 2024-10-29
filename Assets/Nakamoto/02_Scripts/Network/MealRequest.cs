//---------------------------------------------------------------
//
// �H�����N�G�X�g [ MealRequest.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/29
// Update:2024/10/29
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealRequest
{
    /// �琬ID
    [JsonProperty("nurture_id")]
    public int NurtureID { get; set; }

    /// �琬ID
    [JsonProperty("stomach_vol")]
    public int StomachVol { get; set; }

    /// �g�p��̒l
    [JsonProperty("used_vol")]
    public int UsedVol { get; set; }

    /// �o���l (���ݒl + �l����)
    [JsonProperty("exp")]
    public int Exp { get; set; }
}
