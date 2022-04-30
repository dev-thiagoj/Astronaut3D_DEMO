using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class EnemyTrigger : EnemyShoot
{
    public EnemyShoot enemyShoot;
    public EnemyBase enemyBase;
    //public GunBase gunBase;

    private void OnValidate()
    {
        //enemyBase = FindObjectOfType<EnemyBase>();
        //enemyShoot = FindObjectOfType<EnemyShoot>();
    }

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
