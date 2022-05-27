using System.Collections;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public ProjectileBase prefabProjectile;
    
    public Transform positionToShoot;
    public float timeBetweenShoot = .2f;
    public float speed = 50f;

    private Coroutine _currentCoroutine;

    [Header("SFX")]
    //public SFXType sfxType;
    public AudioSource audioSource;

    protected virtual IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(timeBetweenShoot);
        }
    }

    protected virtual void Shoot()
    {   
        var projectile = Instantiate(prefabProjectile);
        projectile.transform.SetPositionAndRotation(positionToShoot.position, positionToShoot.rotation);
        projectile.speed = speed;
        PlayShootSFX();
    }

    public void PlayShootSFX()
    {
        SFXPool.Instance.Play(SFXType.SHOOT);
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
