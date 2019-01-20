using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTInventory.Models;
using DTInventory.ScriptableObjects;
using System;
using UnityEngine.UI;

namespace DTInventory.MonoBehaviours
{
    public class SlotBehaviour : MonoBehaviour
    {
        public string ItemId;
        public string ItemName;
        public Image ItemTexturePanel;

        public Text ItemQuantityPanel;

        private Item item;
        private ItemDatabase itemDatabase;

        // Start is called before the first frame update
        void Start()
        {
            if (String.IsNullOrEmpty(ItemId))
            {
                return;
            }

            var itemDatabase = gameObject.GetComponentInParent<InventoryBehavior>().ItemDatabase;
            if (itemDatabase == null)
            {
                return;
            }

            item = itemDatabase.getItemByID(ItemId);
            if (item == null)
            {
                return;
            }

            var itemImage = ItemTexturePanel.GetComponent<Image>(); ;
            itemImage.sprite = item.Icon;
            itemImage.color = Color.white;

            var itemQuantityText = ItemQuantityPanel.GetComponent<Text>(); ;
            itemQuantityText.text = item.Quantity.ToString();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
