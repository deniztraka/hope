using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthComponent
{
    event DTComponents.Health.DamageHandler OnAfterValueChangedEvent;
    float MaxValue { get; set;}
    float CurrentValue { get; set; }

    void Modify(float value);
}
