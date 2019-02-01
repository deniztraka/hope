using System;
using System.Collections;
using System.Collections.Generic;
using DTInventory.Models;
using UnityEngine;
using UnityEngine.Events;
using DTInventory.ScriptableObjects;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DTInventory.UI;
using DTCrafting.MonoBehaviours;

namespace DTInventory.MonoBehaviours
{
    public class InventoryBehavior : MonoBehaviour
    {
        [SerializeField]
        private int sizeX;
        [SerializeField]
        private int sizeY;

        private bool isInitialized;

        public Scrollbar InventoryScrollbar;

        public bool IsOpened;

        public GameObject[][] SlotGrid;

        public InventoryDataModel InventoryDataModel;

        public ItemDatabase ItemDatabase;

        public GameObject SlotPrefab;

        public Transform SlotsWrapper;

        public Button DropButton;

        public Button UnstackButton;

        public Button UseButton;

        public delegate void InventorySizeChangedEvent();
        public event InventorySizeChangedEvent OnInventorySizeChanged;

        internal void LoadInventory()
        {

            var playerObj = GameObject.FindGameObjectWithTag("Player");
            var player = playerObj.GetComponent<Player>();

            InventoryDataModel = player.PlayerDataModel.InventoryDataModel;

            if (InventoryDataModel == null)
            {
                return;
            }

            foreach (var item in InventoryDataModel.Items)
            {
                Add(item, false);
            }
        }

        internal void LoadInventory(InventoryDataModel inventoryDataModel)
        {
            InventoryDataModel = inventoryDataModel;
            LoadInventory();
        }

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
            Init();

            gameObject.transform.localScale = new Vector3(0, 0, 0);

            if (DropButton != null)
            {
                DropButton.interactable = false;
            }
            if (UnstackButton != null)
            {
                UnstackButton.interactable = false;
            }
            UseButton.interactable = false;

            LoadInventory();
        }

        public virtual void Init()
        {
            if (isInitialized)
            {
                return;
            }



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

            isInitialized = true;
        }

