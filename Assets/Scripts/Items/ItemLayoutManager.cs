using System.Collections.Generic;
using UnityEngine;


namespace Itens
{
    public class ItemLayoutManager : MonoBehaviour
    {
        public List<ItemLayout> itemLayouts;

        public ItemLayout prefabLayout;
        public Transform container;

        private void Start()
        {
            CreateItens();
        }

        private void CreateItens()
        {
            foreach (var setup in ItemManager.Instance.itemSetups)
            {
                var item = Instantiate(prefabLayout, container);
                item.Load(setup);

                itemLayouts.Add(item);
            }
        }
    }
}
