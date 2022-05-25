using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBase : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public int key = 01;

    [Header("SFX")]
    public SFXType sfxType;

    private bool _checkpointActived = false;

    private void Awake()
    {
        TurnItOff();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_checkpointActived && other.transform.CompareTag("Player"))
        {
            CheckCheckpoint();
        }
    }

    private void CheckCheckpoint()
    {
        TurnItOn();
        SaveCheckpoint();
    }

    private void TurnItOn()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.white);
        SFXPool.Instance.Play(sfxType);
    }

    private void TurnItOff()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.black);
    }

    private void SaveCheckpoint()
    {
        CheckpointManager.Instance.SaveCheckpoint(key);
        SaveManager.Instance.SaveDataInCheckpoints();

        _checkpointActived = true;
    }
}
