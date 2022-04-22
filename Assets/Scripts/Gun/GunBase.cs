using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public ProjectileBase prefabProjectile;

    public Vector3 position;
    
    public Transform positionToShoot;
    public float timeBetweenShoot = .3f;

    private Coroutine _currentCoroutine;

    private void Update()
    {
        position = positionToShoot.transform.position;
    }

    protected virtual IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(timeBetweenShoot);
        }
    }

    public void Shoot()
    {   
        var projectile = Instantiate(prefabProjectile);
        projectile.transform.SetPositionAndRotation(position, positionToShoot.rotation);
        
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
