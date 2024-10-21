using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            // àÍíËä‘äuÇ≈ì_ñ≈Ç≥ÇπÇÈ
            currentTimeInvincible = 0;
            invincibleCnt++;
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }

        if(invincibleCnt >= invincibleCntMax)
        {
            // è„å¿êîì_ñ≈ÇµÇΩÇÁå≥Ç…ñﬂÇ∑
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
