using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    #region アイコンボタン関係
    [SerializeField] GameObject menu;
    [SerializeField] Transform btnIconListParent;
    [SerializeField] GameObject btnIconPrefab;
    #endregion

    #region 詳細画面関係
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
    /// モンスターのアイコンボタンを生成する
    /// </summary>
    void GenerateIconButtons()
    {
        for (int i = 0; i < spriteItems.Count; i++)
        {
            int id = new int();
            id = i;

            // ボタンの生成とセットアップ
            var btn = Instantiate(btnIconPrefab, btnIconListParent);
            btn.GetComponent<Image>().sprite = spriteItems[i];
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                SetupDetailUI(id, "アイテムの説明だよーん");
            });
        }
    }

    /// <summary>
    /// アイテムの詳細画面の設定
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
