using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KanKikuchi.AudioManager;

public class CatchArea : MonoBehaviour
{
    [SerializeField] SupplyManager manager;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] Transform hand;
    Vector2 defaultHandPos;

    private void Start()
    {
        defaultHandPos = hand.localPosition;
    }

    void PlayCatchAnim()
    {
        DOTween.Kill(hand);
        hand.localPosition = defaultHandPos;
        hand.DOMoveY(-1.5f, 0.2f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }

    public void OnFoodButton(int index)
    {
        PlayCatchAnim();

        SEManager.Instance.Play(SEPath.SWING);

        GameObject target = GetFoodObj();
        if(target != null)
        {
            if (target.GetComponent<Food>().FoodID == index)
            {
                SEManager.Instance.Play(SEPath.HIT);
                manager.AddFoodCnt();
                Destroy(target);
            }
            else if(target.GetComponent<Food>().FoodID == (int)Food.FOOD_ID.Poop)
            {
                SEManager.Instance.Play(SEPath.MISS);
                manager.SubFoodCnt();
                Destroy(target);
            }
        }
    }

    GameObject GetFoodObj()
    {
        // 始点と終点
        Vector2 startPoint = transform.position - Vector3.right * 0.5f;
        Vector2 endPoint = transform.position + Vector3.right * 0.5f;

        // Rayを始点から飛ばす向きと距離
        Vector2 dir = (endPoint - startPoint).normalized;
        float dis = Vector2.Distance(startPoint, endPoint);

        Debug.DrawRay(startPoint, dir * dis, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(startPoint, dir, dis, targetLayer);
        if (hit.collider)
        {
            return hit.collider.gameObject;
        }

        return null;
    }
}
