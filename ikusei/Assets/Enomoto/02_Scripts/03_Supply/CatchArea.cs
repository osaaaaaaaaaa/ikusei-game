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
        // �n�_�ƏI�_
        Vector2 startPoint = transform.position - Vector3.right * 0.5f;
        Vector2 endPoint = transform.position + Vector3.right * 0.5f;

        //Debug.DrawLine(startPoint, endPoint); // ����������悤�ɂ���A�f�o�b�N�p

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
