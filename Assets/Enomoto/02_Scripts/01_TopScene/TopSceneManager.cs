using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class TopSceneManager : MonoBehaviour
{
    #region タイトル関係
    [SerializeField] GameObject titleSet;
    #endregion

    #region メニュー画面関係
    [SerializeField] GameObject menuSet;
    #endregion

    #region トップ画面関係
    [SerializeField] GameObject topSet;
    #endregion

    [SerializeField] PolygonCollider2D touchTrigger;
    bool isTouchMonster;

    // Start is called before the first frame update
    void Start()
    {
        isTouchMonster = false;

        // モンスター生成処理
        MonsterController.Instance.GenerateMonster(new Vector2(0f, -1.5f)).GetComponent<Rigidbody2D>().gravityScale = 0;
        // TouchTriggerのコライダーを設定
        touchTrigger.points = MonsterController.Instance.GetCollider().points;
        // モンスターのアニメーション再生開始
        MonsterController.Instance.PlayStartAnim();
    }

    // Update is called once per frame
    void Update()
    {
        if (titleSet.activeSelf && Input.GetMouseButtonDown(0))
        {
            titleSet.SetActive(false);
            ToggleTopVisibility(true);
        }

        // モンスター(TouchTrigger)をタップしたらジャンプさせる
        if (!isTouchMonster && Input.GetMouseButtonUp(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit2d)
            {
                GameObject targetObj = hit2d.collider.gameObject;
                if(targetObj.tag == "Trigger")
                {
                    isTouchMonster = true;
                    MonsterController.Instance.PlayJumpAnim();
                    Invoke("ResetTriggerFrag", 1f);
                }
            }
        }

    }

    void ResetTriggerFrag()
    {
        isTouchMonster = false;
    }

    /// <summary>
    /// メニューUIの表示・非表示
    /// </summary>
    /// <param name="isVisibility"></param>
    public void ToggleMenuVisibility(bool isVisibility)
    {
        menuSet.SetActive(isVisibility);
    }

    /// <summary>
    /// トップ画面の表示・非表示
    /// </summary>
    /// <param name="isVisibility"></param>
    public void ToggleTopVisibility(bool isVisibility)
    {
        topSet.SetActive(isVisibility);
    }

    public void OnTrainingButton()
    {
        int rnd = Random.Range(1, 4);
        rnd = 2;
        switch (rnd)
        {
            case 1:
                Initiate.Fade("GameScene1", Color.black, 1.0f);
                break;
            case 2:
                Initiate.Fade("GameScene2", Color.black, 1.0f);
                break;
            case 3:
                Initiate.Fade("GameScene3", Color.black, 1.0f);
                break;
        }
    }

    public void OnSupplyButton()
    {
        SceneManager.LoadScene("03_SupplyScene");
    }

    public void OnMealButton()
    {
        Initiate.Fade("02_MealScene", Color.white, 1.0f);
    }

    public void OnPictureBookButton()
    {
        SceneManager.LoadScene("04_PictureBookScene");
    }

    public void OnInventoryButton()
    {
        SceneManager.LoadScene("05_Inventory");
    }
}
