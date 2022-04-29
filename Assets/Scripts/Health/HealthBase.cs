using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour, IDamageable
{
    public float startLife = 10f;
    public bool destroyOnKill = false;

    public List<UIFillUpdater> uiFillUpdater;

    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnKill;

    public float _currLife;

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
    }

    protected virtual void Kill()
    {
        if (destroyOnKill)
            Destroy(gameObject, 10f);

        OnKill?.Invoke(this);
    }

    public void Damage(float f)
    {
        _currLife -= f;

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
}
