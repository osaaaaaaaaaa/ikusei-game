//---------------------------------------------------------------
//
// �l�b�g���[�N�}�l�[�W���[ [ NetWorkManager.cs ]
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
    // �t�B�[���h

    /// <summary>
    /// API�x�[�XURL
    /// </summary>
    //const string API_BASE_URL = "https://api-shot.japaneast.cloudapp.azure.com/api/";
    const string API_BASE_URL = "http://localhost:8000/api/";


    /// <summary>
    /// get�v���p�e�B���Ăяo�������񎞂ɃC���X�^���X��������static�ŕێ�
    /// </summary>
    private static NetworkManager instance;

    /// <summary>
    /// �v���C���̃��[�U�[��
    /// </summary>
    private string userName = "";

    /// <summary>
    /// API�F�؃g�[�N��
    /// </summary>
    private string authToken = "";

    /// <summary>
    /// NetworkManager�v���p�e�B
    /// </summary>
    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                // GameObject�𐶐����ANetworkManager��ǉ�
                GameObject gameObject = new GameObject("NetworkManager");
                instance = gameObject.AddComponent<NetworkManager>();

                // �V�[���J�ڂŔj������Ȃ��悤�ɐݒ�
                DontDestroyOnLoad(gameObject);
            }

            return instance;
        }
    }

    //-------------------------------------------------------------------
    // ���\�b�h

    /// <summary>
    /// ���[�U�[�f�[�^�ۑ�����
    /// </summary>
    private void SaveUserData()
    {
        // �Z�[�u�f�[�^�N���X�̐���
        SaveData saveData = new SaveData();
        saveData.UserName = this.userName;
        saveData.AuthToken = this.authToken;

        // �f�[�^��JSON�V���A���C�Y
        string json = JsonConvert.SerializeObject(saveData);

        // �w�肵����΃p�X��"saveData.json"��ۑ�
        var writer = new StreamWriter(Application.persistentDataPath + "/saveData.json");
        writer.Write(json); // �����o��
        writer.Flush();     // �o�b�t�@�Ɏc���Ă���l��S�ď����o��
        writer.Close();     // �t�@�C����
    }

    /// <summary>
    /// ���[�U�[�f�[�^�ǂݍ��ݏ���
    /// </summary>
    /// <returns></returns>
    public bool LoadUserData()
    {
        if (!File.Exists(Application.persistentDataPath + "/saveData.json"))
        {   // �w��̃p�X�̃t�@�C�������݂��Ȃ��������A�������^�[��
            Debug.Log("�Z�[�u�f�[�^����");
            return false;
        }

        //  ���[�J���t�@�C�����烆�[�U�[�f�[�^�̓Ǎ�����
        var reader = new StreamReader(Application.persistentDataPath + "/saveData.json");
        string json = reader.ReadToEnd();
        reader.Close();

        // �Z�[�u�f�[�^JSON���f�V���A���C�Y���Ď擾
        SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
        this.userName = saveData.UserName;
        this.authToken = saveData.AuthToken;

        Debug.Log("�Z�[�u�f�[�^�ǂݍ��݊���");

        // �ǂݍ��݌��ʂ����^�[��
        return true;
    }

    //=============================
    // GET����

    /// <summary>
    /// ���[�U�[���擾����
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator GetUserInfo(Action<List<UserInfoResponse>> result)
    {
        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "users/show");
        request.SetRequestHeader("Authorization", "Bearer" + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        // ��M���i�[�p
        List<UserInfoResponse> response = new List<UserInfoResponse>();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // �ʐM������������

            string resultJson = request.downloadHandler.text;   // ���X�|���X�{�f�B(json)�̕�������擾
            response = JsonConvert.DeserializeObject<List<UserInfoResponse>>(resultJson);  // JSON�f�V���A���C�Y
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(response);
    }

    //=============================
    // POST����

    /// <summary>
    /// ���[�U�[�o�^����
    /// </summary>
    /// <param name="name">���[�U�[��</param>
    /// <param name="result">�ʐM�������ɌĂяo���֐�</param>
    /// <returns></returns>
    public IEnumerator StoreUser(string name, Action<bool> result)
    {
        // �T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        StoreUserRequest repuestData = new StoreUserRequest();
        repuestData.Name = name;    // ���O����

        // �T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(repuestData);

        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/store", json, "application/json");
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        bool isSuccess = false; // ��M����

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // �ʐM�����������ꍇ�A�A���Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultJson = request.downloadHandler.text;   // ���X�|���X�{�f�B(json)�̕�������擾
            StoreUserResponse response = JsonConvert.DeserializeObject<StoreUserResponse>(resultJson);  // JSON�f�V���A���C�Y

            // �t�@�C���Ƀ��[�U�[�f�[�^��ۑ�
            this.userName = name;
            this.authToken = response.Token;
            SaveUserData();
            isSuccess = true;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(isSuccess);
    }
}
