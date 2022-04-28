using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    public float startLife = 10f;
    public bool destroyOnKill = false;

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

    protected void ResetLife()
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
        //if (flashColor != null) flashColor.Flash();
        //if (particleSystem != null) particleSystem.Emit(intParticles);

        _currLife -= f;

        if (_currLife <= 0)
        {
            Kill();
        }

        OnDamage?.Invoke(this);

    }
}
