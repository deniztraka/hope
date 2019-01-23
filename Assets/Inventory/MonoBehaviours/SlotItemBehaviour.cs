using System;
using System.Collections;
using System.Collections.Generic;
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

        internal void SetUI()
        {
            var itemQuantityText = ItemQuantityPanel.GetComponent<Text>();
            itemQuantityText.text = Item.Quantity.ToString();

            var itemImage = ItemTexturePanel.GetComponent<Image>();
            itemImage.sprite = Item.Icon;
            var tempColor = itemImage.color;
            tempColor.a = 1f;
            itemImage.color = tempColor;

            var rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            rectTransform.anchoredPosition = new Vector2(0f, 0f);
        }

        internal void DropItem()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            var istantiatedGameObject = Instantiate(Item.GameObject, new Vector3(UnityEngine.Random.Range(player.transform.position.x - 1, player.transform.position.x + 1), player.transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
            var itemBehaviour = istantiatedGameObject.GetComponent<ItemBehaviour>();
            itemBehaviour.SetItemAmount(Item.Quantity);
            Destroy(gameObject);
        }
    }
}