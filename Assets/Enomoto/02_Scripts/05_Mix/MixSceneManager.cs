using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixSceneManager : MonoBehaviour
{
    #region UI関係
    [SerializeField] GameObject btnMix;
    [SerializeField] GameObject cautionPanel;
    [SerializeField] GameObject btnBack;
    [SerializeField] GameObject btnTips;
    #endregion

    [SerializeField] Transform monsterPoint;
    [SerializeField] GameObject eggPrefab;
    GameObject monster;
    const int mixLimitNum = 5;

    // Start is called before the first frame update
    void Start()
    {
        BGMManager.Instance.Play(BGMPath.MIX);

        // モンスター生成処理
        monster = MonsterController.Instance.GenerateMonster(NetworkManager.Instance.nurtureInfo.MonsterID, monsterPoint);
        monster.GetComponent<Rigidbody2D>().gravityScale = 0;
        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Idle);

        // 配合可能か判断
        if(NetworkManager.Instance.nurtureInfo.Level >= mixLimitNum)
        {
            btnMix.SetActive(true);
        }
        else
        {
            cautionPanel.SetActive(true);
        }
    }

    IEnumerator GenarateEgg()
    {
        yield return new WaitForSeconds(2f);

        Instantiate(eggPrefab, monster.transform.position, Quaternion.identity);
        Destroy(monster);

        yield return new WaitForSeconds(2f);
        btnBack.SetActive(true);
        btnTips.SetActive(true);
    }

    public void OnMixButton()
    {
        SEManager.Instance.Play(SEPath.BTN_MENU);
        btnMix.SetActive(false);
        btnBack.SetActive(false);
        btnTips.SetActive(false);
        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Mix);
        StartCoroutine(GenarateEgg());
    }

    public void OnBackButton()
    {
        SEManager.Instance.Play(SEPath.BTN_MENU);
        Initiate.Fade("01_TopScene", Color.black, 1.0f);
    }
}
