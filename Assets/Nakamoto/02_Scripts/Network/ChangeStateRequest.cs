//---------------------------------------------------------------
//
// �琬��ԕύX���N�G�X�g [ MealRequest.cs ]
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
    /// �琬�����X�^�[ID
    [JsonProperty("id")]
    public int ID { get; set; }

    /// �琬���
    [JsonProperty("state")]
    public int State { get; set; }
}
