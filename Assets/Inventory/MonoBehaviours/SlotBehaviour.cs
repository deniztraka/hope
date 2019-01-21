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

        public bool IsSelected;

        public Sprite SelectedSprite;
        public Sprite Sprite;

        public bool HasItem;

        public Item Item;
        private ItemDatabase itemDatabase;


        // Start is called before the first frame update
        void Start()
        {
            if (String.IsNullOrEmpty(ItemId))
            {
                return;
            }

            SetItem(ItemId);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ToggleSelect()
        {
            if (!HasItem)
            {
                return;
            }

            var slotWrapperPanelImage = transform.Find("SlotWrapperCanvas").Find("SlotWrapperPanel").GetComponent<Image>();
            if (IsSelected)
            {
                slotWrapperPanelImage.sprite = Sprite;
            }
            else
            {
                slotWrapperPanelImage.sprite = SelectedSprite;
            }
            IsSelected = !IsSelected;
        }

        private bool SetItem(string itemId)
        {
            ItemId = itemId;
            var itemDatabase = gameObject.GetComponentInParent<InventoryBehavior>().ItemDatabase;
            if (itemDatabase == null)
            {
                return false;
            }

            Item = itemDatabase.getItemByID(ItemId);
            if (Item == null)
            {
                return false;
            }

            return true;
        }

        internal void SetItem(Item item)
        {
            if (SetItem(item.Id))
            {
                var itemImage = ItemTexturePanel.GetComponent<Image>(); ;
                itemImage.sprite = item.Icon;
                itemImage.color = Color.white;

                var itemQuantityText = ItemQuantityPanel.GetComponent<Text>(); ;
                itemQuantityText.text = item.Quantity.ToString();
                HasItem = true;
            }
        }

        internal void Stack(Item item)
        {
            Item.Quantity += item.Quantity;
            var itemQuantityText = ItemQuantityPanel.GetComponent<Text>(); ;
            itemQuantityText.text = Item.Quantity.ToString();
        }
    }
}
