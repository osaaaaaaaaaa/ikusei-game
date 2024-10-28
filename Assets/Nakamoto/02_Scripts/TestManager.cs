using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ���[�U�[�f�[�^�̓Ǎ������E���ʂ��擾
        bool isSuccess = NetworkManager.Instance.LoadUserData();

        if (!isSuccess)
        {
            // ���[�U�[�f�[�^���ۑ�����Ă��Ȃ��ꍇ�͓o�^
            StartCoroutine(NetworkManager.Instance.StoreUser(
                Guid.NewGuid().ToString(),  // ���[�U�[��
                result =>
                {
                    if(result)
                    {
                        Debug.Log("���[�U�[�o�^����");
                        // ���[�U�[���̎擾
                        StartCoroutine(NetworkManager.Instance.GetUserInfo(
                            result =>
                            {
                                Debug.Log("���[�U�[���擾");
                            }));
                    }
                    else
                    {
                        Debug.Log("���[�U�[�o�^�����s");
                    }

                }));
        }
        else
        {
            // ���[�U�[���̎擾
            StartCoroutine(NetworkManager.Instance.GetUserInfo(
                result =>
                {
                    Debug.Log("���[�U�[���擾");
                }));

            StartCoroutine(NetworkManager.Instance.GetNurturing(
                result =>
                {
                    Debug.Log("�琬���擾");
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
    }
}
