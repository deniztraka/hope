using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthComponent
{
    event DTComponents.Health.DamageHandler OnAfterTookDamageEvent;
    int MaxValue { get; }
    int CurrentValue { get;  }

    void Modify(int value);
}
