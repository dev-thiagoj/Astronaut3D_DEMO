using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectableCoin : ItemCollectableBase
{
    protected override void OnCollect()
    {
        if (!ItemManager.Instance)
        {
            Debug.Log("ItemManager Instance Missing");
            //var ItemManager = Instantiate(Instance(ItemManager);
            return;

        }
        
        base.OnCollect();
        ItemManager.Instance.AddCoins();
    }
}
