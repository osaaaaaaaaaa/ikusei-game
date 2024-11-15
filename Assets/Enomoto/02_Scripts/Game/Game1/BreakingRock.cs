using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using KanKikuchi.AudioManager;

public class BreakingRock : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject effectExplosion;
    [SerializeField] GameObject effectGroundHit;

    [SerializeField] MiniGameManager1 manager;
    [SerializeField] Image imageCrack;
    [SerializeField] Sprite spriteRock;
    float maxCrackValue;
    public bool isBreaking { get; private set; }
    bool isCollision;
    const float breakingPower = 2.5f;

    private void Start()
    {
        maxCrackValue = 0;
        isCollision = false;
        isBreaking = false;
    }

    private void Update()
    {
        if (maxCrackValue == 0) return;
        if(imageCrack.fillAmount < maxCrackValue)
        {
            // 岩に入るひびを描写する
            imageCrack.fillAmount += 0.5f * Time.deltaTime;

            // 最後までひびが描写された場合
            if(imageCrack.fillAmount >= maxCrackValue)
            {
                if (isBreaking)
                {
                    Invoke("PlayBreakingAnim", 1f);
                }
                else
                {
                    Invoke("PlayGameoverSE", 1f);
                    Invoke("CallManagerMethod", 1f);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(manager.state == MiniGameManager1.MINIGAME1_STATE.BreakAnim 
           && collision.gameObject.tag == "Monster"
           && !isCollision)
        {
            effectGroundHit.SetActive(true);

            // 岩に入るひびの値を取得
            maxCrackValue = manager.totalPower / MiniGameManager1.totalPowerMax;
            Debug.Log(manager.totalPower);

            if (manager.totalPower > breakingPower / 2)
            {
                SEManager.Instance.Play(SEPath.ROCK_BREAK);
                // 顔差分に入れ替える
                GetComponent<SpriteRenderer>().sprite = spriteRock;
            }
            if (manager.totalPower > breakingPower)
            {
                // 壊れるようにする
                isBreaking = true;
                maxCrackValue = 1;
            }

            // カメラを揺らす
            mainCamera.transform.DOShakePosition(0.5f, manager.totalPower / 2, 15, 2f, false, true);

            isCollision = true;
        }
    }

    void PlayBreakingAnim()
    {
        SEManager.Instance.Play(SEPath.BREAK);

        effectExplosion.SetActive(true);
        imageCrack.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        Invoke("CallManagerMethod", 5f);
    }

    void CallManagerMethod()
    {
        manager.UpdateGameState();
    }

    void PlayGameoverSE()
    {
        SEManager.Instance.Play(SEPath.FAILURE);
    }
}
