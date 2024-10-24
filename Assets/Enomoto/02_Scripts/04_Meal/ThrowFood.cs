using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFood : MonoBehaviour
{
    [SerializeField] GameObject eatingEffect;
    [SerializeField] string colorString;
    Color createColor;
    HungerGageController gageController;
    bool isThrow;
    const float dragSpeed = 100;

    private void Awake()
    {
        isThrow = false;
        gageController = GameObject.Find("HungerGage").GetComponent<HungerGageController>();

        // �F���쐬
        ColorUtility.TryParseHtmlString(colorString, out createColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) isThrow = true;
        if (isThrow) return;

        if (Input.GetMouseButton(0))
        {
            // �h���b�O����
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (worldPos - transform.position).normalized;
            transform.GetComponent<Rigidbody2D>().velocity = dir * dragSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeathZone")
        {
            Debug.Log("�s�@�����s�@�����s�@����");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isThrow && collision.tag == "Monster")
        {
            Debug.Log("���ւ�");
            gageController.UpdateGage(Constant.GetHungerIncrease());

            // �H�ׂ�G�t�F�N�g�𔭐�������
            var effect = Instantiate(eatingEffect);
            effect.transform.position = this.transform.position;
            effect.GetComponent<ParticleSystem>().startColor = createColor;
            effect.GetComponent<ParticleSystem>().Play();

            Destroy(this.gameObject);
        }
    }
}
