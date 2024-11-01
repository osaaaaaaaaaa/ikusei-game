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
            // ä‚Ç…ì¸ÇÈÇ–Ç—Çï`é Ç∑ÇÈ
            imageCrack.fillAmount += 0.5f * Time.deltaTime;

            // ç≈å„Ç‹Ç≈Ç–Ç—Ç™ï`é Ç≥ÇÍÇΩèÍçá
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

            // ä‚Ç…ì¸ÇÈÇ–Ç—ÇÃílÇéÊìæ
            maxCrackValue = manager.totalPower / MiniGameManager1.totalPowerMax;
            Debug.Log(manager.totalPower);

            if (manager.totalPower > breakingPower / 2)
            {
                // äÁç∑ï™Ç…ì¸ÇÍë÷Ç¶ÇÈ
                GetComponent<SpriteRenderer>().sprite = spriteRock;
            }
            if (manager.totalPower > breakingPower)
            {
                // âÛÇÍÇÈÇÊÇ§Ç…Ç∑ÇÈ
                isBreaking = true;
                maxCrackValue = 1;
            }

            // ÉJÉÅÉâÇóhÇÁÇ∑
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
