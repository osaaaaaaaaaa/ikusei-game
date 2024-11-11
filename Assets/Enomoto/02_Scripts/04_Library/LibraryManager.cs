using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
    #region �A�C�R���{�^���֌W
    [SerializeField] GameObject menu;
    [SerializeField] Transform btnIconListParent;
    [SerializeField] GameObject btnIconPrefab;
    #endregion

    #region �����X�^�[�̏ڍ׉��
    [SerializeField] GameObject details;
    [SerializeField] Image imgMonster;
    [SerializeField] Text nameText;
    [SerializeField] Text textDescription;
    #endregion

    [SerializeField] List<Sprite> spriteMonsters;
    int[] nurturedID;
    NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.Instance;
        GenerateIconButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �����X�^�[�̃A�C�R���{�^���𐶐�����
    /// </summary>
    void GenerateIconButtons()
    {
        // �琬���E�������������X�^�[ID���擾
        StartCoroutine(NetworkManager.Instance.GetNurturedInfo(
            result =>
            {
                nurturedID = result;

                for (int i = 0; i < networkManager.monsterList.Count; i++)
                {
                    int id = i;
                    bool isCheck = false;

                    for(int j = 0; j < nurturedID.Length; j++)
                    {
                        if(i+1 == nurturedID[j]) { isCheck = true; }
                    }

                    // �{�^���̐����ƃZ�b�g�A�b�v
                    if (isCheck)
                    {   // �琬���E�������������X�^�[�̏ꍇ
                        var btn = Instantiate(btnIconPrefab, btnIconListParent);
                        btn.GetComponent<Image>().sprite = spriteMonsters[i + 1];
                        btn.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            Debug.Log(id);
                            SetupMonsterDetailUI(id + 1, NetworkManager.Instance.monsterList[id].Name, NetworkManager.Instance.monsterList[id].Text);
                        });
                    }
                    else
                    {   // ���琬�̏ꍇ
                        var btn = Instantiate(btnIconPrefab, btnIconListParent);
                        btn.GetComponent<Image>().sprite = spriteMonsters[0];
                        btn.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            Debug.Log(id);
                            SetupMonsterDetailUI(0, "???", "?????");
                        });
                    }
                }
            }));
    }

    /// <summary>
    /// �����X�^�[�̏ڍ׉�ʂ̐ݒ�
    /// </summary>
    void SetupMonsterDetailUI(int monsterID,string name,string description)
    {
        menu.SetActive(false);
        details.SetActive(true);
        imgMonster.sprite = spriteMonsters[monsterID];
        nameText.text = name;
        textDescription.text = description;
    }

    public void ShowMenuUI()
    {
        menu.SetActive(true);
        details.SetActive(false);
    }

    public void OnBackButton()
    {
        SEManager.Instance.Play(SEPath.BTN_MENU);
        Initiate.Fade("01_TopScene", Color.black, 1.0f);
    }
}
