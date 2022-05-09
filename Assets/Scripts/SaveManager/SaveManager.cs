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
        _saveSetup.playerName = "Thiago";
    }

    private void Start()
    {
        Invoke(nameof(LoadFile), 0.1f);
    }

    #region Save

    [NaughtyAttributes.Button]
    private void Save()
    {
        string setupToJson = JsonUtility.ToJson(_saveSetup, true);
        Debug.Log(setupToJson);
        SaveFile(setupToJson);
    }

    public void SaveLastLevel(int level)
    {
        _saveSetup.lastLevel = level;
        SaveItens();
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
        Save();
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

    #region DEBUG

    [NaughtyAttributes.Button]
    private void SaveLevelOne()
    {
        SaveLastLevel(1);
    }

    [NaughtyAttributes.Button]
    private void SaveLevelFive()
    {
        SaveLastLevel(5);
    }

    #endregion
}


[System.Serializable]
public class SaveSetup
{
    public int lastLevel;
    public int coins;
    public int lifePack;
    public string playerName;
}
