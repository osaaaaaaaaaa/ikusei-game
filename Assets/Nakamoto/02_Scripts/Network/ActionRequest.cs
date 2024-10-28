//---------------------------------------------------------------
//
// �琬�s�����N�G�X�g [ ActionRequest.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/28
// Update:2024/10/28
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRequest
{
    /// �琬ID
    [JsonProperty("nurture_id")]
    public int NurtureID { get; set; }

    /// �g�p��̒l
    [JsonProperty("used_vol")]
    public int UsedVol { get; set; }

    /// �o���l (���ݒl + �l����)
    [JsonProperty("exp")]
    public int Exp { get; set; }
}
