using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
//using Cloth;
//using Ebac.Core.Singleton;

public class HealthBase : MonoBehaviour, IDamageable
{
    public bool destroyOnKill = false;
    public float startLife = 10f;
    public float _currLife; //não pode ser privado pois o boss base acessa ele
    public float timeToDestroy = 10;

    public List<UIFillUpdater> uiFillUpdater;

    public float damageMultiply = 1;

    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnKill;

    //[Space]
    //[SerializeField] private ClothChange clothChange;


    //public DestructableItemBase destructableItemBase;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        ResetLife();
    }

    public void ResetLife()
    {
        _currLife = startLife;
        UpdateUI();
    }

    protected virtual void Kill()
    {
        if (destroyOnKill)
        {   
            Destroy(gameObject, timeToDestroy);
        }

        OnKill?.Invoke(this);
    }

    [NaughtyAttributes.Button]
    public void Damage()
    {
        Damage(5);
    }

    public void Damage(float f)
    {
        _currLife -= f * damageMultiply;

        if (_currLife <= 0)
        {
            Kill();
        }

        UpdateUI();

        OnDamage?.Invoke(this);
    }

    public void Damage(float damage, Vector3 dir)
    {
        Damage(damage);
    }

    private void UpdateUI()
    {
        if (uiFillUpdater != null) uiFillUpdater.ForEach(i => i.UpdateValue((float)_currLife / startLife));
    }

    public void ChangeDamageMultiply(float damage, float duration)
    {
        StartCoroutine(ChangeDamageMultiplyCoroutine(damage, duration));
    }

    IEnumerator ChangeDamageMultiplyCoroutine(float damageMultiply, float duration)
    {
        this.damageMultiply = damageMultiply;
        yield return new WaitForSeconds(duration);

        this.damageMultiply = 1;
    }
}
