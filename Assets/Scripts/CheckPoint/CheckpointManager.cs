using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;

public class CheckpointManager : Singleton<CheckpointManager>
{
    public int lastCheckpointKey = 0;

    public List<CheckPointBase> checkpoints;

    protected override void Awake()
    {
        base.Awake();

        LoadLastCheckpointFromSave();
    }

    private void Start()
    {

        //if (lastCheckpointKey > 0) transform.position = GetPositionFromLastCheckpoint();
    }

    public bool HasCheckpoint()
    {
        return lastCheckpointKey > 0; //se não tiver, retorna falso.
    }

    public void SaveCheckpoint(int i)
    {
        if (i > lastCheckpointKey)
        {
            lastCheckpointKey = i;
        }
    }

    public Vector3 GetPositionFromLastCheckpoint()
    {

        var checkpoint = checkpoints.Find(i => i.key == lastCheckpointKey);

        return checkpoint.transform.position; // <-------------------------- erro


        //else return Player.Instance.initialPos.transform.position;
    }

    public void LoadLastCheckpointFromSave()
    {
        lastCheckpointKey = SaveManager.Instance.Setup.lastCheckpoint;
    }
}
