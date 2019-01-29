using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthComponent
{
    event DTComponents.Health.DamageHandler OnAfterValueChangedEvent;
    int MaxValue { get; set;}
    int CurrentValue { get; set; }

    void Modify(int value);
}
