using System;
using System.Collections;
using System.Collections.Generic;
using DTInventory.Models;
using UnityEngine;

namespace DTCrafting.Models
{
    [Serializable]
    public class RequiredItem
    {
        public Item Item;
        public int Amount;
    }
}