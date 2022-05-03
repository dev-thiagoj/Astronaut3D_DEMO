using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DestructableItemBase : MonoBehaviour
{
    public HealthBase healthBase;

    public float shakeDuration = .1f;

    public int shakeForce = 5;

    public int amountCoins = 10;
    public GameObject prefabCoin;
    public Transform dropPosition;

    private void OnValidate()
    {
        if (healthBase == null) healthBase = GetComponent<HealthBase>();
    }

    private void Awake()
    {
        OnValidate();
        healthBase.OnDamage += OnDamage;
    }

    private void OnDamage(HealthBase h)
    {
        transform.DOShakeScale(shakeDuration, Vector3.up/5, shakeForce);
        DropGroupOfCoins();
    }

    [NaughtyAttributes.Button]
    private void DropCoins()
    {
        var i = Instantiate(prefabCoin);
        i.transform.position = dropPosition.position;
        i.transform.DOScale(0, .5f).SetEase(Ease.OutBack).From();
    }

    private void DropGroupOfCoins()
    {
        /*for(int i = 0; i < amountCoins; i++)
        {
            DropCoins();
        }*/

        StartCoroutine(DropGroupOfCoinsCoroutine());

    }

    IEnumerator DropGroupOfCoinsCoroutine()
    {
        for (int i = 0; i < amountCoins; i++)
        {
            DropCoins();
            yield return new WaitForSeconds(.3f);
        }
    }
}
