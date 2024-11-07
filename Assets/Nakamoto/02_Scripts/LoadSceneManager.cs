//---------------------------------------------------------------
//
// ロードシーンマネージャー [ LoadManager.cs ]
// Author:Kenta Nakamoto
// Data:2024/10/28
// Update:2024/10/28
//
//---------------------------------------------------------------
using KanKikuchi.AudioManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    private NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void TransTopScene()
    {
        Initiate.Fade("01_TopScene", Color.white, 1.0f);
    }

    public void PushStart()
    {
        SEManager.Instance.Play(SEPath.BTN_SELECT);

        startButton.interactable = false;

        networkManager = NetworkManager.Instance;

        // ユーザーデータの読込処理・結果を取得
        bool isSuccess = NetworkManager.Instance.LoadUserData();

        if (!isSuccess)
        {
            // ユーザーデータが保存されていない場合は登録
            StartCoroutine(NetworkManager.Instance.StoreUser(
                Guid.NewGuid().ToString(),  // ユーザー名
                result =>
                {
                    if (result)
                    {
                        Debug.Log("ユーザー登録完了");
                        // ユーザー情報の取得
                        StartCoroutine(NetworkManager.Instance.GetUserInfo(
                            result =>
                            {
                                Debug.Log("ユーザー情報取得");
                                // モンスター初回登録
                                StartCoroutine(NetworkManager.Instance.InitMonsterStore(
                                    "おためし",
                                    result =>
                                    {
                                        Debug.Log("育成情報取得");
                                        StartCoroutine(NetworkManager.Instance.GetMonsterInfo(
                                            result =>
                                            {
                                                Debug.Log("モンスターリスト取得");
                                                TransTopScene();
                                            }));
                                    }));
                            }));
                    }
                    else
                    {
                        Debug.Log("ユーザー登録失敗");
                    }
                }));
        }
        else
        {
            // ユーザー情報の取得
            StartCoroutine(NetworkManager.Instance.GetPlayData(
                result =>
                {
                    if (result == null)
                    {
                        Debug.Log("プレイデータ取得失敗");
                    }
                    else
                    {
                        Debug.Log("プレイデータ取得");
                        TransTopScene();
                    }
                }));
        }
    }
}
