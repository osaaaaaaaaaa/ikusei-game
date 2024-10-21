using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MiniGameManager1 : MonoBehaviour
{
    [SerializeField] GameObject gage1;
    [SerializeField] List<GameObject> gage2List;
    [SerializeField] GameObject gage3;
    [SerializeField] GameObject gage3StartPoint;
    public float endTime;
    bool isTap;
    bool isPlayTween;

    enum MINIGAME1_STATE
    {
        Opening,
        Gage1,
        Gage2,
        Gage3,
        Result
    }
    MINIGAME1_STATE state;

    private void Awake()
    {
        isTap = false;
        isPlayTween = false;
        state = MINIGAME1_STATE.Opening;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTap)
        {
            isTap = true;
            if (state == MINIGAME1_STATE.Gage2)
            {
                for (int i = 0; i < gage2List.Count; i++)
                {
                    DOTween.Kill(gage2List[i].transform);
                }
            }
            if(state == MINIGAME1_STATE.Gage3) DOTween.Kill(gage3.transform);

            Invoke("UpdateGameState", 0.5f);
        }
        if (isTap) return;

        switch (state)
        {
            case MINIGAME1_STATE.Opening:
                break;
            case MINIGAME1_STATE.Gage1:
                gage1.SetActive(true);
                if (gage1.GetComponent<Slider>().value >= 1) gage1.GetComponent<Slider>().value = 0;
                else gage1.GetComponent<Slider>().value += Time.deltaTime;
                break;
            case MINIGAME1_STATE.Gage2:
                if (isPlayTween) return;
                isPlayTween = true;
                for (int i = 0; i < gage2List.Count; i++)
                {
                    gage2List[i].SetActive(true);

                    float mul = 1.5f;
                    Vector3 startPointGage2 = gage2List[i].transform.localPosition;
                    Vector3[] pathGage2 =
                    {
                        startPointGage2 + (Vector3.right * mul + Vector3.up * mul) * (i == 0 ? 1 : -1),    // ‰Eãor¶‰º
                        startPointGage2 + (Vector3.up * mul) * (i == 0 ? 1 : -1),                          // ãor‰º
                        startPointGage2 + (Vector3.left * mul + Vector3.up * mul) * (i == 0 ? 1 : -1),     // ¶ã or ‰Eã
                        startPointGage2,
                    };

                    gage2List[i].transform.DOLocalPath(pathGage2, endTime, PathType.CatmullRom).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
                }
                break;
            case MINIGAME1_STATE.Gage3:
                if (isPlayTween) return;
                gage3.SetActive(true);
                gage3StartPoint.SetActive(true);
                isPlayTween = true;
                Vector3 startPointGage3 = gage3.transform.localPosition;
                Vector3[] pathGage3 =
                {
                    new Vector3(1.3f,-1f,0f),
                    new Vector3(2.3f,-0.1f,0f),
                    new Vector3(1.3f,-1f,0f),
                    startPointGage3,
                    new Vector3(-1.3f,-1f,0f),
                    new Vector3(-2.3f,-0.1f,0f),
                    new Vector3(-1.3f,-1f,0f),
                    startPointGage3,
                };

                gage3.transform.DOLocalPath(pathGage3, endTime, PathType.CatmullRom).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

                break;
            case MINIGAME1_STATE.Result:
                break;
        }
    }


    void UpdateGameState()
    {
        switch (state)
        {
            case MINIGAME1_STATE.Gage1:
                gage1.SetActive(false);
                Debug.Log("ƒQ[ƒW‚P(Min0,Max1)F" + gage1.GetComponent<Slider>().value);
                break;
            case MINIGAME1_STATE.Gage2:
                for (int i = 0; i < gage2List.Count; i++)
                {
                    gage2List[i].SetActive(false);
                }
                Debug.Log("ƒQ[ƒW‚Q(‹——£Min0,‹——£Max‚í‚©‚ñ‚È‚¢)F" + Mathf.Abs(Vector3.Distance(gage2List[0].transform.localPosition, gage2List[1].transform.localPosition)));
                break;
            case MINIGAME1_STATE.Gage3:
                gage3.SetActive(false);
                gage3StartPoint.SetActive(false);
                Debug.Log("ƒQ[ƒW‚R(‹——£Min0,‹——£Max‚í‚©‚ñ‚È‚¢)F" + Mathf.Abs(Vector3.Distance(gage3StartPoint.transform.position,gage3.transform.position)));
                break;
        }

        if (state < MINIGAME1_STATE.Result) state++;
        isTap = false;
        isPlayTween = false;
    }
}
