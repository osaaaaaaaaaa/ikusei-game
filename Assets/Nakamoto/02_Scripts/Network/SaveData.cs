//---------------------------------------------------------------
//
// セーブデータクラス [ SaveData.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/24
// Update:2024/10/24
//
//---------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    /// <summary>
    /// ユーザー名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 認証トークン
    /// </summary>
    public string AuthToken { get; set; }
}