using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MiniGameManager2 : MonoBehaviour
{
    #region マネージャー関係
    [SerializeField] JumpController jumpController;
    [SerializeField] CountDown countDown;
    [SerializeField] InvincibleController invincibleController;
    #endregion

    [SerializeField] Transform stageParent;
    [SerializeField] List<GameObject> obstractPrefabs;
    [SerializeField] List<GameObject> monsterPrefabs;
    [SerializeField] GameObject resultUI;
    GameObject monster;
    int monsterHitCnt;
    float addSpeed;
    float currentTimeObstracle;
    float triggerTimeObstracle;
    public bool isGameEnd { get; private set; }
    bool isInvincible;
    public bool IsInvincible { get { return isInvincible; }set { isInvincible = value; } }

    void Start()
    {
        monsterHitCnt = 0;
        addSpeed = 0;
        currentTimeObstracle = 0;
        triggerTimeObstracle = 3;
        isGameEnd = false;

        GenerateMonster();
    }

    private void Update()
    {
        if(!countDown.isAnimEnd)
        {
            return;
        }
        if (isGameEnd)
        {
            monster.transform.Rotate(new Vector3(0f, 0f, -300f) * Time.deltaTime);
            return;
        }

        currentTimeObstracle += Time.deltaTime;
        if (currentTimeObstracle >= triggerTimeObstracle)
        {
            // 一定間隔で障害物を生成する
            currentTimeObstracle = 0;
            GenerateObstract();
        }
    }

    void GenerateMonster()
    {
        // モンスターを生成する
        monster = Instantiate(monsterPrefabs[0]);
        jumpController.Init(monster, monster.GetComponent<Rigidbody2D>());
    }

    void GenerateObstract()
    {
        int index = Random.Range(0, obstractPrefabs.Count + 1);
        if (index != 0)
        {
            // 障害物を生成し、徐々にスピードアップさせる
            var obstacle = Instantiate(obstractPrefabs[index - 1], stageParent);
            obstacle.GetComponent<Obstacle>().Init(this,4 + addSpeed);
            addSpeed += 0.1f;
            if (triggerTimeObstracle > 1f) triggerTimeObstracle -= addSpeed;
            if (triggerTimeObstracle <= 1f) triggerTimeObstracle = 1f;
        }
    }

    public void HitMonster()
    {
        if (isGameEnd || isInvincible) return;

        monsterHitCnt++;
        if(monsterHitCnt >= 3)
        {
            isGameEnd = true;
            PlayDeathAnimMonster();
            Invoke("ShowResult", 2f);
        }
        else
        {
            invincibleController.PlayInvincibleAnim(monster.GetComponent<SpriteRenderer>());
        }
    }

    void PlayDeathAnimMonster()
    {
        monster.GetComponent<BoxCollider2D>().enabled = false;
        jumpController.Jump();
    }

    void ShowResult()
    {
        resultUI.SetActive(true);
    }

    public void OnBackButton()
    {
        Initiate.Fade("TopScene", Color.black, 1.0f);
    }
}
