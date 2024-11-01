using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixSceneManager : MonoBehaviour
{
    [SerializeField] GameObject eggPrefab;
    GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        // ÉÇÉìÉXÉ^Å[ê∂ê¨èàóù
        monster = MonsterController.Instance.GenerateMonster(NetworkManager.Instance.nurtureInfo.MonsterID, new Vector2(0f, -2f));
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
