using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    private NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
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

                }));
        }
        else
        {
            StartCoroutine(NetworkManager.Instance.GetPlayData(
                result =>
                {
                    Debug.Log("プレイデータ取得");
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log(networkManager.nurtureInfo.Name);
            Debug.Log(networkManager.nurtureInfo.Level);
            Debug.Log(networkManager.nurtureInfo.StomachVol);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(NetworkManager.Instance.ExeExercise(
                80,
                600,
                result =>
                {
                    Debug.Log(result.Level);
                    Debug.Log(result.Exp);
                }));
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(NetworkManager.Instance.ExeMeal(
                500,
                1500,
                result =>
                {
                    Debug.Log(result.Level);
                    Debug.Log(result.Exp);
                }));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(NetworkManager.Instance.GetMonsterInfo(
                result =>
                {
                    Debug.Log(NetworkManager.Instance.monsterList[0].Name);
                }));
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(NetworkManager.Instance.MixMiracle(
                result =>
                {
                    if(result)
                    {
                        Debug.Log(NetworkManager.Instance.nurtureInfo.MonsterID);
                    }
                }));
        }
    }
}
