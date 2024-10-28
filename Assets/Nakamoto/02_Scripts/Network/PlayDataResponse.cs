//---------------------------------------------------------------
//
// �v���C�f�[�^���X�|���X [ PlayDataResponse.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/28
// Update:2024/10/28
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDataResponse
{
    /// ���[�U�[���
    [JsonProperty("user_info")]
    public UserInfoResponse UserInfo { get; set; }

    // �琬���
    [JsonProperty("nurture_info")]
    public List<NurturingInfoResponse> NurtureInfo { get; set; }

    // �����X�^�[���X�g
    [JsonProperty("monster_list")]
    public List<MonsterListResponse> MonsterList { get; set; }
}
