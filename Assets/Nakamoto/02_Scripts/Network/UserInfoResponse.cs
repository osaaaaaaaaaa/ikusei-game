//---------------------------------------------------------------
//
// ���[�U�[��񃌃X�|���X [ UserInfoResponse.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/24
// Update:2024/10/24
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoResponse
{
    /// ���[�U�[��
    [JsonProperty("name")]
    public int Name { get; set; }

    /// �H���c��
    [JsonProperty("food_vol")]
    public int FoodVol { get; set; }

    /// �{�݃��x��
    [JsonProperty("facility_lv")]
    public string FacilityLv { get; set; }

    /// �����[����
    [JsonProperty("reroll_num")]
    public string RerollNum { get; set; }

    /// ������
    [JsonProperty("money")]
    public string Money { get; set; }
}
