using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTInventory.MonoBehaviours
{
    [RequireComponent(typeof(OnDeathEventBehaviour))]
    public class DropBehaviour : MonoBehaviour
    {
        public List<DropItem> Drops;

        void Start()
        {
            var onDeathEventBehaviour = gameObject.GetComponent<OnDeathEventBehaviour>();
            onDeathEventBehaviour.OnDeath += DropItems;
        }

        void DropItems()
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

    [Serializable]
    public class DropItem
    {
        public double DropRate;
        public GameObject Object;
        public int Quantity;
    }
}