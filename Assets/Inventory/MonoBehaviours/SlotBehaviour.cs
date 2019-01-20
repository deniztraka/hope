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

        private Item item;
        private ItemDatabase itemDatabase;
        private bool hasItem;

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
            hasItem = true;


        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ToggleSelect()
        {
            if(!hasItem){
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
    }
}
