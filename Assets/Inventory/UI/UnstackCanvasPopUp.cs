using System;
using System.Collections;
using System.Collections.Generic;
using DTInventory.Models;
using DTInventory.MonoBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace DTInventory.UI
{
    public class UnstackCanvasPopUp : MonoBehaviour
    {
        public Item ItemToUnstack;
        public InventoryBehavior InventoryBehaviour;

        public Button UnstackButton;
        public Text AmountText;

        public float SelectedAmount;

        private Scrollbar scrollBar;

        // Start is called before the first frame update
        void Start()
        {
            //setminimumamount to 1
            //set maximum amount to ItemToUnstack.Quantity



        }

        public void OnScrollbarValueChanged(float value)
        {
            SelectedAmount = (float)Math.Floor(ItemToUnstack.Quantity * value);
            if (SelectedAmount <= 1)
            {
                SelectedAmount = 1;
            }
            else if (SelectedAmount >= ItemToUnstack.Quantity)
            {
                SelectedAmount = ItemToUnstack.Quantity - 1;
            }
            AmountText.text = SelectedAmount.ToString();
        }
        public void Unstack()
        {
            var initialAmount = ItemToUnstack.Quantity;
            var amountLeft = initialAmount - (int)SelectedAmount;

            //prepare and add new item            
            var newItemToAdd = InventoryBehaviour.ItemDatabase.getItemByID(ItemToUnstack.Id);
            newItemToAdd.Quantity = (int)SelectedAmount;

            InventoryBehaviour.SetSelectedItemAmount(amountLeft);

            InventoryBehaviour.Add(newItemToAdd, false);

            InventoryBehaviour.DisableButtons();
            
            Cancel();
        }

        public void Cancel()
        {
            ItemToUnstack = null;
            InventoryBehaviour = null;
            scrollBar.onValueChanged.RemoveListener(OnScrollbarValueChanged);
            gameObject.SetActive(false);
        }

        internal void Init(Item item, InventoryBehavior inventoryBehavior)
        {
            ItemToUnstack = item;
            InventoryBehaviour = inventoryBehavior;
            scrollBar = gameObject.GetComponentInChildren<Scrollbar>();
            scrollBar.onValueChanged.AddListener(OnScrollbarValueChanged);
            scrollBar.value = 0.5f;
            SelectedAmount = (float)Math.Floor(ItemToUnstack.Quantity * scrollBar.value);
            scrollBar.onValueChanged.Invoke(0.5f);
        }
    }

}
