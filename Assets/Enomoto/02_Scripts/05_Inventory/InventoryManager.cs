using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    #region �A�C�R���{�^���֌W
    [SerializeField] GameObject menu;
    [SerializeField] Transform btnIconListParent;
    [SerializeField] GameObject btnIconPrefab;
    #endregion

    #region �ڍ׉�ʊ֌W
    [SerializeField] GameObject panelDetails;
    [SerializeField] Image imgItem;
    [SerializeField] Text textDescription;
    #endregion

    [SerializeField] List<Sprite> spriteItems;

    // Start is called before the first frame update
    void Start()
    {
        GenerateIconButtons();
    }

    /// <summary>
    /// �����X�^�[�̃A�C�R���{�^���𐶐�����
    /// </summary>
    void GenerateIconButtons()
    {
        for (int i = 0; i < spriteItems.Count; i++)
        {
            int id = new int();
            id = i;

            // �{�^���̐����ƃZ�b�g�A�b�v
            var btn = Instantiate(btnIconPrefab, btnIconListParent);
            btn.GetComponent<Image>().sprite = spriteItems[i];
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                SetupDetailUI(id, "�A�C�e���̐�������[��");
            });
        }
    }

    /// <summary>
    /// �A�C�e���̏ڍ׉�ʂ̐ݒ�
    /// </summary>
    void SetupDetailUI(int itemID, string description)
    {
        menu.SetActive(false);
        panelDetails.SetActive(true);
        imgItem.sprite = spriteItems[itemID];
        textDescription.text = description;
    }

    public void ShowMenuUI()
    {
        menu.SetActive(true);
        panelDetails.SetActive(false);
    }

    public void OnBackButton()
    {
        Initiate.Fade("01_TopScene", Color.black, 1.0f);
    }
}
