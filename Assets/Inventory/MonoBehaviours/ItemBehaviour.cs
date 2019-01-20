using System;
using System.Collections;
using System.Collections.Generic;
using DTInventory.Models;
using DTInventory.ScriptableObjects;
using Unity.Collections;
using UnityEngine;

namespace DTInventory.MonoBehaviours
{
    public class ItemBehaviour : MonoBehaviour
    {
        public string Id;
        public ItemDatabase ItemDatabase;

        public Item Item;

        void Start()
        {
            if (ItemDatabase == null)
            {
                Debug.Log(String.Format("ItemDatabase is null for this object:{0}", gameObject.name));
                return;
            }

            Item = ItemDatabase.getItemByID(Id);
        }
    }
}