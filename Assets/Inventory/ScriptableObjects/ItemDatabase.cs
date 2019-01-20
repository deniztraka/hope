using System.Collections;
using System.Collections.Generic;
using DTInventory.Models;
using UnityEngine;

namespace DTInventory.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItemDataBase", menuName = "Database/ItemDataBase", order = 1)]
    public class ItemDatabase : ScriptableObject
    {        
        [SerializeField]
        public List<Item> ItemList = new List<Item>();

        public Item getItemByID(string id)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].Id == id)
                    return ItemList[i].getCopy<Item>();
            }
            return null;
        }

        public Item getItemByName(string name)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].Name.ToLower().Equals(name.ToLower()))
                    return ItemList[i].getCopy<Item>();
            }
            return null;
        }
    }
}