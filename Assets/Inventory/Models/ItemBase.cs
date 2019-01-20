using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTInventory.Models
{
    public abstract class ItemBase
    {
        public string Id;
        public string Name;
        public string Description;
        public ItemType Type;        

        public ItemBase(string name, string description, ItemType type)
        {
            name = Name;
            description = Description;
            type = Type;
        }

        public ItemBase()
        {            
        }

        public T getCopy<T>()
        {
            return (T)this.MemberwiseClone();
        }

    }
}