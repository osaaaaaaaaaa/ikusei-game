//---------------------------------------------------------------
//
// ネットワークマネージャー [ NetWorkManager.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/24
// Update:2024/10/24
//
//---------------------------------------------------------------
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    //-------------------------------------------------------------------
    // フィールド

    /// <summary>
    /// APIベースURL
    /// </summary>
    //const string API_BASE_URL = "https://api-shot.japaneast.cloudapp.azure.com/api/";
    const string API_BASE_URL = "http://localhost:8000/api/";


    /// <summary>
    /// getプロパティを呼び出した初回時にインスタンス生成してstaticで保持
    /// </summary>
    private static NetworkManager instance;

    /// <summary>
    /// プレイ中のユーザー名
    /// </summary>
    private string userName = "";

    /// <summary>
    /// API認証トークン
    /// </summary>
    private string authToken = "";

    /// <summary>
    /// NetworkManagerプロパティ
    /// </summary>
    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                // GameObjectを生成し、NetworkManagerを追加
                GameObject gameObject = new GameObject("NetworkManager");
                instance = gameObject.AddComponent<NetworkManager>();

                // シーン遷移で破棄されないように設定
                DontDestroyOnLoad(gameObject);
            }

            return instance;
        }
    }

    //-------------------------------------------------------------------
    // メソッド

    /// <summary>
    /// ユーザーデータ保存処理
    /// </summary>
    private void SaveUserData()
    {
        // セーブデータクラスの生成
        SaveData saveData = new SaveData();
        saveData.UserName = this.userName;
        saveData.AuthToken = this.authToken;

        // データをJSONシリアライズ
        string json = JsonConvert.SerializeObject(saveData);

        // 指定した絶対パスに"saveData.json"を保存
        var writer = new StreamWriter(Application.persistentDataPath + "/saveData.json");
        writer.Write(json); // 書き出し
        writer.Flush();     // バッファに残っている値を全て書き出し
        writer.Close();     // ファイル閉
    }

    /// <summary>
    /// ユーザーデータ読み込み処理
    /// </summary>
    /// <returns></returns>
    public bool LoadUserData()
    {
        if (!File.Exists(Application.persistentDataPath + "/saveData.json"))
        {   // 指定のパスのファイルが存在しなかった時、早期リターン
            Debug.Log("セーブデータ無し");
            return false;
        }

        //  ローカルファイルからユーザーデータの読込処理
        var reader = new StreamReader(Application.persistentDataPath + "/saveData.json");
        string json = reader.ReadToEnd();
        reader.Close();

        // セーブデータJSONをデシリアライズして取得
        SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
        this.userName = saveData.UserName;
        this.authToken = saveData.AuthToken;

        Debug.Log("セーブデータ読み込み完了");

        // 読み込み結果をリターン
        return true;
    }

    //=============================
    // GET処理

    /// <summary>
    /// ユーザー情報取得処理
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator GetUserInfo(Action<List<UserInfoResponse>> result)
    {
        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "users/show");
        request.SetRequestHeader("Authorization", "Bearer" + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        // 受信情報格納用
        List<UserInfoResponse> response = new List<UserInfoResponse>();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // 通信が成功した時

            string resultJson = request.downloadHandler.text;   // レスポンスボディ(json)の文字列を取得
            response = JsonConvert.DeserializeObject<List<UserInfoResponse>>(resultJson);  // JSONデシリアライズ
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(response);
    }

    //=============================
    // POST処理

    /// <summary>
    /// ユーザー登録処理
    /// </summary>
    /// <param name="name">ユーザー名</param>
    /// <param name="result">通信完了辞に呼び出す関数</param>
    /// <returns></returns>
    public IEnumerator StoreUser(string name, Action<bool> result)
    {
        // サーバーに送信するオブジェクトを作成
        StoreUserRequest repuestData = new StoreUserRequest();
        repuestData.Name = name;    // 名前を代入

        // サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(repuestData);

        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/store", json, "application/json");
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        bool isSuccess = false; // 受信結果

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // 通信が成功した場合、帰ってきたJSONをオブジェクトに変換
            string resultJson = request.downloadHandler.text;   // レスポンスボディ(json)の文字列を取得
            StoreUserResponse response = JsonConvert.DeserializeObject<StoreUserResponse>(resultJson);  // JSONデシリアライズ

            // ファイルにユーザーデータを保存
            this.userName = name;
            this.authToken = response.Token;
            SaveUserData();
            isSuccess = true;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(isSuccess);
    }
}
