using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixSceneManager : MonoBehaviour
{
    #region UIä÷åW
    [SerializeField] GameObject btnMix;
    [SerializeField] GameObject btnBack;
    [SerializeField] GameObject btnTips;
    #endregion

    [SerializeField] Transform monsterPoint;
    [SerializeField] GameObject eggPrefab;
    GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        // ÉÇÉìÉXÉ^Å[ê∂ê¨èàóù
        monster = MonsterController.Instance.GenerateMonster(NetworkManager.Instance.nurtureInfo.MonsterID, monsterPoint);
        monster.GetComponent<Rigidbody2D>().gravityScale = 0;
        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Idle);
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
        btnMix.SetActive(false);
        btnBack.SetActive(false);
        btnTips.SetActive(false);
        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Mix);
        StartCoroutine(GenarateEgg());
    }

    public void OnBackButton()
    {
        Initiate.Fade("01_TopScene", Color.black, 1.0f);
    }
}
