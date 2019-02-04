using System.Collections;
using System.Collections.Generic;
using DTCrafting.Models;
using DTInventory.Models;
using UnityEngine;

namespace DTCrafting.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Crafting/Recipe", order = 2)]
    public class CraftingRecipe : ScriptableObject
    {
        public List<RequiredItem> RequiredItems;
        public Item OutputItem;

    }
}