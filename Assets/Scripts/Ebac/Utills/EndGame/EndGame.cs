using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndGame : MonoBehaviour
{
    public List<GameObject> endGameObjects;
    public GameObject restartScreen;
    public int currentLevel = 1;
    public Boss.BossBase bossBase;

    private bool _endGame = false;

    private void Awake()
    {
        endGameObjects.ForEach(i => i.SetActive(false));
        restartScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.transform.GetComponent<Player>();

        if (!_endGame && p != null && bossBase._isAlive == false) ShowEndGame();
        else return;
    }

    private void ShowEndGame()
    {
        _endGame = true;

        MusicPlayer.Instance.PlayWinJingle();

        foreach(var i in endGameObjects)
        {
            i.SetActive(true);
            i.transform.DOScale(0, 0.2f).SetEase(Ease.OutBack).From();

            SaveManager.Instance.SaveLastLevel(currentLevel);
            Invoke(nameof(ShowRestartGame), 5);
        }
    }

    public void ShowRestartGame()
    {
        restartScreen.SetActive(true);
        Player.Instance.isAlive = false;
    }

    public void SpawnRestart()
    {
        LoadSceneHelper.Instance.LoadLevel(0);
        SaveManager.Instance.CreateNewSave();
    }
}
