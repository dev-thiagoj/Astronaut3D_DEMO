using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DestructableItemBase : MonoBehaviour
{
    public HealthBase healthBase;

    public float shakeDuration = .1f;

    public int shakeForce = 5;

    public int amountCoins = 5;
    public GameObject prefabCoin;
    public Transform[] dropPositions;
    private int _index = 0;

    private Collider collider;

    private void OnValidate()
    {
        if (healthBase == null) healthBase = GetComponent<HealthBase>();
        if (collider == null) collider = GetComponent<Collider>();
    }

    private void Awake()
    {
        OnValidate();
        healthBase.OnDamage += OnDamage;
        healthBase.OnKill += OnKill;
    }

    private void OnDamage(HealthBase h)
    {
        transform.DOShakeScale(shakeDuration, Vector3.up / 5, shakeForce);
        //DropGroupOfCoins();
    }

    private void OnKill(HealthBase h)
    {
        collider.enabled = false;
        Invoke(nameof(DropGroupOfCoins), .1f);
    }

    [NaughtyAttributes.Button]
    private void DropCoins()
    {
        var i = Instantiate(prefabCoin);
        i.transform.position = dropPositions[_index].transform.position;
        i.transform.DOScale(0, .1f).SetEase(Ease.OutBack).From();

        _index++;
    }

    public void DropGroupOfCoins()
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
            yield return new WaitForSeconds(.1f);
        }
    }
}
