using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTInventory.MonoBehaviours;
using DTCrafting.ScriptableObjects;
using System;
using UnityEngine.UI;

namespace DTCrafting.MonoBehaviours
{
    public class CraftingTable : InventoryBehavior
    {
        public RecipeDatabase RecipeDatabase;
        public InventoryBehavior InventoryBehavior;

        public SlotBehaviour OutputSlot;

        [SerializeField]
        private Text OutputItemTitleText;
        [SerializeField]
        private Text OutputItemDescText;

        // Start is called before the first frame update
        void Start()
        {
            Init();

            //gameObject.transform.localScale = new Vector3(0, 0, 0);            
        }

        public void Init()
        {
            SlotGrid = new GameObject[SizeX][];
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                if (SlotGrid[x] == null)
                {
                    SlotGrid[x] = new GameObject[SizeY];
                }
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    var slotObj = Instantiate(SlotPrefab, Vector3.zero, Quaternion.identity, SlotsWrapper);
                    var slot = slotObj.GetComponent<SlotBehaviour>();
                    slot.IsSelectable = false;
                    SlotGrid[x][y] = slot.gameObject;
                }
            }
        }

        internal override void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
        {
            base.OnSimpleDragAndDropEvent(desc);

            if (desc != null && desc.triggerType == DragAndDropCell.TriggerType.DropEventEnd && desc.sourceCell != null)
            {
                var inventoryBehaviour = desc.sourceCell.GetComponentInParent<InventoryBehavior>();
                inventoryBehaviour.UpdateSlots();
                inventoryBehaviour.DisableButtons();
            }

            if (desc.triggerType == DragAndDropCell.TriggerType.DropEventEnd)
            {
                Debug.Log("check recipe database if crafting table has item.");
                var recipe = TryCraft();
                SetCraftUI(recipe);
            }
        }

        private void SetCraftUI(CraftingRecipe recipe)
        {
            if (recipe != null)
            {
                var itemToCraft = ItemDatabase.getItemByID(recipe.OutputItem.Id);
                itemToCraft.Quantity = 1;

                var itemAlreadyThere = OutputSlot.GetItem();
                if (itemAlreadyThere == null || !itemAlreadyThere.Id.Equals(itemToCraft.Id))
                {
                    OutputSlot.AddItem(itemToCraft);
                    var dragDropItemComponent = OutputSlot.gameObject.GetComponentInChildren<DragAndDropItem>();
                    dragDropItemComponent.enabled = false;
                    OutputItemTitleText.text = itemToCraft.Name;
                    OutputItemDescText.text = itemToCraft.Description;
                    UseButton.interactable = true;
                }
            }
            else
            {
                OutputItemTitleText.text = "";
                OutputItemDescText.text = "";
                if (OutputSlot.HasItem)
                {
                    OutputSlot.RemoveItem();
                }
                UseButton.interactable = false;
            }
        }

        private CraftingRecipe TryCraft()
        {
            var recipeList = RecipeDatabase.RecipeList;

            var itemListEach = GetItemsEach();
            if (itemListEach.Count > 0)
            {
                var canCraft = false;
                foreach (var recipe in recipeList)
                {
                    var hasAllItems = true;
                    foreach (var requiredItem in recipe.RequiredItems)
                    {
                        var items = itemListEach.FindAll(item => item.Id == requiredItem.Item.Id);
                        if (items == null || items.Count < requiredItem.Amount)
                        {
                            hasAllItems = false;
                            break;
                        }
                    }
                    if (hasAllItems)
                    {
                        return recipe;
                    }
                }
            }


            return null;
        }

        internal void UpdateUI()
        {
            var recipe = TryCraft();
            SetCraftUI(recipe);
        }

        public void CraftItem()
        {
            
        }
    }
}