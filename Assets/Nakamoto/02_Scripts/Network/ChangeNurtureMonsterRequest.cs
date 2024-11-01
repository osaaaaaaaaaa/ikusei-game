//---------------------------------------------------------------------
//
// �i�����N�G�X�g [ EvolutionMonsterRequest.cs ]
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
    /// �琬ID
    [JsonProperty("id")]
    public int ID { get; set; }

    /// �琬���̃����X�^�[ID
    [JsonProperty("monster_id")]
    public int MonsterID { get; set; }

    /// ���O
    [JsonProperty("name")]
    public string Name { get; set; }
}

