using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Itens;

public class ActionLifePack : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.L;
    private SO_int so_Int;
    public TextMeshProUGUI noLifePackText;

    private void Start()
    {
        so_Int = ItemManager.Instance.GetItemByType(ItemType.LIFE_PACK).so_Int;
    }

    private void RecoverLife()
    {
        if(so_Int.value > 0)
        {
            ItemManager.Instance.RemoveByType(ItemType.LIFE_PACK);

            Player.Instance.healthBase.ResetLife();
        }
        else
        {
            noLifePackText.gameObject.SetActive(true);
            Invoke(nameof(TurnOffLifePackText), 3);
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            RecoverLife();
        }
    }

    void TurnOffLifePackText()
    {
        noLifePackText.gameObject.SetActive(false);
    }
}
