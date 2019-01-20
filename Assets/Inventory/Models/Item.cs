using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTInventory.Models
{

    [Serializable]
    public class Item : ItemBase
    {
        public GameObject GameObject;
        public float Weight;
        public int MaxStack;
        public int Quantity;
        public Sprite Icon;
        
        public Item(string name, string description, ItemType type, GameObject gameObject, Sprite icon, float weight = 1f, int maxStack = 1) : base(name, description, type)
        {
            Icon = icon;
            GameObject = gameObject;
            Weight = weight;
            MaxStack = maxStack;
        }
        public Item() : base()
        {            
        }
    }
}