        internal void SetSelectedItemAmount(int newAmount)
        {
            var selectedSlot = GetSelectedSlot();
            if (selectedSlot != null)
            {
                selectedSlot.SetItemAmount(newAmount);
            }

            UpdateSlots();
        }
        public void UpdateDataModel()
        {
            InventoryDataModel.Items = new List<Item>();
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    var currentSlotBehaviour = SlotGrid[x][y].GetComponent<SlotBehaviour>();
                    if (currentSlotBehaviour.HasItem)
                    {
                        Item item = currentSlotBehaviour.GetItem();
                        if (item != null)
                        {
                            InventoryDataModel.Items.Add(item);
                        }
                    }
                }
            }
        }

        internal void InventorySizeChanged()
        {
            var wrapperContentRectTransform = SlotsWrapper.GetComponent<RectTransform>();

            //wrapperContentRectTransform.sizeDelta = new Vector2(wrapperContentRectTransform.sizeDelta.x, ((SizeX * SizeY / SizeX) * 41)+60);
            wrapperContentRectTransform.sizeDelta = new Vector2(wrapperContentRectTransform.sizeDelta.x, SizeY * 44);
        }

        internal void UnselectSlotExcept(SlotBehaviour slotBehaviour)
        {
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    var currentSlotBehaviour = SlotGrid[x][y].GetComponent<SlotBehaviour>();
                    if (currentSlotBehaviour != slotBehaviour)
                    {
                        currentSlotBehaviour.SetSelected(false);
                    }
                }
            }
        }

        public void UseSelected()
        {
            var selectedSlot = GetSelectedSlot();
            if (selectedSlot != null)
            {
                var isUsed = selectedSlot.UseItem();
                if (!selectedSlot.HasItem)
                {
                    UseButton.interactable = false;
                }
            }
        }

        public virtual void DisableButtons()
        {
            UseButton.interactable = false;
            if (DropButton != null)
            {
                DropButton.interactable = false;
            }
            if (UnstackButton != null)
            {
                UnstackButton.interactable = false;
            }
        }

        public void DropSelected()
        {
            var selectedSlot = GetSelectedSlot();
            if (selectedSlot != null)
            {
                selectedSlot.DropItem();
                //UpdateSlots();
                selectedSlot.SetSelected(false);
                DisableButtons();
            }
        }

        public void UnstackSelected()
        {
            var emptySlot = FindEmptySlot();
            if (emptySlot == null)
            {
                return;
            }

            var selectedSlot = GetSelectedSlot();
            if (selectedSlot == null)
            {
                return;
            }

            var item = selectedSlot.GetItem();
            if (item == null || item.Quantity <= 1)
            {
                return;
            }

            var results = Resources.FindObjectsOfTypeAll<UnstackCanvasPopUp>();
            var unstackCanvas = results[0].gameObject;
            unstackCanvas.SetActive(true);
            var unstackCanvasPopup = unstackCanvas.GetComponent<UnstackCanvasPopUp>();
            unstackCanvasPopup.Init(item, this);
        }

        private SlotBehaviour GetSelectedSlot()
        {
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    var slotBehaviour = SlotGrid[x][y].GetComponent<SlotBehaviour>();
                    if (slotBehaviour.IsSelected)
                    {
                        return slotBehaviour;
                    }
                }
            }

            return null;
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
                if (InventoryScrollbar != null)
                {
                    InventoryScrollbar.value = 1;
                }
                //gameObject.SetActive(true);
            }

            IsOpened = !IsOpened;
        }

        public void SetSize(int x, int y)
        {
            SizeX = x;
            SizeY = y;
        }

        public bool Add(Item item, bool tryStacking = true)
        {
            if (tryStacking)
            {
                //Check for any available stack for this item
                SlotItemBehaviour stackableSlotItem = CheckHasStackableItem(item);
                if (stackableSlotItem)
                {
                    stackableSlotItem.Stack(item);
                    return true;
                }
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
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
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

        internal virtual void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
        {
            //Debug.Log("updating slots:"+gameObject.name +" evenetName:"+desc.triggerType.ToString());
            UpdateSlots();
            DisableButtons();

            if (desc.triggerType == DragAndDropCell.TriggerType.DropEventEnd && gameObject.name == "PlayerInventory")
            {
                var craftMenuObj = GameObject.Find("CraftMenu");
                var craftingTable = craftMenuObj.GetComponent<CraftingTable>();
                craftingTable.UpdateUI();
            }

            return;
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

        internal void UpdateSlots()
        {
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    //Debug.Log("updating: "+x + "," + y);
                    UpdateSlot(SlotGrid[x][y]);
                }
            }
        }

        private void UpdateSlot(GameObject slotGameObject)
        {
            //Updating HasItem property
            var slotBehaviour = slotGameObject.GetComponent<SlotBehaviour>();

            var slotItem = slotGameObject.transform.GetComponentInChildren<SlotItemBehaviour>();
            //Debug.Log("hasItem:"+(slotItem != null).ToString());
            slotBehaviour.HasItem = slotItem != null;

            //Updating slot item icon
            // var slotItemImage = slotItem.gameObject.GetComponent<Image>();
            // slotItemImage.sprite = slotItem.Item.Icon;

            //Updating Selection of slot
            slotBehaviour.SetSelected(false);
        }

        public List<Item> GetItemsEach()
        {
            var items = new List<Item>();
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    var slot = SlotGrid[x][y].GetComponent<SlotBehaviour>();
                    if (slot.HasItem)
                    {
                        var item = slot.GetItem();
                        if (item != null)
                        {
                            for (int i = 0; i < item.Quantity; i++)
                            {
                                var copiedItem = item.getCopy<Item>();
                                copiedItem.Quantity = 1;

                                items.Add(copiedItem);
                            }
                        }
                        else
                        {
                            slot.HasItem = false;
                        }

                    }
                }
            }

            return items;
        }
    }
}