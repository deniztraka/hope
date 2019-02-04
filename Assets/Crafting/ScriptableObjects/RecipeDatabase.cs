using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTCrafting.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Crafting/RecipeDatabase", order = 1)]
    public class RecipeDatabase : ScriptableObject
    {
       public List<CraftingRecipe> RecipeList;
    }
}