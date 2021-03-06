﻿using System;
using System.Collections;
using System.Collections.Generic;
using DTComponents;
using DTInventory.Models;
using DTInventory.ScriptableObjects;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DTInventory.MonoBehaviours
{
    public class ItemBehaviour : MonoBehaviour
    {
        public ItemDatabase ItemDatabase;

        public Item Item;

        void Awake()
        {
            if (ItemDatabase == null)
            {
                Debug.Log(String.Format("ItemDatabase is null for this object:{0}", gameObject.name));
                return;
            }

            Item = ItemDatabase.getItemByID(Item.Id);
        }

        public void OnClick()
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            var player = playerObject.GetComponent<Player>();
            var pickupResult = player.PickUpItem(Item);
            if (pickupResult)
            {
                Destroy(gameObject);
            }

        }

        public void SetItemAmount(int quantity)
        {
            Item.Quantity = quantity;
        }

        public virtual void Use()
        {

            switch (Item.Type)
            {
                case ItemType.Consumable:
                    var consumableComponent = GetComponent<Consumable>();
                    consumableComponent.Consume();
                    break;
                case ItemType.Blueprint:
                    var bluePrintComponent = GetComponent<BluePrint>();
                    bluePrintComponent.OnUse();
                    break;
            }

            Debug.Log("ıtem is used");
        }
    }
}