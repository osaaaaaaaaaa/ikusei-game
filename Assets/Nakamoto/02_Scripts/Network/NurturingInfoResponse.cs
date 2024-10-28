//---------------------------------------------------------------
//
// �琬�����X�^�[��񃌃X�|���X [ NurturingInfoResponse.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/28
// Update:2024/10/28
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NurturingInfoResponse
{
    /// �琬ID
    [JsonProperty("id")]
    public int ID { get; set; }

    /// �����X�^�[ID
    [JsonProperty("monster_id")]
    public int MonsterID { get; set; }

    /// �e1
    [JsonProperty("parent1_id")]
    public int Parent1ID { get; set; }

    /// �e2
    [JsonProperty("parent2_id")]
    public int Parent2ID { get; set; }

    /// ���O
    [JsonProperty("name")]
    public string Name { get; set; }

    /// ���x��
    [JsonProperty("level")]
    public int Level { get; set; }

    /// �o���l
    [JsonProperty("exp")]
    public string Exp { get; set; }

    /// �����x
    [JsonProperty("stomach_vol")]
    public string StomachVol { get; set; }
}
