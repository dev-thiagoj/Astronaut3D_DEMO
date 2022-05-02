using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;

public class BossTrigger : MonoBehaviour
{
    public BossBase bossBase;
    public GameObject bossCamera;
    public Color gizmoColor = Color.yellow;

    private void OnValidate()
    {
        bossBase = GameObject.FindObjectOfType<BossBase>();
    }

    private void Awake()
    {
        OnValidate();
        bossCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TurnBossCameraOn();

            bossBase.lookAtPlayer = true;
            bossBase.SwitchState(BossAction.WALK);
            gameObject.SetActive(false);
        }
    }

    private void TurnBossCameraOn()
    {
        bossCamera.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, transform.localScale.y);
    }
}
