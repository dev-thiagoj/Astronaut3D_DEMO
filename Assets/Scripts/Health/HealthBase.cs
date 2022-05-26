using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class HealthBase : MonoBehaviour, IDamageable
{
    public bool destroyOnKill = false;
    public float startLife = 10f;
    public float _currLife;
    public float timeToDestroy = 10;

    [Header("Screen Updater")]
    public List<UILifeUpdater> uiLifeUpdater;

    public float damageMultiply = 1;

    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnKill;


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
        if (uiLifeUpdater != null) uiLifeUpdater.ForEach(i => i.UpdateValue((float)_currLife / startLife));
    }


    #region ========== POWER_UP ==========
    public void ChangeDamageMultiply(float damage, float duration)
    {
        StartCoroutine(ChangeDamageMultiplyCoroutine(damage, duration));
    }

    IEnumerator ChangeDamageMultiplyCoroutine(float damageMultiply, float duration)
    {
        this.damageMultiply = damageMultiply;
        Player.Instance.strongIcon.enabled = true;
        yield return new WaitForSeconds(duration);

        this.damageMultiply = 1;
        Player.Instance.strongIcon.enabled = false;
    }
    #endregion
}
