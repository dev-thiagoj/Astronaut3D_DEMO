using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayerHelper : MonoBehaviour
{
    public SFXType sfxType;

    public void PlaySFX()
    {
        SFXPool.Instance.Play(sfxType);
    }
}
