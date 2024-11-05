//---------------------------------------------------------------
//
// ネットワークマネージャー [ NetWorkManager.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/24
// Update:2024/10/30
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
    const string API_BASE_URL = "https://ikusei.japaneast.cloudapp.azure.com/api/";

    /// <summary>
    /// getプロパティを呼び出した初回時にインスタンス生成してstaticで保持
    /// </summary>
    private static NetworkManager instance;

    //----------------------------------------------------------------------------------
    // ユーザー情報

    /// <summary>
    /// プレイ中のユーザー名
    /// </summary>
    private string userName = "";

    /// <summary>
    /// API認証トークン
    /// </summary>
    private string authToken = "";

    /// <summary>
    /// モンスターマスターデータ
    /// </summary>
    public List<MonsterListResponse> monsterList { get; private set; } = new List<MonsterListResponse>();

    /// <summary>
    /// ユーザー情報
    /// </summary>
    public UserInfoResponse userInfo { get; set; } = new UserInfoResponse();

    /// <summary>
    /// 育成モンスター情報
    /// </summary>
    public NurturingInfoResponse nurtureInfo { get; set; } = new NurturingInfoResponse();

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
    public IEnumerator GetUserInfo(Action<UserInfoResponse> result)
    {
        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "users/show");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        // 受信情報格納用
        UserInfoResponse response = new UserInfoResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // 通信が成功した時

            string resultJson = request.downloadHandler.text;   // レスポンスボディ(json)の文字列を取得
            response = JsonConvert.DeserializeObject<UserInfoResponse>(resultJson);  // JSONデシリアライズ

            userInfo = response;
        }
        else
        {   // 通信失敗時はnull
            response = null;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(response);
    }

    /// <summary>
    /// 育成情報取得処理
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator GetNurturing(Action<List<NurturingInfoResponse>> result)
    {
        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "monsters/nurturing");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        // 受信情報格納用
        List<NurturingInfoResponse> response = new List<NurturingInfoResponse>();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // 通信が成功した時

            string resultJson = request.downloadHandler.text;   // レスポンスボディ(json)の文字列を取得
            response = JsonConvert.DeserializeObject<List<NurturingInfoResponse>>(resultJson);  // JSONデシリアライズ

            nurtureInfo = response[0];  // 取得情報を保存
        }
        else
        {   // 通信失敗時はnull
            response = null;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(response);
    }

    /// <summary>
    /// モンスターのマスター情報取得
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator GetMonsterInfo(Action<List<MonsterListResponse>> result)
    {
        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "monsters");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        // 受信情報格納用
        List<MonsterListResponse> response = new List<MonsterListResponse>();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // 通信が成功した時

            string resultJson = request.downloadHandler.text;   // レスポンスボディ(json)の文字列を取得
            response = JsonConvert.DeserializeObject<List<MonsterListResponse>>(resultJson);  // JSONデシリアライズ

            monsterList = response;
        }
        else
        {   // 通信失敗時はnull
            response = null;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(response);
    }

    /// プレイデータ取得
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator GetPlayData(Action<PlayDataResponse> result)
    {
        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "users/play-data");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        // 受信情報格納用
        PlayDataResponse response = new PlayDataResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // 通信が成功した時

            string resultJson = request.downloadHandler.text;   // レスポンスボディ(json)の文字列を取得
            response = JsonConvert.DeserializeObject<PlayDataResponse>(resultJson);  // JSONデシリアライズ

            userInfo = response.UserInfo;
            nurtureInfo = response.NurtureInfo[0];
            monsterList = response.MonsterList;
        }
        else
        {   // 通信失敗時はnull
            response = null;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(response);
    }

    /// <summary>
    /// モンスターの育成状況情報取得
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator GetNurturedInfo(Action<List<MonsterListResponse>> result)
    {
        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "monsters");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        // 受信情報格納用
        List<MonsterListResponse> response = new List<MonsterListResponse>();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // 通信が成功した時

            string resultJson = request.downloadHandler.text;   // レスポンスボディ(json)の文字列を取得
            response = JsonConvert.DeserializeObject<List<MonsterListResponse>>(resultJson);  // JSONデシリアライズ

            monsterList = response;
        }
        else
        {   // 通信失敗時はnull
            response = null;
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
        NameRequest repuestData = new NameRequest();
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

    /// <summary>
    /// モンスターの初回登録
    /// </summary>
    /// <param name="name">ユーザー名</param>
    /// <param name="result">通信完了辞に呼び出す関数</param>
    /// <returns></returns>
    public IEnumerator InitMonsterStore(string name, Action<NurturingInfoResponse> result)
    {
        // サーバーに送信するオブジェクトを作成
        NameRequest repuestData = new NameRequest();
        repuestData.Name = name;    // 名前を代入

        // サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(repuestData);

        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/init-store", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        // 受信情報格納用
        NurturingInfoResponse response = new NurturingInfoResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // 通信が成功した場合、帰ってきたJSONをオブジェクトに変換
            string resultJson = request.downloadHandler.text;   // レスポンスボディ(json)の文字列を取得
            response = JsonConvert.DeserializeObject<NurturingInfoResponse>(resultJson);  // JSONデシリアライズ

            nurtureInfo = response;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(response);
    }

    /// <summary>
    /// ユーザー名変更処理
    /// </summary>
    /// <param name="name">ユーザー名</param>
    /// <param name="result">通信完了辞に呼び出す関数</param>
    /// <returns></returns>
    public IEnumerator ChangeName(string name, Action<bool> result)
    {
        // サーバーに送信するオブジェクトを作成
        NameRequest repuestData = new NameRequest();
        repuestData.Name = name;    // 名前を代入

        // サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(repuestData);

        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        bool isSuccess = false; // 受信結果

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // 通信成功
            isSuccess = true;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(isSuccess);
    }

    /// <summary>
    /// 育成状態変更処理
    /// </summary>
    /// <param name="name">ユーザー名</param>
    /// <param name="result">通信完了辞に呼び出す関数</param>
    /// <returns></returns>
    public IEnumerator ChangeState(int state, Action<bool> result)
    {
        // サーバーに送信するオブジェクトを作成
        ChangeStateRequest repuestData = new ChangeStateRequest();
        repuestData.ID = nurtureInfo.ID;
        repuestData.State = state;

        // サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(repuestData);

        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        bool isSuccess = false; // 受信結果

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // 通信成功
            isSuccess = true;
            nurtureInfo.State = 2;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(isSuccess);
    }

    /// <summary>
    /// 育成モンスター変更処理
    /// </summary>
    /// <param name="name">ユーザー名</param>
    /// <param name="result">通信完了辞に呼び出す関数</param>
    /// <returns></returns>
    public IEnumerator EvolutionMonster(int id,string name, Action<bool> result)
    {
        // サーバーに送信するオブジェクトを作成
        EvolutionMonsterRequest repuestData = new EvolutionMonsterRequest();
        repuestData.ID = nurtureInfo.ID;
        repuestData.MonsterID = id;
        repuestData.Name = name;

        // サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(repuestData);

        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        bool isSuccess = false; // 受信結果

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // 通信成功
            isSuccess = true;
            nurtureInfo.MonsterID = id;
            nurtureInfo.Name = name;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(isSuccess);
    }

    /// <summary>
    /// 食事処理
    /// </summary>
    /// <param name="usedVol">食事後の残量</param>
    /// <param name="getExp"> 獲得経験値</param>
    /// <param name="result"> レベル・経験値</param>
    /// <returns></returns>
    public IEnumerator ExeMeal(int stomachVol,int usedVol,int getExp, Action<ActionResponse> result) 
    {
        // サーバーに送信するオブジェクトを作成
        MealRequest requestData = new MealRequest();
        requestData.NurtureID = nurtureInfo.ID;
        requestData.StomachVol = stomachVol;
        requestData.UsedVol = usedVol;
        requestData.Exp = getExp;

        // サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(requestData);

        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/meal", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        // 受信情報格納用
        ActionResponse response = new ActionResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // 通信が成功した場合、帰ってきたJSONをオブジェクトに変換
            string resultJson = request.downloadHandler.text;   // レスポンスボディ(json)の文字列を取得
            response = JsonConvert.DeserializeObject<ActionResponse>(resultJson);  // JSONデシリアライズ
            nurtureInfo.StomachVol = stomachVol;
            userInfo.FoodVol = usedVol;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(response);
    }

    /// <summary>
    /// 運動処理
    /// </summary>
    /// <param name="usedVol">運動後の満腹値</param>
    /// <param name="getExp"> 獲得経験値</param>
    /// <param name="result"> レベル・経験値</param>
    /// <returns></returns>
    public IEnumerator ExeExercise(int usedVol, int getExp, Action<ActionResponse> result)
    {
        // サーバーに送信するオブジェクトを作成
        ActionRequest requestData = new ActionRequest();
        requestData.NurtureID = nurtureInfo.ID;
        requestData.UsedVol = usedVol;
        requestData.Exp = getExp;

        // サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(requestData);

        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/exercise", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        // 受信情報格納用
        ActionResponse response = new ActionResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // 通信が成功した場合、帰ってきたJSONをオブジェクトに変換
            string resultJson = request.downloadHandler.text;   // レスポンスボディ(json)の文字列を取得
            response = JsonConvert.DeserializeObject<ActionResponse>(resultJson);  // JSONデシリアライズ
            nurtureInfo.StomachVol = usedVol;
        }
        else
        {
            response = null;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(response);
    }

    /// <summary>
    /// 食料数変更処理
    /// </summary>
    /// <param name="foodVol"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator SupplyFoods(int foodVol,Action<bool> result)
    {
        // サーバーに送信するオブジェクトを作成
        SupplyRequest repuestData = new SupplyRequest();
        repuestData.FoodVol = foodVol;

        // サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(repuestData);

        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        bool isSuccess = false; // 受信結果

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // 通信成功
            userInfo.FoodVol = foodVol;
            isSuccess = true;
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(isSuccess);
    }

    /// <summary>
    /// ミラクル配合
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator MixMiracle(Action<bool> result)
    {
        // サーバーに送信するオブジェクトを作成
        MixMiracleRequest repuestData = new MixMiracleRequest();
        repuestData.NurtureID = nurtureInfo.ID;

        // サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(repuestData);

        // リクエスト送信処理
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/mix/miracle", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // 結果を受信するまで待機

        bool isSuccess = false;
        NurturingInfoResponse response = new NurturingInfoResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // 通信成功
            string resultJson = request.downloadHandler.text;   // レスポンスボディ(json)の文字列を取得
            response = JsonConvert.DeserializeObject<NurturingInfoResponse>(resultJson);  // JSONデシリアライズ
            isSuccess = true;

            nurtureInfo = response; // 世代交代
        }

        // 呼び出し元のresult処理を呼び出す
        result?.Invoke(isSuccess);
    }
}
