using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixSceneManager : MonoBehaviour
{
    [SerializeField] Transform monsterPoint;
    [SerializeField] GameObject eggPrefab;
    GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        // モンスター生成処理
        monster = MonsterController.Instance.GenerateMonster(NetworkManager.Instance.nurtureInfo.MonsterID, monsterPoint);
        monster.GetComponent<Rigidbody2D>().gravityScale = 0;
        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Idle);
    }

    void GenarateEgg()
    {
        Instantiate(eggPrefab, monster.transform.position, Quaternion.identity);
        Destroy(monster);
    }

    public void OnMixButton()
    {
        MonsterController.Instance.PlayMonsterAnim(MonsterController.ANIM_ID.Mix);
        Invoke("GenarateEgg", 2f);
    }

    public void OnBackButton()
    {
        Initiate.Fade("01_TopScene", Color.black, 1.0f);
    }
}
