//---------------------------------------------------------------
//
// �琬�s�����X�|���X [ ActionResponse.cs ]
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
    /// ���x��
    [JsonProperty("level")]
    public int Level { get; set; }

    /// �]��o���l
    [JsonProperty("exp")]
    public int Exp { get; set; }
}
