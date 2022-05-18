using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayLevel : MonoBehaviour
{
    public TextMeshProUGUI uiTextName;

    private void Start()
    {
        SaveManager.Instance.FileLoaded += OnLoad;
    }

    public void OnLoad(SaveSetup setup)
    {
        uiTextName.text = "Play " + (setup.lastLevel + 1); //para mostrar o próximo level doq foi salvo (salvou o level 1, aparecerá no botao play level 2
                                                           //precisa estar entre parenteses pois senão ele le como arquivo de texto e não valor, resultando em "Play 11"
                                                           //e queremos "Play 1"
    }

    private void OnDestroy()
    {
        SaveManager.Instance.FileLoaded -= OnLoad;
    }
}
