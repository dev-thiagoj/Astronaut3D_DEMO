using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbillityShoot : PlayerAbillityBase
{
    public GunBase gunBase;

    protected override void Init()
    {
        base.Init();

        inputs.Gameplay.Shoot.performed += ctx => StartShoot();
        inputs.Gameplay.Shoot.canceled += ctx => CancelShoot();
    }

    private void StartShoot()
    {
        gunBase.StartShoot();
        Debug.Log("Start Shoot");
    }
    
    private void CancelShoot()
    {
        gunBase.StopShoot();
        Debug.Log("Cancel Shoot");
    }
}
