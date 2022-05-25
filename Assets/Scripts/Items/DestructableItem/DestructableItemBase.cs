using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DestructableItemBase : MonoBehaviour
{
    public HealthBase healthBase;

    [Header("Shake Animation")]
    public float shakeDuration = .1f;
    public int shakeForce = 5;

    public int amountCoins = 5;
    public GameObject prefabCoin;
    public Transform[] dropPositions;
    private int _index = 0;

    private Collider _collider;

    private void OnValidate()
    {
        if (healthBase == null) healthBase = GetComponent<HealthBase>();
        if (_collider == null) _collider = GetComponent<Collider>();
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
    }

    private void OnKill(HealthBase h)
    {
        _collider.enabled = false;
        Invoke(nameof(DropGroupOfCoins), .1f);
    }

    private void DropCoins()
    {
        var i = Instantiate(prefabCoin);
        i.transform.position = dropPositions[_index].transform.position;
        i.transform.DOScale(0, .1f).SetEase(Ease.OutBack).From();

        _index++;
    }

    public void DropGroupOfCoins()
    {
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
