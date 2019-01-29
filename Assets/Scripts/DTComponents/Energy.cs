using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTComponents
{
    public class Energy : Health
    {
        public int DecreaseHealthValueAmount;

        public override void LoadValues()
        {
            if (this.GetType() == typeof(Energy))
            {
                var player = GetComponent<Player>();
                CurrentValue = player.PlayerDataModel.Energy;
            }
        }
    }
}