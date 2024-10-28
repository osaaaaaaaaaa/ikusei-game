using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ユーザーデータの読込処理・結果を取得
        bool isSuccess = NetworkManager.Instance.LoadUserData();

        if (!isSuccess)
        {
            // ユーザーデータが保存されていない場合は登録
            StartCoroutine(NetworkManager.Instance.StoreUser(
                Guid.NewGuid().ToString(),  // ユーザー名
                result =>
                {
                    if(result)
                    {
                        Debug.Log("ユーザー登録完了");
                        // ユーザー情報の取得
                        StartCoroutine(NetworkManager.Instance.GetUserInfo(
                            result =>
                            {
                                Debug.Log("ユーザー情報取得");
                            }));
                    }
                    else
                    {
                        Debug.Log("ユーザー登録完失敗");
                    }

                }));
        }
        else
        {
            // ユーザー情報の取得
            StartCoroutine(NetworkManager.Instance.GetUserInfo(
                result =>
                {
                    Debug.Log("ユーザー情報取得");
                }));

            StartCoroutine(NetworkManager.Instance.GetNurturing(
                result =>
                {
                    Debug.Log("育成情報取得");
                }));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            // ユーザー情報の取得
            StartCoroutine(NetworkManager.Instance.ChangeName(
                "name",
                result =>
                {
                    if(result)
                    {
                        Debug.Log("名前変更完了");
                    }
                }));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ユーザー情報の取得
            StartCoroutine(NetworkManager.Instance.InitMonsterStore(
                "hoge",
                result =>
                {
                        Debug.Log("初回モンスター完了");
                }));
        }
    }
}
