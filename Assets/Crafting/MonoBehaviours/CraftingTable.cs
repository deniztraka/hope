using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTInventory.MonoBehaviours;

namespace DTCrafting.MonoBehaviours
{
    public class CraftingTable : InventoryBehavior
    {
        // Start is called before the first frame update
        void Start()
        {
            Init();

            //gameObject.transform.localScale = new Vector3(0, 0, 0);            
        }

        public void Init()
        {
            SlotGrid = new GameObject[SizeX][];
            for (int x = 0; x < SlotGrid.Length; x++)
            {
                if (SlotGrid[x] == null)
                {
                    SlotGrid[x] = new GameObject[SizeY];
                }
                for (int y = 0; y < SlotGrid[x].Length; y++)
                {
                    var slotObj = Instantiate(SlotPrefab, Vector3.zero, Quaternion.identity, SlotsWrapper);
                    var slot = slotObj.GetComponent<SlotBehaviour>();
                    slot.IsSelectable = false;
                    SlotGrid[x][y] = slot.gameObject;
                }
            }
        }

        internal override void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
        {
            base.OnSimpleDragAndDropEvent(desc);

            Debug.Log("check recipe database if crafting table has item.");
        }

    }
}