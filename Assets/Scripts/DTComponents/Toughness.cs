using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTComponents
{
    public class Toughness : Health
    {
        public int DecreaseHealthValueAmount;

        public override void LoadValues(){
            if (this.GetType() == typeof(Toughness))
            {
                var player = GetComponent<Player>();
                CurrentValue = player.PlayerDataModel.Toughness;
            }
        }                
    }
}