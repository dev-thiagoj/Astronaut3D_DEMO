using UnityEngine;
using DG.Tweening;

public class BounceHelper : MonoBehaviour
{
    public float bounceScale = 1.2f;
    public float bounceDuration = .5f;
    public Ease bounceEase = Ease.OutBack;


    public void TransformBounce()
    {
        transform.DOScale(bounceScale, bounceDuration).SetEase(bounceEase);
    }

    public void TransformBounceWithYoyo()
    {
        transform.DOScale(bounceScale, bounceDuration).SetEase(bounceEase).SetLoops(2, LoopType.Yoyo);
    }
}
