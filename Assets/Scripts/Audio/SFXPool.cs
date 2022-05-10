using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;

public class SFXPool : Singleton<SFXPool>
{
    public int poolSize = 10;
    
    private List<AudioSource> _audioSourcesList;
    private int _index = 0;

    protected override void Awake()
    {
        base.Awake();

        CreatePool();
    }

    private void CreatePool()
    {
        _audioSourcesList = new List<AudioSource>(); // 1 - instancia uma lista nova
        
        for(int i = 0; i < poolSize; i++) // 5 - cria o pool
        {
            CreateAudioSourceItem();
        }
    }

    private void CreateAudioSourceItem()
    {
        GameObject go = new GameObject("SFX_Pool"); // 2 - cria o objeto
        go.transform.SetParent(gameObject.transform); // 3 - cria dentro do gameobject pai na cena
        _audioSourcesList.Add(go.AddComponent<AudioSource>()); // 4 - adiciona um Audio Source no objeto criado
    }

    public void Play(SFXType sfxType)
    {
        if (sfxType == SFXType.NONE) return;

        var sfx = SoundManager.Instance.GetSFXByType(sfxType);

        _audioSourcesList[_index].clip = sfx.audioClip;
        _audioSourcesList[_index].Play();

        _index++;

        if (_index >= _audioSourcesList.Count) _index = 0; // retorna ao começo do pool
    }
}
