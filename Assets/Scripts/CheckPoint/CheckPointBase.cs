using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBase : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public int key = 01;

    private bool checkpointActived = false;
    private string checkPointKey = "CheckpointKey";

    private void Awake()
    {
        TurnItOff();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!checkpointActived && other.transform.CompareTag("Player"))
        {
            CheckCheckpoint();

        }
    }

    private void CheckCheckpoint()
    {
        TurnItOn();
        SaveCheckpoint();
    }

    [NaughtyAttributes.Button]
    private void TurnItOn()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.white);
    }

    [NaughtyAttributes.Button]
    private void TurnItOff()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.black);
    }

    private void SaveCheckpoint()
    {
        /*if (PlayerPrefs.GetInt(checkPointKey, 0) > key) //checagem para garantir que um save menor não sobrescreva um maior, para sempre ter o maior salvo (o mais "distante")
            PlayerPrefs.SetInt(checkPointKey, key);*/

        CheckpointManager.Instance.SaveCheckpoint(key);

        checkpointActived = true;
    }
}
