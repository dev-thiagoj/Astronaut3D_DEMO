using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayerHelper : MonoBehaviour
{
    //public SFXType sfxType;
    public AudioSource audioSource;

    [Header("Delays")]
    public float minDelay = 3f;
    public float maxDelay = 10f;

    private void OnValidate()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (audioSource == null) return;
        StartCoroutine(PlayAudioCoroutine());
    }

    public void PlayFootstepsSFX()
    {
        SFXPool.Instance.Play(SFXType.FOOTSTEPS);
    }

    public void PlayAudioSource()
    {
        audioSource.Play();
    } 

    IEnumerator PlayAudioCoroutine()
    {
        while (true)
        {
            PlayAudioSource();
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
