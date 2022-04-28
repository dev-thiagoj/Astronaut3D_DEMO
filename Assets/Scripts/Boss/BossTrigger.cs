using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;

public class BossTrigger : MonoBehaviour
{
    public BossBase bossBase;

    private void OnValidate()
    {
        bossBase = GameObject.FindObjectOfType<BossBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Trigger Enter");
            //bossBase.SwitchWalk(); //walk
            bossBase.lookAtPlayer = true;
            bossBase.SwitchState(BossAction.WALK);
            gameObject.SetActive(false);
        }
    }
}
