using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Itens;

public class ActionLifePack : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.L;
    public SO_int so_Int;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            RecoverLife();
        }
    }
}
