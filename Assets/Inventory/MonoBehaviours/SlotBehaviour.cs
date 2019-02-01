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

        public bool IsSelectable;

        public Sprite SelectedSprite;
        public Sprite Sprite;

        public bool HasItem;
        public GameObject SlotItemPrefab;
        private ItemDatabase itemDatabase;

        private InventoryBehavior InventoryBehavior;


        public delegate void SlotEventHandler();
        public event SlotEventHandler OnItemAdded;
        public event SlotEventHandler OnItemRemoved;


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
            if (!IsSelectable)
            {
                return;
            }

            if (!HasItem)
            {
                return;
            }

            var slotItem = transform.GetComponentInChildren<SlotItemBehaviour>();

            IsSelected = !IsSelected;
            SetSelected(IsSelected);
            if (InventoryBehavior.DropButton != null)
            {
                InventoryBehavior.DropButton.interactable = IsSelected;
            }


            if (InventoryBehavior.DropButton != null)
            {
                InventoryBehavior.DropButton.interactable = IsSelected;
            }

            if (IsSelected && slotItem.Item.Quantity > 1)
            {
                if (InventoryBehavior.UnstackButton != null)
                {
                    InventoryBehavior.UnstackButton.interactable = IsSelected;
                }

            }
            else
            {
                if (InventoryBehavior.UnstackButton != null)
                {
                    InventoryBehavior.UnstackButton.interactable = false;
                }

            }

            InventoryBehavior.UseButton.interactable = slotItem.Item.Type == ItemType.Consumable && IsSelected;

        }

        private void SetItem(Item item, GameObject slotItem)
        {
            var slotItemBehaviour = slotItem.GetComponent<SlotItemBehaviour>();
            slotItemBehaviour.Item = item;

            var dbItem = InventoryBehavior.ItemDatabase.getItemByID(item.Id);

            slotItemBehaviour.SetUI(dbItem.Icon);



        }

        public void DropItem()
        {
            var slotItemBehaviour = transform.GetComponentInChildren<SlotItemBehaviour>();
            slotItemBehaviour.DropItem();
            HasItem = false;
            ToggleSelect();
        }

        internal GameObject AddItem(Item item)
        {
            if (item != null)
            {
                if (InventoryBehavior == null)
                {
                    InventoryBehavior = gameObject.GetComponentInParent<InventoryBehavior>();
                }
                var itemDatabase = InventoryBehavior.ItemDatabase;
                var slotWrapperPanel = transform.Find("SlotWrapperCanvas").Find("SlotWrapperPanel");
                var slotItem = Instantiate(SlotItemPrefab, new Vector3(0, 0, 0), Quaternion.identity, slotWrapperPanel);
                SetItem(item, slotItem);
                HasItem = true;

                if(OnItemAdded!=null){
                    OnItemAdded();
                }
                return slotItem;
            }
            return null;
        }

        internal void SetSelected(bool select)
        {
            if (!IsSelectable)
            {
                return;
            }

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

        internal Item GetItem()
        {
            var slotItemBehaviour = transform.GetComponentInChildren<SlotItemBehaviour>();
            if (slotItemBehaviour != null)
            {
                return slotItemBehaviour.Item;
            }

            return null;
        }

        internal void SetItemAmount(int newAmount)
        {
            var slotItemBehaviour = transform.GetComponentInChildren<SlotItemBehaviour>();
            slotItemBehaviour.SetItemAmount(newAmount);
        }

        internal void RemoveItem()
        {
            HasItem = false;
            var slotItemBehaviour = transform.GetComponentInChildren<SlotItemBehaviour>();
            Destroy(slotItemBehaviour.gameObject);
            SetSelected(false);
            if(OnItemRemoved != null){
                OnItemRemoved();
            }
        }

        internal bool UseItem()
        {
            var slotItemBehaviour = transform.GetComponentInChildren<SlotItemBehaviour>();
            var isUsed = slotItemBehaviour.UseItem();
            if (isUsed)
            {
                var itemUsed = GetItem();
                if (itemUsed.Quantity <= 0)
                {
                    RemoveItem();
                }
            }
            return isUsed;
        }
    }
}
