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


        public Slot[,] SlotGrid;

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
            SlotGrid = new Slot[SizeX, SizeY];
            OnInventorySizeChanged += InventorySizeChanged;
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                var slot = Instantiate(SlotPrefab, Vector3.zero, Quaternion.identity, SlotsWrapper);
                // var slotRectTransform = slot.GetComponent<RectTransform>();
                // slotRectTransform.position.Set(0,0,0);
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void InventorySizeChanged()
        {
            Debug.Log("changed");
        }

        public void ToggleActive()
        {
            if (gameObject.active)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        public void SetSize(int x, int y)
        {
            SizeX = x;
            SizeY = y;
        }
    }
}