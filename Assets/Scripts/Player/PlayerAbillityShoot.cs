using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerAbillityShoot : PlayerAbillityBase
{   
    public List<GunBase> gunBases;
    public Transform gunPosition;
    public FlashColor gunFlashColor;

    private GunBase _gunBaseIndex;
    private GunBase _currentGun;

    [Header("Canvas")]
    public Image infoGun01;
    public Image infoGun02;

    protected override void Init()
    {
        base.Init();

        _gunBaseIndex = gunBases[0];
        
        CreateGun();

        inputs.Gameplay.Shoot.performed += ctx => StartShoot();
        inputs.Gameplay.Shoot.canceled += ctx => CancelShoot();
    }

    private void Awake()
    {
        infoGun01.color = Color.green;
        infoGun02.color = Color.white;
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
        gunFlashColor?.Flash(); // esse interrogação é para checar se ele não é nulo. Se for nulo não executa o .Flash()
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
            infoGun01.color = Color.green;
            infoGun02.color = Color.white;
            CreateGun();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {   
            Destroy(_currentGun.gameObject);
            _gunBaseIndex = gunBases[1];
            infoGun01.color = Color.white;
            infoGun02.color = Color.green;
            CreateGun();
        }
    }
}
