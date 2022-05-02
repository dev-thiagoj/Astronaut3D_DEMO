using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BounceHelper : MonoBehaviour
{
    public float bounceScale = 1.2f;
    public float bounceDuration = .5f;
    public Ease bounceEase = Ease.OutBack;

    private void Update()
    {
        //KeepScale();
    }

    public void KeepScale()
    {
        //transform.localScale = Vector3.one;
    }
    
    [NaughtyAttributes.Button]
    public void TransformBounce()
    {
        transform.DOScale(bounceScale, bounceDuration).SetEase(bounceEase);
    }
    
    [NaughtyAttributes.Button]
    public void TransformBounceWithYoyo()
    {
        transform.DOScale(bounceScale, bounceDuration).SetEase(bounceEase).SetLoops(2, LoopType.Yoyo);
    }
}
