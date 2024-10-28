using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    [SerializeField] SupplyManager manager;
    [SerializeField] LayerMask targetLayer;

    public void OnFoodButton(int index)
    {
        GameObject target = GetFoodObj();
        if(target != null)
        {
            if(target.GetComponent<Food>().FoodID == index)
            {
                manager.AddFoodCnt();
                Destroy(target);
            }
            else if(target.GetComponent<Food>().FoodID == (int)Food.FOOD_ID.Poop)
            {
                Debug.Log("��������");
                Destroy(target);
            }
        }
    }

    GameObject GetFoodObj()
    {
        // �n�_�ƏI�_
        Vector2 startPoint = transform.position - Vector3.right * 0.5f;
        Vector2 endPoint = transform.position + Vector3.right * 0.5f;

        // Ray���n�_�����΂������Ƌ���
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
