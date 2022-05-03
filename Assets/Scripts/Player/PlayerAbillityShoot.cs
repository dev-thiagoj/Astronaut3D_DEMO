using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbillityShoot : PlayerAbillityBase
{   
    public List<GunBase> gunBases;
    public Transform gunPosition;

    private GunBase _gunBaseIndex;
    private GunBase _currentGun;

    protected override void Init()
    {
        base.Init();

        _gunBaseIndex = gunBases[0];

        CreateGun();

        inputs.Gameplay.Shoot.performed += ctx => StartShoot();
        inputs.Gameplay.Shoot.canceled += ctx => CancelShoot();
    }

    private void Update()
    {
        ChooseBetweensGuns();
    }

    private void CreateGun()
    {
        _currentGun = Instantiate(_gunBaseIndex, gunPosition);

        _currentGun.transform.localPosition = _currentGun.transform.localEulerAngles = Vector3.zero;
    }

    private void StartShoot()
    {
        _currentGun.StartShoot();
    }
    
    private void CancelShoot()
    {
        _currentGun.StopShoot();
    }

    private void ChooseBetweensGuns()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Destroy(_currentGun.gameObject);
            _gunBaseIndex = gunBases[0];
            CreateGun();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {   
            Destroy(_currentGun.gameObject);
            _gunBaseIndex = gunBases[1];
            CreateGun();
        }
    }
}
