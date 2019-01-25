using System;
using System.Collections;
using System.Collections.Generic;
using DTModels;
using UnityEngine;
namespace DTComponents
{
    public class Drop : MonoBehaviour
    {
        public List<DropItem> Drops;
        internal void TryDropItems()
        {
            foreach (var item in Drops)
            {
                var rnd = UnityEngine.Random.Range(0f, 1f);
                if (rnd <= item.DropRate)
                {
                    var rndQuantity = UnityEngine.Random.Range(1, item.Quantity);

                    for (int i = 0; i < rndQuantity; i++)
                    {
                        Instantiate(item.Object, new Vector3(UnityEngine.Random.Range(transform.position.x - 1, transform.position.x + 1), transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
                    }
                }
            }
        }
    }
}