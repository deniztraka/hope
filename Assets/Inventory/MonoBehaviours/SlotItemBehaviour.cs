﻿using System;
using System.Collections;
using System.Collections.Generic;
using DTComponents;
using DTInventory.Models;
using UnityEngine;
using UnityEngine.UI;

namespace DTInventory.MonoBehaviours
{
    public class SlotItemBehaviour : MonoBehaviour
    {
        public Item Item;

        public GameObject ItemQuantityPanel;
        public GameObject ItemTexturePanel;

        internal void Stack(Item item)
        {
            Item.Quantity += item.Quantity;
            var itemQuantityText = ItemQuantityPanel.GetComponent<Text>();
            itemQuantityText.text = Item.Quantity.ToString();
        }

        internal void SetUI(Sprite icon)
        {
            SetItemAmount(Item.Quantity);

            var itemImage = ItemTexturePanel.GetComponent<Image>();

            itemImage.sprite = icon;
            var tempColor = itemImage.color;
            tempColor.a = 1f;
            itemImage.color = tempColor;

            var rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            rectTransform.anchoredPosition = new Vector2(0f, 0f);

            var addetSlotBehaviour = gameObject.GetComponentInParent<SlotItemBehaviour>();
            var slotItemImg = addetSlotBehaviour.gameObject.GetComponent<Image>();
            slotItemImg.sprite = icon;
        }

        internal void DropItem()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            var istantiatedGameObject = Instantiate(Item.GameObject, new Vector3(UnityEngine.Random.Range(player.transform.position.x - 1, player.transform.position.x + 1), player.transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
            var itemBehaviour = istantiatedGameObject.GetComponent<ItemBehaviour>();
            itemBehaviour.SetItemAmount(Item.Quantity);
            Destroy(gameObject);
        }

        internal bool UseItem()
        {
            var used = false;

            var itemBehaviour = Item.GameObject.GetComponent<ItemBehaviour>();                       

            if (Item.Quantity > 0 )
            {
                itemBehaviour.Use();
                Item.Quantity--;
                SetItemAmount(Item.Quantity);
                used = true;
            }            

            return used;
        }

        internal void SetItemAmount(int newAmount)
        {
            var itemQuantityText = ItemQuantityPanel.GetComponent<Text>();
            itemQuantityText.text = newAmount.ToString();
            Item.Quantity = newAmount;
        }
    }
}