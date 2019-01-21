using System;
using System.Collections;
using System.Collections.Generic;
using DTInventory.Models;
using UnityEngine;
using UnityEngine.Events;
using DTInventory.ScriptableObjects;

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

        // Update is called once per frame        
        void Update()
        {

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
            SlotBehaviour stackableSlot = CheckHasStackableItem(item);
            if(stackableSlot){
                stackableSlot.Stack(item);
                return true;
            }

            //check empty slot for this item
            var emptySlot = FindEmptySlot();
            if (emptySlot != null)
            {
                emptySlot.SetItem(item);
            }

            return emptySlot != null;
        }

        private SlotBehaviour CheckHasStackableItem(Item item)
        {
            SlotBehaviour stackableSlot = null;
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    var slotBehaviour = SlotGrid[x][y].GetComponent<SlotBehaviour>();
                    if (slotBehaviour.HasItem && slotBehaviour.ItemId.Equals(item.Id))
                    {
                        //in here we have same item
                        //now we need to check MaxStack count on that item.                        
                        if((slotBehaviour.Item.Quantity + item.Quantity) <= item.MaxStack){
                            return slotBehaviour;
                        }
                    }
                }
            }

            return stackableSlot;
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
                if (emptySlot != null)
                {
                    break;
                }
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    if (emptySlot != null)
                    {
                        break;
                    }
                    var slotBehaviour = SlotGrid[x][y].GetComponent<SlotBehaviour>();
                    if (!slotBehaviour.HasItem)
                    {
                        emptySlot = slotBehaviour;
                    }
                }
            }

            return emptySlot;
        }
    }
}