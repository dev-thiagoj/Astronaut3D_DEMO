using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;

public class SoundManager : Singleton<SoundManager>
{
    public List<MusicSetup> musicSetups;
    public List<SFXSetup> sfxSetups;

    [Header("Sound On/Off")]
    public GameObject buttonSoundOff;
    public GameObject buttonSoundOn;
    public AudioSource musicSource;

    protected override void Awake()
    {
        base.Awake();

        buttonSoundOff.SetActive(false);
        buttonSoundOn.SetActive(true);
    }

    public void PlayMusicbyType(MusicType musicType)
    {
        var music = GetMusicByType(musicType);

        musicSource.clip = music.audioClip;
        musicSource.Play();
    }

    public MusicSetup GetMusicByType(MusicType musicType)
    {
        return musicSetups.Find(i => i.musicType == musicType);
    }
    
    public SFXSetup GetSFXByType(SFXType sfxType)
    {
        return sfxSetups.Find(i => i.sfxType == sfxType);
    }

    public void TurnMusicOff()
    {
        musicSource.enabled = false;
        musicSource.Pause();
        buttonSoundOn.SetActive(false);
        buttonSoundOff.SetActive(true);
    }

    public void TurnMusicOn()
    {
        musicSource.enabled = true;
        musicSource.Play();
        buttonSoundOff.SetActive(false);
        buttonSoundOn.SetActive(true);
    }
}

public enum MusicType
{
    NONE,
    AMBIENCE_MAIN,
    LEVEL_WIN,
    LEVEL_LOSE,
}

[System.Serializable]
public class MusicSetup
{
    public MusicType musicType;
    public AudioClip audioClip;
}

public enum SFXType
{
    NONE,
    COIN_COLLECT,
    LIFEPACK_COLLECT,
    FOOTSTEPS,
    SHOOT,
    BOSS_WAKEUP,
    BOSS_DEATH,
    CHECKPOINT
}

[System.Serializable]
public class SFXSetup
{
    public SFXType sfxType;
    public AudioClip audioClip;
}
