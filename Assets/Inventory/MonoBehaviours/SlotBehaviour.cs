using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTInventory.Models;
using DTInventory.ScriptableObjects;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DTInventory.MonoBehaviours
{
    public class SlotBehaviour : MonoBehaviour
    {
        public bool IsSelected;

        public Sprite SelectedSprite;
        public Sprite Sprite;

        public bool HasItem;
        public GameObject SlotItemPrefab;        
        private ItemDatabase itemDatabase;

        private InventoryBehavior InventoryBehavior;


        // Start is called before the first frame update
        void Start()
        {
            InventoryBehavior = gameObject.GetComponentInParent<InventoryBehavior>();

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

             var slotItem = transform.GetComponentInChildren<SlotItemBehaviour>();

            IsSelected = !IsSelected;
            SetSelected(IsSelected);
            InventoryBehavior.DropButton.interactable = IsSelected;

            if (IsSelected && slotItem.Item.Quantity > 1)
            {
                InventoryBehavior.UnstackButton.interactable = IsSelected;
            }else {
                InventoryBehavior.UnstackButton.interactable = false;
            }
        }

        private void SetItem(Item item, GameObject slotItem)
        {
            var slotItemBehaviour = slotItem.GetComponent<SlotItemBehaviour>();
            slotItemBehaviour.Item = item;
            slotItemBehaviour.SetUI();
        }

        internal GameObject AddItem(Item item)
        {
            if (item != null)
            {
                var itemDatabase = InventoryBehavior.ItemDatabase;
                var slotWrapperPanel = transform.Find("SlotWrapperCanvas").Find("SlotWrapperPanel");
                var slotItem = Instantiate(SlotItemPrefab, new Vector3(0, 0, 0), Quaternion.identity, slotWrapperPanel);
                SetItem(item, slotItem);
                HasItem = true;
                return slotItem;
            }
            return null;
        }

        internal void SetSelected(bool select)
        {
            var slotWrapperPanelImage = transform.Find("SlotWrapperCanvas").Find("SlotWrapperPanel").GetComponent<Image>();
            if (!select)
            {
                slotWrapperPanelImage.sprite = Sprite;
            }
            else
            {
                slotWrapperPanelImage.sprite = SelectedSprite;
                InventoryBehavior.UnselectSlotExcept(this);
            }
            IsSelected = select;
        }
    }
}
