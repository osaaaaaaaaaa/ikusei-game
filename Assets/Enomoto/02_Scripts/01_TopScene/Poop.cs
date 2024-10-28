using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Poop : MonoBehaviour
{
    [SerializeField] GameObject hitParticle;

    public void Destroy()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        var particle= Instantiate(hitParticle);
        particle.transform.position = this.transform.position;
        this.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).SetEase(Ease.Linear).OnComplete(() => { Destroy(gameObject); });
    }
}
