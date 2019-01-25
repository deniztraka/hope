using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTInterfaces
{
    public interface IDestructable
    {
        float HealthAmount
        {
            get;
            set;
        }

        bool IsDead
        {
            get;
            set;
        }

        void TakeDamage(float damageAmount);
    }
}


