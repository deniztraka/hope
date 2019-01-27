using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTComponents
{
    public class Consumable : MonoBehaviour, IConsumable
    {
        public List<StatModifier> Modifiers;

        public virtual void Consume()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");


            foreach (var modifier in Modifiers)
            {
                var statToModify = (IHealthComponent)playerObj.GetComponent(modifier.Name);
                statToModify.Modify(modifier.Value);                
            }

            Debug.Log("Consumable: Item consumed.");

        }
    }

[Serializable]
    public class StatModifier
    {
        public string Name;
        public int Value;
    }
}