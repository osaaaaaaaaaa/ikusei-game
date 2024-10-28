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

        // ���[�U�[�f�[�^�̓Ǎ������E���ʂ��擾
        bool isSuccess = NetworkManager.Instance.LoadUserData();

        if (!isSuccess)
        {
            // ���[�U�[�f�[�^���ۑ�����Ă��Ȃ��ꍇ�͓o�^
            StartCoroutine(NetworkManager.Instance.StoreUser(
                Guid.NewGuid().ToString(),  // ���[�U�[��
                result =>
                {

                }));
        }
        else
        {
            StartCoroutine(NetworkManager.Instance.GetPlayData(
                result =>
                {
                    Debug.Log("�v���C�f�[�^�擾");
                }));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            // ���[�U�[���̎擾
            StartCoroutine(NetworkManager.Instance.ChangeName(
                "name",
                result =>
                {
                    if(result)
                    {
                        Debug.Log("���O�ύX����");
                    }
                }));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���[�U�[���̎擾
            StartCoroutine(NetworkManager.Instance.InitMonsterStore(
                "hoge",
                result =>
                {
                        Debug.Log("���񃂃��X�^�[����");
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
