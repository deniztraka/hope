using System;
using System.Collections;
using System.Collections.Generic;
using DTInventory.Models;
using UnityEngine;
using UnityEngine.Events;
using DTInventory.ScriptableObjects;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTInventory.MonoBehaviours
{
    public class InventoryBehavior : MonoBehaviour
    {
        [SerializeField]
        private int sizeX;
        [SerializeField]
        private int sizeY;


        public bool IsOpened;

        public GameObject[][] SlotGrid;

        public Inventory Inventory;

        public ItemDatabase ItemDatabase;

        public GameObject SlotPrefab;

        public Transform SlotsWrapper;

        public delegate void InventorySizeChangedEvent();
        public event InventorySizeChangedEvent OnInventorySizeChanged;

        public int SizeX
        {
            get
            {
                return sizeX;
            }

            set
            {
                sizeX = value;
                if (value != sizeX)
                {
                    OnInventorySizeChanged();
                }
            }
        }

        public int SizeY
        {
            get
            {
                return sizeY;
            }

            set
            {
                sizeY = value;
                if (value != sizeY)
                {
                    OnInventorySizeChanged();
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Inventory = new Inventory();
            SlotGrid = new GameObject[SizeX][];

            OnInventorySizeChanged += InventorySizeChanged;
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                if (SlotGrid[x] == null)
                {
                    SlotGrid[x] = new GameObject[SizeY];
                }
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    SlotGrid[x][y] = Instantiate(SlotPrefab, Vector3.zero, Quaternion.identity, SlotsWrapper);
                }
            }
            InventorySizeChanged();

            gameObject.transform.localScale = new Vector3(0, 0, 0);


        }

        private void InventorySizeChanged()
        {
            var wrapperContentRectTransform = SlotsWrapper.GetComponent<RectTransform>();

            wrapperContentRectTransform.sizeDelta = new Vector2(wrapperContentRectTransform.sizeDelta.x, (SizeX * SizeY / 4) * 70);
        }

        public void ToggleActive()
        {
            if (IsOpened)
            {
                gameObject.transform.localScale = new Vector3(0, 0, 0);
                //gameObject.SetActive(false);

            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                //gameObject.SetActive(true);
            }

            IsOpened = !IsOpened;
        }

        public void SetSize(int x, int y)
        {
            SizeX = x;
            SizeY = y;
        }

        public bool Add(Item item)
        {
            //Check for any available stack for this item
            SlotItemBehaviour stackableSlotItem = CheckHasStackableItem(item);
            if (stackableSlotItem)
            {
                stackableSlotItem.Stack(item);
                return true;
            }

            //check empty slot for this item
            var emptySlot = FindEmptySlot();
            if (emptySlot == null)
            {
                return false;
            }

            var addetSlotItem = emptySlot.AddItem(item);

            var desc = new DragAndDropCell.DropEventDescriptor();
            // Fill event descriptor
            desc.triggerType = DragAndDropCell.TriggerType.ItemAdded;
            desc.item = addetSlotItem.GetComponent<DragAndDropItem>();
            desc.sourceCell = addetSlotItem.GetComponentInParent<DragAndDropCell>();
            desc.destinationCell = addetSlotItem.GetComponentInParent<DragAndDropCell>(); ;
            SendNotification(desc);

            return true;
        }

        private SlotItemBehaviour CheckHasStackableItem(Item item)
        {
            SlotItemBehaviour stackableSlotItemBehaviour = null;
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    var slotBehaviour = SlotGrid[x][y].GetComponent<SlotBehaviour>();
                    if (slotBehaviour.HasItem)
                    {
                        var slotItemBehaviour = SlotGrid[x][y].GetComponentInChildren<SlotItemBehaviour>();
                        if (slotItemBehaviour.Item.Id.Equals(item.Id) && (slotItemBehaviour.Item.Quantity + item.Quantity) <= item.MaxStack)
                        {
                            return slotItemBehaviour;
                        }
                    }
                    // if (slotBehaviour.HasItem && slotBehaviour.ItemId.Equals(item.Id))
                    // {
                    //     //in here we have same item
                    //     //now we need to check MaxStack count on that item.                        
                    //     if((slotBehaviour.Item.Quantity + item.Quantity) <= item.MaxStack){
                    //         return slotBehaviour;
                    //     }
                    // }
                }
            }

            return stackableSlotItemBehaviour;
        }

        private bool HasEmptySlot()
        {
            var hasEmptySlot = false;
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    var slotBehaviour = SlotGrid[x][y].GetComponent<SlotBehaviour>();
                    if (!slotBehaviour.HasItem)
                    {
                        hasEmptySlot = slotBehaviour.HasItem;
                    }
                }
            }

            return hasEmptySlot;
        }

        private SlotBehaviour FindEmptySlot()
        {
            SlotBehaviour emptySlot = null;
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                // if (emptySlot != null)
                // {
                //     break;
                // }
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    // if (emptySlot != null)
                    // {
                    //     break;
                    // }
                    var slotBehaviour = SlotGrid[x][y].GetComponent<SlotBehaviour>();
                    if (!slotBehaviour.HasItem)
                    {
                        return slotBehaviour;

                    }
                }
            }

            return emptySlot;
        }


        /// <summary>
        /// Send drag and drop information to application
        /// </summary>
        /// <param name="desc"> drag and drop event descriptor </param>
        private void SendNotification(DragAndDropCell.DropEventDescriptor desc)
        {
            if (desc != null)
            {
                // Send message with DragAndDrop info to parents GameObjects
                gameObject.SendMessageUpwards("OnSimpleDragAndDropEvent", desc, SendMessageOptions.DontRequireReceiver);
            }
        }

        void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
        {
            // Get control unit of source cell
            var sourceSheet = desc.sourceCell.GetComponentInParent<InventoryBehavior>();
            // Get control unit of destination cell
            var destinationSheet = desc.destinationCell.GetComponentInParent<InventoryBehavior>();
            var actualItem = desc.item.GetComponent<SlotItemBehaviour>();
            var icon = desc.item.GetComponent<Image>();
            icon.sprite = actualItem.Item.Icon;


            switch (desc.triggerType)                                               // What type event is?
            {
                case DragAndDropCell.TriggerType.DropRequest:                       // Request for item drag (note: do not destroy item on request)
                    Debug.Log("Request " + actualItem.Item.Name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                    break;
                case DragAndDropCell.TriggerType.DropEventEnd:                      // Drop event completed (successful or not)
                    if (desc.permission == true)                                    // If drop successful (was permitted before)
                    {
                        Debug.Log("Successful drop " + actualItem.Item.Name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                        UpdateSlots();
                    }
                    else                                                            // If drop unsuccessful (was denied before)
                    {
                        Debug.Log("Denied drop " + actualItem.Item.Name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                    }
                    break;
                case DragAndDropCell.TriggerType.ItemAdded:                         // New item is added from application
                    Debug.Log("Item " + actualItem.Item.Name + " added into " + destinationSheet.name);
                    break;
                case DragAndDropCell.TriggerType.ItemWillBeDestroyed:               // Called before item be destructed (can not be canceled)
                    Debug.Log("Item " + actualItem.Item.Name + " will be destroyed from " + sourceSheet.name);
                    break;
                default:
                    Debug.Log("Unknown drag and drop event");
                    break;
            }
        }

        private void UpdateSlots()
        {
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    UpdateSlot(SlotGrid[x][y]);
                }
            }
        }

        private void UpdateSlot(GameObject slotGameObject)
        {
            //Updating HasItem property
            var slotBehaviour = slotGameObject.GetComponent<SlotBehaviour>();
            var slotItem = slotGameObject.transform.GetComponentInChildren<SlotItemBehaviour>();
            slotBehaviour.HasItem = slotItem != null;
        }
    }
}