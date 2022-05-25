using System.Collections;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [Header("Projectile")]
    public ProjectileBase prefabProjectile;
    public Transform shootPos;

    [Header("Shoots")]
    public int amountShoots = 4;
    public float angle = 15f;
    public float timeToShoot = 1f;
    public float projectileSpeed = 25f;

    private Coroutine _currCoroutine;

    private void Shoot()
    {
        var projectile = Instantiate(prefabProjectile);
        projectile.transform.SetPositionAndRotation(shootPos.position, shootPos.rotation);
        projectile.speed = projectileSpeed;
    }

    private void AngularShoots()
    {
        int mult = 0;

        for (int i = 0; i < amountShoots - 1; i++)
        {
            if (i % 2 == 0) mult++;

            var projectile = Instantiate(prefabProjectile, shootPos);

            projectile.transform.localPosition = Vector3.zero;
            projectile.transform.localEulerAngles = Vector3.zero +
                (Vector3.up * (i % 2 == 0 ? angle : -angle)) * mult;

            projectile.speed = projectileSpeed;

            projectile.transform.parent = null;
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(0.1f);
            if (amountShoots > 1) AngularShoots();
            yield return new WaitForSeconds(timeToShoot);
        }
    }

    public void StartShoot()
    {
        StopShoot();
        _currCoroutine = StartCoroutine(ShootCoroutine());
    }

    public void StopShoot()
    {
        if (_currCoroutine != null)
        {
            StopCoroutine(_currCoroutine);
        }
    }
}
