using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbillityShoot : PlayerAbillityBase
{   
    public List<GunBase> gunBases;
    public Transform gunPosition;

    //public GunBase gunBase;

    public FlashColor gunFlashColor;
    private GunBase _gunBaseIndex;
    private GunBase _currentGun;

    private void OnValidate()
    {
        //_gunFlashColor = GetComponentInChildren<FlashColor>(); não pude fazer assim pois existe outro flash color no player pra qdo sofre dano, teve que ser publico para eu selecionar qual flash manualmente
    }

    private void Awake()
    {
        OnValidate();
    }

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
        gunFlashColor?.Flash(); // esse interrogação é para checar se ele não é nulo. Se for nulo não executa o .Flash()

        //gunBase.StartShoot(); //
    }
    
    private void CancelShoot()
    {
        _currentGun.StopShoot();

        //gunBase.StopShoot();
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
