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
        public Item Item;
        private ItemDatabase itemDatabase;


        // Start is called before the first frame update
        void Start()
        {


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
                var itemDatabase = gameObject.GetComponentInParent<InventoryBehavior>().ItemDatabase;                
                var slotWrapperPanel = transform.Find("SlotWrapperCanvas").Find("SlotWrapperPanel");
                var slotItem = Instantiate(SlotItemPrefab, new Vector3(0,0,0), Quaternion.identity,slotWrapperPanel);                
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
            }
            IsSelected = select;
        }
    }
}
