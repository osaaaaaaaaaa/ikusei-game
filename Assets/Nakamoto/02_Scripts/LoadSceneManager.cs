//---------------------------------------------------------------
//
// ���[�h�V�[���}�l�[�W���[ [ LoadManager.cs ]
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

        // ���[�U�[�f�[�^�̓Ǎ������E���ʂ��擾
        bool isSuccess = NetworkManager.Instance.LoadUserData();

        if (!isSuccess)
        {
            // ���[�U�[�f�[�^���ۑ�����Ă��Ȃ��ꍇ�͓o�^
            StartCoroutine(NetworkManager.Instance.StoreUser(
                Guid.NewGuid().ToString(),  // ���[�U�[��
                result =>
                {
                    if (result)
                    {
                        Debug.Log("���[�U�[�o�^����");
                        // ���[�U�[���̎擾
                        StartCoroutine(NetworkManager.Instance.GetUserInfo(
                            result =>
                            {
                                Debug.Log("���[�U�[���擾");
                                // �����X�^�[����o�^
                                StartCoroutine(NetworkManager.Instance.InitMonsterStore(
                                    "�����߂�",
                                    result =>
                                    {
                                        Debug.Log("�琬���擾");
                                        StartCoroutine(NetworkManager.Instance.GetMonsterInfo(
                                            result =>
                                            {
                                                Debug.Log("�����X�^�[���X�g�擾");
                                                TransTopScene();
                                            }));
                                    }));
                            }));
                    }
                    else
                    {
                        Debug.Log("���[�U�[�o�^���s");
                    }
                }));
        }
        else
        {
            // ���[�U�[���̎擾
            StartCoroutine(NetworkManager.Instance.GetPlayData(
                result =>
                {
                    if (result == null)
                    {
                        Debug.Log("�v���C�f�[�^�擾���s");
                    }
                    else
                    {
                        Debug.Log("�v���C�f�[�^�擾");
                        TransTopScene();
                    }
                }));
        }
    }
}
