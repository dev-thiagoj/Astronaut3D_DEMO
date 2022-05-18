using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Ebac.Core.Singleton;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField] private SaveSetup _saveSetup;

    private string _path = Application.streamingAssetsPath + "/save.txt"; // - salva em uma pasta dentro do inspector, necessario criar a pasta StreamingAssets primeiro (fica mais localizado)
    //string path = Application.dataPath + "/save.txt"; - salva o json na pasta do jogo
    //string path = Application.persistentDataPath + "/save.txt"; - salva o json no usuario do computador, fora do jogo

    public int lastlevel;

    public Action<SaveSetup> FileLoaded;

    public SaveSetup Setup
    {
        get { return _saveSetup; }
    }


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); //não sera destruido qdo carregar outra cena, mantendo sempre o mesmo desde que começa o jogo
    }

    private void CreateNewSave()
    {
        _saveSetup = new SaveSetup();
        _saveSetup.lastLevel = 0;
        _saveSetup.playerName = "Test";
    }

    private void Start()
    {
        //LoadFile();
        Invoke(nameof(LoadFile), 0.1f);
    }

    #region Save

    [NaughtyAttributes.Button]
    public void Save()
    {
        string setupToJson = JsonUtility.ToJson(_saveSetup, true);
        Debug.Log(setupToJson);
        SaveFile(setupToJson);
    }

    public void SaveDataInCheckpoints()
    {
        SaveCheckpoints();
        SaveItens();
        //SaveCurrCloth();
        SaveLifeStatus();
        Save();
    }

    /*public void SaveDataWhenGetKilled()
    {
        SaveCheckpoints();
        SaveItens();
        SaveCurrCloth();
        SaveLifeStatus();
        Save();
    }*/

    public void SaveLastLevel(int level)
    {
        _saveSetup.lastLevel = level;
        SaveDataInCheckpoints();
        Save();
    }

    public void SaveName(string text)
    {
        _saveSetup.playerName = text;
        Save();
    }

    public void SaveItens()
    {
        _saveSetup.coins = Itens.ItemManager.Instance.GetItemByType(Itens.ItemType.COIN).so_Int.value;
        _saveSetup.lifePack = Itens.ItemManager.Instance.GetItemByType(Itens.ItemType.LIFE_PACK).so_Int.value;
        //Save();
    }

    public void SaveCheckpoints()
    {
        _saveSetup.lastCheckpoint = CheckpointManager.Instance.lastCheckpointKey;
        //Save();
    }

    public void SaveCurrCloth()
    {

    }

    public void SaveLifeStatus()
    {
        _saveSetup.lifeStatus = Player.Instance.healthBase._currLife;
        //Save();
    }

    #endregion

    private void SaveFile(string json)
    {
        Debug.Log(_path);
        File.WriteAllText(_path, json);
    }

    [NaughtyAttributes.Button]
    private void LoadFile()
    {
        string fileLoaded = "";

        if (File.Exists(_path))
        {
            fileLoaded = File.ReadAllText(_path);
            _saveSetup = JsonUtility.FromJson<SaveSetup>(fileLoaded);

            lastlevel = _saveSetup.lastLevel;
        }

        else
        {
            CreateNewSave();
            Save();
        }

        FileLoaded.Invoke(_saveSetup);
    }
}


[System.Serializable]
public class SaveSetup
{
    public int lastLevel;
    public int coins;
    public int lifePack;
    public int lastCheckpoint;
    public int currCloth; //
    public float lifeStatus; // <----------- atualmente
    public string playerName;
}
