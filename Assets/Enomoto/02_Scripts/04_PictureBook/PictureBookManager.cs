using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureBookManager : MonoBehaviour
{
    #region アイコンボタン関係
    [SerializeField] GameObject menu;
    [SerializeField] Transform btnIconListParent;
    [SerializeField] GameObject btnIconPrefab;
    #endregion

    #region モンスターの詳細画面
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
    /// モンスターのアイコンボタンを生成する
    /// </summary>
    void GenerateIconButtons()
    {
        for(int i = 0; i< spriteMonsters.Count; i++)
        {
            int id = new int();
            id = i;

            // ボタンの生成とセットアップ
            var btn = Instantiate(btnIconPrefab, btnIconListParent);
            btn.GetComponent<Image>().sprite = spriteMonsters[i];
            btn.GetComponent<Button>().onClick.AddListener(() => {
                SetupMonsterDetailUI(id, "モンスターの説明だよーん");
            });
        }
    }

    /// <summary>
    /// モンスターの詳細画面の設定
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
