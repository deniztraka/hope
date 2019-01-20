// using System.Collections;
// using System.Collections.Generic;
// using DT.Inventory.ScriptableObjects;
// using UnityEditor;
// using UnityEngine;
// using DT.Inventory.Models;
// using System.Linq;

// [CustomEditor(typeof(ItemDatabase))]
// public class ItemDatabaseEditor : Editor
// {
//     Item tempItem = new Item();
//     bool createItemToggle = false;
//     bool editItemToggle = false;
//     bool deleteItemToggle = false;
//     private ItemTypeDatabase itemTypeDatabase;
//     int typeSelectedIndex = 0;
//     Object source;

//     void OnEnable()
//     {
//         var thisObject = (ItemDatabase)target;
//         itemTypeDatabase = thisObject.ItemTypeDatabase;
//     }

//     public bool CreateItemToogle
//     {
//         get { return createItemToggle; }
//         set
//         {
//             if (value)
//             {
//                 deleteItemToggle = false;
//                 editItemToggle = false;
//             }

//             createItemToggle = value;
//         }
//     }
//     public bool EditItemToogle
//     {
//         get { return editItemToggle; }
//         set
//         {
//             if (value)
//             {
//                 deleteItemToggle = false;
//                 createItemToggle = false;
//             }

//             editItemToggle = value;
//         }
//     }
//     public bool DeleteItemToogle
//     {
//         get { return deleteItemToggle; }
//         set
//         {
//             if (value)
//             {
//                 createItemToggle = false;
//                 editItemToggle = false;
//             }

//             deleteItemToggle = value;
//         }
//     }

//     public override void OnInspectorGUI()
//     {

//         EditorGUILayout.BeginHorizontal();
//         EditorGUILayout.BeginVertical();
//         EditorGUILayout.HelpBox("Please choose what you want to do with item database.", MessageType.Info, true);
//         EditorGUILayout.EndVertical();
//         EditorGUILayout.BeginVertical();
//         CreateItemToogle = EditorGUILayout.Toggle("Create Item", createItemToggle);
//         EditItemToogle = EditorGUILayout.Toggle("Edit Item", editItemToggle);
//         DeleteItemToogle = EditorGUILayout.Toggle("Delete Item", deleteItemToggle);
//         EditorGUILayout.EndVertical();
//         EditorGUILayout.EndHorizontal();
//         EditorGUILayout.Space();
//         if (createItemToggle && !editItemToggle && !deleteItemToggle)
//         {
//             EditorGUILayout.BeginToggleGroup("Create Item", createItemToggle);
//             EditorGUILayout.PrefixLabel("Name");
//             tempItem.Name = EditorGUILayout.TextField(tempItem.Name);
//             EditorGUILayout.PrefixLabel("Description");
//             tempItem.Description = EditorGUILayout.TextField(tempItem.Description);

//             typeSelectedIndex = EditorGUILayout.Popup("Type", typeSelectedIndex, itemTypeDatabase.ItemTypes.Select(it => it.Name).ToArray());
//             tempItem.Type = itemTypeDatabase.GetItemByIndex(typeSelectedIndex);

//             EditorGUILayout.PrefixLabel("GameObject");
//             source = EditorGUILayout.ObjectField(source, typeof(Object), true);
//             EditorGUILayout.PrefixLabel("Name");
//             tempItem.Name = EditorGUILayout.TextField(tempItem.Name);
//             EditorGUILayout.PrefixLabel("Name");
//             tempItem.Name = EditorGUILayout.TextField(tempItem.Name);
//             EditorGUILayout.EndToggleGroup();
//         }

//         if (!createItemToggle && editItemToggle && !deleteItemToggle)
//         {
//             EditorGUILayout.BeginToggleGroup("Edit Item", editItemToggle);
//             EditorGUILayout.PrefixLabel("Name");
//             tempItem.Name = EditorGUILayout.TextField(tempItem.Name);
//             EditorGUILayout.EndToggleGroup();
//         }

//         if (!createItemToggle && !editItemToggle && deleteItemToggle)
//         {
//             EditorGUILayout.BeginToggleGroup("Delete Item", deleteItemToggle);
//             EditorGUILayout.PrefixLabel("Name");
//             tempItem.Name = EditorGUILayout.TextField(tempItem.Name);
//             EditorGUILayout.EndToggleGroup();
//         }
//         EditorGUILayout.Separator();





//         DrawDefaultInspector();


//         EditorUtility.SetDirty(target);
//     }
// }
