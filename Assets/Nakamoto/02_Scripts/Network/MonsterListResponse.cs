//---------------------------------------------------------------
//
// �S�����X�^�[���X�g���X�|���X [ MonsterListResponse.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/28
// Update:2024/10/28
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterListResponse
{
    /// ID
    [JsonProperty("id")]
    public int ID { get; set; }

    /// ���O
    [JsonProperty("name")]
    public string Name { get; set; }

    /// ����
    [JsonProperty("text")]
    public string Text { get; set; }

    /// �i�����x��
    [JsonProperty("evo_lv")]
    public int EvoLv { get; set; }

    /// �i����ID
    [JsonProperty("evo_id")]
    public int EvoID { get; set; }

    /// ���A�x
    [JsonProperty("rarity")]
    public string Rarity { get; set; }
}
