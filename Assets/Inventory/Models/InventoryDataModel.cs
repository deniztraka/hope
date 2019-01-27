using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTInventory.Models
{

    [Serializable]
    public class InventoryDataModel
    {
        public List<Item> Items;

        public InventoryDataModel()
        {
            Items = new List<Item>();            
        }

    }

}