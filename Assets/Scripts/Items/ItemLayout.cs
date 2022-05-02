using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Itens
{
    public class ItemLayout : MonoBehaviour
    {
        public Image uiIcon;
        public TextMeshProUGUI uiValue;
        
        private ItemSetup _currSetup;

        public void Load(ItemSetup setup)
        {
            _currSetup = setup;
            UpdateUI();
        }

        private void UpdateUI()
        {
            uiIcon.sprite = _currSetup.icon;
        }

        private void Update()
        {
            uiValue.text = _currSetup.so_Int.value.ToString();
        }
    }

}
