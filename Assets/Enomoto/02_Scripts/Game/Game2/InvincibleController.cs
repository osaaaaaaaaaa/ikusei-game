using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvincibleController : MonoBehaviour
{
    [SerializeField] MiniGameManager2 gameManager;
    SpriteRenderer spriteRenderer;
    int invincibleCnt;
    int invincibleCntMax;
    float currentTimeInvincible;
    float triggerTimeInvincible;

    // Start is called before the first frame update
    void Start()
    {
        invincibleCnt = 0;
        invincibleCntMax = 10;
        currentTimeInvincible = 0;
        triggerTimeInvincible = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.IsInvincible) return;

        currentTimeInvincible += Time.deltaTime;
        if (currentTimeInvincible >= triggerTimeInvincible)
        {
            // 一定間隔で点滅させる
            currentTimeInvincible = 0;
            invincibleCnt++;
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }

        if(invincibleCnt >= invincibleCntMax)
        {
            // 上限数点滅したら元に戻す
            currentTimeInvincible = 0;
            invincibleCnt = 0;
            gameManager.IsInvincible = false;
            spriteRenderer.enabled = true;
        }
    }

    public void PlayInvincibleAnim(SpriteRenderer _spriteRenderer)
    {
        gameManager.IsInvincible = true;
        spriteRenderer = _spriteRenderer;
    }
}
