using UnityEngine;
using Enemy;

public class EnemyTrigger : MonoBehaviour
{
    public EnemyShoot enemyShoot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyShoot.StartShooting();
        }

        else return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyShoot.StopShooting();
        }

        else return;
    }
}
