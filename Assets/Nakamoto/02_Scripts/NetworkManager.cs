//---------------------------------------------------------------
//
// �l�b�g���[�N�}�l�[�W���[ [ NetWorkManager.cs ]
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
    // �t�B�[���h

    /// <summary>
    /// API�x�[�XURL
    /// </summary>
    const string API_BASE_URL = "https://ikusei.japaneast.cloudapp.azure.com/api/";

    /// <summary>
    /// get�v���p�e�B���Ăяo�������񎞂ɃC���X�^���X��������static�ŕێ�
    /// </summary>
    private static NetworkManager instance;

    //----------------------------------------------------------------------------------
    // ���[�U�[���

    /// <summary>
    /// �v���C���̃��[�U�[��
    /// </summary>
    private string userName = "";

    /// <summary>
    /// API�F�؃g�[�N��
    /// </summary>
    private string authToken = "";

    /// <summary>
    /// �����X�^�[�}�X�^�[�f�[�^
    /// </summary>
    public List<MonsterListResponse> monsterList { get; private set; } = new List<MonsterListResponse>();

    /// <summary>
    /// ���[�U�[���
    /// </summary>
    public UserInfoResponse userInfo { get; set; } = new UserInfoResponse();

    /// <summary>
    /// �琬�����X�^�[���
    /// </summary>
    public NurturingInfoResponse nurtureInfo { get; set; } = new NurturingInfoResponse();

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
    public IEnumerator GetUserInfo(Action<UserInfoResponse> result)
    {
        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "users/show");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        // ��M���i�[�p
        UserInfoResponse response = new UserInfoResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // �ʐM������������

            string resultJson = request.downloadHandler.text;   // ���X�|���X�{�f�B(json)�̕�������擾
            response = JsonConvert.DeserializeObject<UserInfoResponse>(resultJson);  // JSON�f�V���A���C�Y

            userInfo = response;
        }
        else
        {   // �ʐM���s����null
            response = null;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(response);
    }

    /// <summary>
    /// �琬���擾����
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator GetNurturing(Action<List<NurturingInfoResponse>> result)
    {
        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "monsters/nurturing");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        // ��M���i�[�p
        List<NurturingInfoResponse> response = new List<NurturingInfoResponse>();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // �ʐM������������

            string resultJson = request.downloadHandler.text;   // ���X�|���X�{�f�B(json)�̕�������擾
            response = JsonConvert.DeserializeObject<List<NurturingInfoResponse>>(resultJson);  // JSON�f�V���A���C�Y

            nurtureInfo = response[0];  // �擾����ۑ�
        }
        else
        {   // �ʐM���s����null
            response = null;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(response);
    }

    /// <summary>
    /// �����X�^�[�̃}�X�^�[���擾
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator GetMonsterInfo(Action<List<MonsterListResponse>> result)
    {
        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "monsters");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        // ��M���i�[�p
        List<MonsterListResponse> response = new List<MonsterListResponse>();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // �ʐM������������

            string resultJson = request.downloadHandler.text;   // ���X�|���X�{�f�B(json)�̕�������擾
            response = JsonConvert.DeserializeObject<List<MonsterListResponse>>(resultJson);  // JSON�f�V���A���C�Y

            monsterList = response;
        }
        else
        {   // �ʐM���s����null
            response = null;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(response);
    }

    /// �v���C�f�[�^�擾
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator GetPlayData(Action<PlayDataResponse> result)
    {
        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "users/play-data");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        // ��M���i�[�p
        PlayDataResponse response = new PlayDataResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // �ʐM������������

            string resultJson = request.downloadHandler.text;   // ���X�|���X�{�f�B(json)�̕�������擾
            response = JsonConvert.DeserializeObject<PlayDataResponse>(resultJson);  // JSON�f�V���A���C�Y

            userInfo = response.UserInfo;
            nurtureInfo = response.NurtureInfo[0];
            monsterList = response.MonsterList;
        }
        else
        {   // �ʐM���s����null
            response = null;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(response);
    }

    /// <summary>
    /// �����X�^�[�̈琬�󋵏��擾
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator GetNurturedInfo(Action<List<MonsterListResponse>> result)
    {
        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "monsters");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        // ��M���i�[�p
        List<MonsterListResponse> response = new List<MonsterListResponse>();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {   // �ʐM������������

            string resultJson = request.downloadHandler.text;   // ���X�|���X�{�f�B(json)�̕�������擾
            response = JsonConvert.DeserializeObject<List<MonsterListResponse>>(resultJson);  // JSON�f�V���A���C�Y

            monsterList = response;
        }
        else
        {   // �ʐM���s����null
            response = null;
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
        NameRequest repuestData = new NameRequest();
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

    /// <summary>
    /// �����X�^�[�̏���o�^
    /// </summary>
    /// <param name="name">���[�U�[��</param>
    /// <param name="result">�ʐM�������ɌĂяo���֐�</param>
    /// <returns></returns>
    public IEnumerator InitMonsterStore(string name, Action<NurturingInfoResponse> result)
    {
        // �T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        NameRequest repuestData = new NameRequest();
        repuestData.Name = name;    // ���O����

        // �T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(repuestData);

        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/init-store", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        // ��M���i�[�p
        NurturingInfoResponse response = new NurturingInfoResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // �ʐM�����������ꍇ�A�A���Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultJson = request.downloadHandler.text;   // ���X�|���X�{�f�B(json)�̕�������擾
            response = JsonConvert.DeserializeObject<NurturingInfoResponse>(resultJson);  // JSON�f�V���A���C�Y

            nurtureInfo = response;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(response);
    }

    /// <summary>
    /// ���[�U�[���ύX����
    /// </summary>
    /// <param name="name">���[�U�[��</param>
    /// <param name="result">�ʐM�������ɌĂяo���֐�</param>
    /// <returns></returns>
    public IEnumerator ChangeName(string name, Action<bool> result)
    {
        // �T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        NameRequest repuestData = new NameRequest();
        repuestData.Name = name;    // ���O����

        // �T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(repuestData);

        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        bool isSuccess = false; // ��M����

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // �ʐM����
            isSuccess = true;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(isSuccess);
    }

    /// <summary>
    /// �琬��ԕύX����
    /// </summary>
    /// <param name="name">���[�U�[��</param>
    /// <param name="result">�ʐM�������ɌĂяo���֐�</param>
    /// <returns></returns>
    public IEnumerator ChangeState(int state, Action<bool> result)
    {
        // �T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        ChangeStateRequest repuestData = new ChangeStateRequest();
        repuestData.ID = nurtureInfo.ID;
        repuestData.State = state;

        // �T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(repuestData);

        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        bool isSuccess = false; // ��M����

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // �ʐM����
            isSuccess = true;
            nurtureInfo.State = 2;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(isSuccess);
    }

    /// <summary>
    /// �琬�����X�^�[�ύX����
    /// </summary>
    /// <param name="name">���[�U�[��</param>
    /// <param name="result">�ʐM�������ɌĂяo���֐�</param>
    /// <returns></returns>
    public IEnumerator EvolutionMonster(int id,string name, Action<bool> result)
    {
        // �T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        EvolutionMonsterRequest repuestData = new EvolutionMonsterRequest();
        repuestData.ID = nurtureInfo.ID;
        repuestData.MonsterID = id;
        repuestData.Name = name;

        // �T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(repuestData);

        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        bool isSuccess = false; // ��M����

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // �ʐM����
            isSuccess = true;
            nurtureInfo.MonsterID = id;
            nurtureInfo.Name = name;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(isSuccess);
    }

    /// <summary>
    /// �H������
    /// </summary>
    /// <param name="usedVol">�H����̎c��</param>
    /// <param name="getExp"> �l���o���l</param>
    /// <param name="result"> ���x���E�o���l</param>
    /// <returns></returns>
    public IEnumerator ExeMeal(int stomachVol,int usedVol,int getExp, Action<ActionResponse> result) 
    {
        // �T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        MealRequest requestData = new MealRequest();
        requestData.NurtureID = nurtureInfo.ID;
        requestData.StomachVol = stomachVol;
        requestData.UsedVol = usedVol;
        requestData.Exp = getExp;

        // �T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(requestData);

        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/meal", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        // ��M���i�[�p
        ActionResponse response = new ActionResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // �ʐM�����������ꍇ�A�A���Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultJson = request.downloadHandler.text;   // ���X�|���X�{�f�B(json)�̕�������擾
            response = JsonConvert.DeserializeObject<ActionResponse>(resultJson);  // JSON�f�V���A���C�Y
            nurtureInfo.StomachVol = stomachVol;
            userInfo.FoodVol = usedVol;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(response);
    }

    /// <summary>
    /// �^������
    /// </summary>
    /// <param name="usedVol">�^����̖����l</param>
    /// <param name="getExp"> �l���o���l</param>
    /// <param name="result"> ���x���E�o���l</param>
    /// <returns></returns>
    public IEnumerator ExeExercise(int usedVol, int getExp, Action<ActionResponse> result)
    {
        // �T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        ActionRequest requestData = new ActionRequest();
        requestData.NurtureID = nurtureInfo.ID;
        requestData.UsedVol = usedVol;
        requestData.Exp = getExp;

        // �T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(requestData);

        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/exercise", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        // ��M���i�[�p
        ActionResponse response = new ActionResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // �ʐM�����������ꍇ�A�A���Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultJson = request.downloadHandler.text;   // ���X�|���X�{�f�B(json)�̕�������擾
            response = JsonConvert.DeserializeObject<ActionResponse>(resultJson);  // JSON�f�V���A���C�Y
            nurtureInfo.StomachVol = usedVol;
        }
        else
        {
            response = null;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(response);
    }

    /// <summary>
    /// �H�����ύX����
    /// </summary>
    /// <param name="foodVol"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator SupplyFoods(int foodVol,Action<bool> result)
    {
        // �T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        SupplyRequest repuestData = new SupplyRequest();
        repuestData.FoodVol = foodVol;

        // �T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(repuestData);

        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        bool isSuccess = false; // ��M����

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // �ʐM����
            userInfo.FoodVol = foodVol;
            isSuccess = true;
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(isSuccess);
    }

    /// <summary>
    /// �~���N���z��
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator MixMiracle(Action<bool> result)
    {
        // �T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        MixMiracleRequest repuestData = new MixMiracleRequest();
        repuestData.NurtureID = nurtureInfo.ID;

        // �T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(repuestData);

        // ���N�G�X�g���M����
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "monsters/mix/miracle", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();  // ���ʂ���M����܂őҋ@

        bool isSuccess = false;
        NurturingInfoResponse response = new NurturingInfoResponse();

        if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
        {
            // �ʐM����
            string resultJson = request.downloadHandler.text;   // ���X�|���X�{�f�B(json)�̕�������擾
            response = JsonConvert.DeserializeObject<NurturingInfoResponse>(resultJson);  // JSON�f�V���A���C�Y
            isSuccess = true;

            nurtureInfo = response; // ������
        }

        // �Ăяo������result�������Ăяo��
        result?.Invoke(isSuccess);
    }
}
