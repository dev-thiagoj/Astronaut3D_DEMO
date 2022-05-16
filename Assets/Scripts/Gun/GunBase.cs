using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COISAS PARA FAZER:

// - arrumar a gun angle

public class GunBase : MonoBehaviour
{
    public ProjectileBase prefabProjectile;
    
    public Transform positionToShoot;
    //public Vector3 pos;
    public float timeBetweenShoot = .2f;
    public float speed = 50f;

    private Coroutine _currentCoroutine;
    //private Player _player;

    [Header("SFX")]
    //public SoundManager soundManager;
    public SFXType sfxType;
    public AudioSource audioSource;
    //private SFXPlayer sfxPlayer;

    private void OnValidate()
    {
        //_player = FindObjectOfType<Player>();
        //if (sfxPlayer == null) sfxPlayer = GetComponent<SFXPlayer>();

    }

    protected virtual IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(Random.Range(2, 10));
        }
    }

    protected virtual void Shoot()
    {   
        var projectile = Instantiate(prefabProjectile);
        projectile.transform.SetPositionAndRotation(positionToShoot.position, positionToShoot.rotation);
        projectile.speed = speed;
        //_player.ShootAnimation();
        PlayShootSFX();
    }

    public void PlayShootSFX()
    {
        SFXPool.Instance.Play(sfxType);
    }

    public void StartShoot()
    {
        StopShoot();
        _currentCoroutine = StartCoroutine(ShootCoroutine());
    }

    public void StopShoot()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
    }
}
