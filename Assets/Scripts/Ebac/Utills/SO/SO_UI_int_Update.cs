using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SO_UI_int_Update : MonoBehaviour
{
    public SO_int soInt;
    public TextMeshProUGUI uiTextValue;

    private void Start()
    {
        uiTextValue.text = "x " + soInt.value.ToString();
    }

    private void Update()
    {
        uiTextValue.text = "x " + soInt.value.ToString();
    }

}
