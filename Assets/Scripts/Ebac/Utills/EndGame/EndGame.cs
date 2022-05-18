using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndGame : MonoBehaviour
{
    public List<GameObject> endGameObjects;

    private bool _endGame = false;

    public int currentLevel = 1;

    public Boss.BossBase bossBase;

    private void Awake()
    {
        endGameObjects.ForEach(i => i.SetActive(false));
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

        foreach(var i in endGameObjects)
        {
            i.SetActive(true);
            i.transform.DOScale(0, 0.2f).SetEase(Ease.OutBack).From();

            SaveManager.Instance.SaveLastLevel(currentLevel);
        }

    }
}
