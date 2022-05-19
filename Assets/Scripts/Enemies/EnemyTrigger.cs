using UnityEngine;
using Enemy;

public class EnemyTrigger : EnemyShoot
{
    public EnemyShoot enemyShoot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyShoot.StartShooting();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyShoot.StopShooting();
        }
    }
}
