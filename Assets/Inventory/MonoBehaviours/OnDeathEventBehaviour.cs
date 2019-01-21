using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathEventBehaviour : MonoBehaviour
{
    public delegate void OnDeathEvent();
    public event OnDeathEvent OnDeath;    

    public void DeathOccurred(){
        OnDeath();
    }
}
