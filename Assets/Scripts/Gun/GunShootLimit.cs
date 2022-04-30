using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//COISAS PARA FAZER:

// - ajustar para pegar somente os Ui de ammo e deixar de fora os de health

public class GunShootLimit : GunBase
{
    public List<UIFillUpdater> uiFillUpdaters;

    public float maxShoot = 5f;
    public float timeToRecharge = 1f;

    private float _currentShoots;
    private bool _recharging = false;

    private void Awake()
    {
        GetAllUIs();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) StartRecharge();

    }

    protected override IEnumerator ShootCoroutine()
    {
        if (_recharging) yield break; //esse check serve para se estiver recarregando, quebra a coroutine, evitando que passe e entre em loop infinito
                                      //no while (true)  

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

            //qdo caisse aqui, entraria em loop infinito e quebraria a Unity
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

            uiFillUpdaters.ForEach(i => i.UpdateValue(time / timeToRecharge));

            yield return new WaitForEndOfFrame();
        }

        _currentShoots = 0;
        _recharging = false;
    }

    private void UpdateUI()
    {
        uiFillUpdaters.ForEach(i => i.UpdateValue(maxShoot, _currentShoots));
    }

    private void GetAllUIs()
    {
        uiFillUpdaters = FindObjectsOfType<UIFillUpdater>().ToList();
    }
}
