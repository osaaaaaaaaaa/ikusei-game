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
    [SerializeField] Text textDescription;
    #endregion

    [SerializeField] List<Sprite> spriteMonsters;

    // Start is called before the first frame update
    void Start()
    {
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
        for(int i = 0; i< spriteMonsters.Count; i++)
        {
            int id = new int();
            id = i;

            // �{�^���̐����ƃZ�b�g�A�b�v
            var btn = Instantiate(btnIconPrefab, btnIconListParent);
            btn.GetComponent<Image>().sprite = spriteMonsters[i];
            btn.GetComponent<Button>().onClick.AddListener(() => {
                SetupMonsterDetailUI(id, "�����X�^�[�̐�������[��");
            });
        }
    }

    /// <summary>
    /// �����X�^�[�̏ڍ׉�ʂ̐ݒ�
    /// </summary>
    void SetupMonsterDetailUI(int monsterID,string description)
    {
        menu.SetActive(false);
        details.SetActive(true);
        imgMonster.sprite = spriteMonsters[monsterID];
        textDescription.text = description;
    }

    public void ShowMenuUI()
    {
        menu.SetActive(true);
        details.SetActive(false);
    }

    public void OnBackButton()
    {
        Initiate.Fade("01_TopScene", Color.black, 1.0f);
    }
}