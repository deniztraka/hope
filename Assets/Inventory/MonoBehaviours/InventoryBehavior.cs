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
        private int sizeX;
        private int sizeY;


        public Slot[,] SlotGrid;

        public Inventory Inventory;

        public ItemDatabase ItemDatabase;

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
            SlotGrid = new Slot[SizeX, SizeY];
            OnInventorySizeChanged += InventorySizeChanged;

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void InventorySizeChanged()
        {
            Debug.Log("changed");
        }
    }
}