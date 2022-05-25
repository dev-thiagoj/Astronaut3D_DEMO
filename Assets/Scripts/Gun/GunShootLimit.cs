using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GunShootLimit : GunBase
{
    public List<UIAmmoUpdater> uiAmmoUpdaters;

    public float maxShoot = 5f;
    public float timeToRecharge = 1f;

    private float _currentShoots;
    private bool _recharging = false;

    private void Start()
    {
        GetAllUIs();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) StartRecharge();
        if (maxShoot == 0) maxShoot = 1;
    }

    protected override IEnumerator ShootCoroutine()
    {
        if (_recharging) yield break;

        while (true)
        {
            if (_currentShoots < maxShoot)
            {
                Shoot();
                _currentShoots++;

                CheckRecharge();
                UpdateUI();

                yield return new WaitForSeconds(timeToRecharge);
            }

            CheckRecharge();
        }
    }

    private void CheckRecharge()
    {
        if (_currentShoots >= maxShoot)
        {
            StopShoot();
            StartRecharge();
        }
    }

    private void StartRecharge()
    {
        _recharging = true;
        StartCoroutine(RechargeCoroutine());
    }

    IEnumerator RechargeCoroutine()
    {
        float time = 0;

        while (time < timeToRecharge)
        {
            time += Time.deltaTime;

            uiAmmoUpdaters.ForEach(i => i.UpdateValue(time / timeToRecharge));

            yield return new WaitForEndOfFrame();
        }

        _currentShoots = 0;
        _recharging = false;
    }

    private void UpdateUI()
    {
        uiAmmoUpdaters.ForEach(i => i.UpdateValue(maxShoot, _currentShoots));
    }

    private void GetAllUIs()
    {
        uiAmmoUpdaters = FindObjectsOfType<UIAmmoUpdater>().ToList();
    }
}
