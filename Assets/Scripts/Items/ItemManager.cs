using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using TMPro;

public class ItemManager : /*Singleton<ItemManager>*/ MonoBehaviour
{
    //tentei fazer usando o singleton mas não funcionou, qdo coleto as moedas no jogo me aparece um erro de instancia

    public static ItemManager Instance;

    public TextMeshProUGUI uiTextCoins;
    public TextMeshProUGUI uiTextLife;

    public SO_int coins;
    public SO_int life;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        coins.value = 0;
        life.value = 0;
    }

    private void Update()
    {
        UpdateUI();
    }

    public void AddCoins(int amount = 1)
    {
        coins.value += amount;        
    }

    public void AddLife(int amount = 1)
    {
        life.value += amount;
    }

    public void UpdateUI()
    {
        uiTextCoins.text = "x " + coins.value;
        uiTextLife.text = "x " + life.value;
    }
}
