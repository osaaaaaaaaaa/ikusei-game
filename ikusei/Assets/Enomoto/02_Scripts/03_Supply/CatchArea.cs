using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;

    private void Update()
    {
        GameObject target = GetFoodObj();
        if (Input.GetMouseButtonDown(0) && target != null)
        {
            Destroy(target);
        }
    }

    GameObject GetFoodObj()
    {
        // 始点と終点
        Vector2 startPoint = transform.position - Vector3.right * 0.5f;
        Vector2 endPoint = transform.position + Vector3.right * 0.5f;

        //Debug.DrawLine(startPoint, endPoint); // 線を見えるようにする、デバック用

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
