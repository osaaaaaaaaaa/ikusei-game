using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashing : MonoBehaviour
{
    int invincibleCnt;
    int invincibleCntMax;
    float currentTimeInvincible;
    float triggerTimeInvincible;
    bool isPlayFlashing;

    // Start is called before the first frame update
    void Start()
    {
        invincibleCnt = 0;
        invincibleCntMax = 4;
        currentTimeInvincible = 0;
        triggerTimeInvincible = 0.2f;
        isPlayFlashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayFlashing) return;

        currentTimeInvincible += Time.deltaTime;
        if (currentTimeInvincible >= triggerTimeInvincible)
        {
            // 一定間隔で点滅させる
            currentTimeInvincible = 0;
            invincibleCnt++;
            GetComponent<Image>().enabled = !GetComponent<Image>().enabled;
        }

        if (invincibleCnt >= invincibleCntMax)
        {
            // 上限数点滅したら元に戻す
            currentTimeInvincible = 0;
            invincibleCnt = 0;
            GetComponent<Image>().enabled = false;
            isPlayFlashing = false;
        }
    }

    public void PlayFlashing()
    {
        isPlayFlashing = true;
    }
}
