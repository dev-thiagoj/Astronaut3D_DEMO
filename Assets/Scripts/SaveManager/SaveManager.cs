using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Ebac.Core.Singleton;

public class SaveManager : Singleton<SaveManager>
{
    private SaveSetup _saveSetup;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); //não sera destruido qdo carregar outra cena, mantendo sempre o mesmo desde que começa o jogo
        _saveSetup = new SaveSetup();
        _saveSetup.lastLevel = 2;
        _saveSetup.playerName = "Thiago";
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
        Save();
    }

    public void SaveName(string text)
    {
        _saveSetup.playerName = text;
        Save();
    }

    #endregion

    private void SaveFile(string json)
    {
        //string path = Application.dataPath + "/save.txt"; - salva o json na pasta do jogo
        //string path = Application.persistentDataPath + "/save.txt"; - salva o json no usuario do computador, fora do jogo
        string path = Application.streamingAssetsPath + "/save.txt"; // - salva em uma pasta dentro do inspector, necessario criar a pasta StreamingAssets primeiro (fica mais localizado)
        

        //string fileLoaded = "";

        //if (File.Exists(path)) fileLoaded = File.ReadAllText(path);

        Debug.Log(path);
        File.WriteAllText(path, json);
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
    public string playerName;
}
