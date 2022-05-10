using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using TMPro;


namespace Itens
{
    public enum ItemType
    {
        COIN,
        LIFE_PACK
    }

    public class ItemManager : Singleton<ItemManager>
    {
        public List<ItemSetup> itemSetups;

        private void Start()
        {
            Reset();
            LoadItemsFromSave();
        }

        private void LoadItemsFromSave()
        {
            AddByType(ItemType.COIN, (int)SaveManager.Instance.Setup.coins);
            AddByType(ItemType.LIFE_PACK, (int)SaveManager.Instance.Setup.lifePack);
        }

        private void Reset()
        {
            foreach(var i in itemSetups)
            {
                i.so_Int.value = 0;
            }
        }

        public ItemSetup GetItemByType(ItemType itemType)
        {
            return itemSetups.Find(i => i.itemType == itemType);
        }

        public void AddByType(ItemType itemType, int amount = 1)
        {
            if (amount < 0) return;
            var item = itemSetups.Find(i => i.itemType == itemType);
            item.so_Int.value += amount;
        }

        public void RemoveByType(ItemType itemType, int amount = 1)
        {
            var item = itemSetups.Find(i => i.itemType == itemType);
            item.so_Int.value -= amount;

            if (item.so_Int.value < 0) item.so_Int.value = 0;
        }

        #region DEBUG
        [NaughtyAttributes.Button]
        private void AddCoin()
        {
            AddByType(ItemType.COIN);
        }
        
        [NaughtyAttributes.Button]
        private void AddLifePack()
        {
            AddByType(ItemType.LIFE_PACK);
        }
        #endregion
    }

    [System.Serializable]
    public class ItemSetup
    {
        public ItemType itemType;
        public SO_int so_Int;
        public Sprite icon;
    }

}
