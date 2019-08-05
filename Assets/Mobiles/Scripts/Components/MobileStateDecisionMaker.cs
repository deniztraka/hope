using System;
using System.Collections;
using System.Collections.Generic;
using DTComponents;
using UnityEngine;

public class MobileStateDecisionMaker : MonoBehaviour
{
    private Health healthComponent;
    private Player player;
    public bool IsPeaceful;
    public float WalkingChance = 0.5f;
    public float AttackRange = 1f;
    public float ChaseRange = 6f;
    public float MinStateDecisionFrequency = 0f;
    public float MaxStateDecisionFrequency = 3f;
    public float StateChangingChance = 0.5f;
    public float EscapeHealth = 40;

    public MobileBehaviourStates CurrentState;
    public delegate void MobileStateDecisionHandler(MobileBehaviourStates currentState);
    public event MobileStateDecisionHandler OnStateChanged;
    public float CurrentHealth
    {
        get { return healthComponent.CurrentValue; }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        healthComponent = GetComponent<Health>();
        healthComponent.OnAfterValueChangedEvent += new Health.DamageHandler(OnHealthChanged);
        healthComponent.OnDeathEvent += new Health.DeathZeroHandler(OnDeath);


        CurrentState = MobileBehaviourStates.Idling;
        StartCoroutine(TryChangeState());
    }

    private void OnDeath()
    {
        throw new NotImplementedException();
    }

    private void OnHealthChanged(float beforeValue, float afterValue)
    {
        if ((CurrentHealth < EscapeHealth) || (IsPeaceful && (beforeValue > afterValue)))
        {
            var playerDistance = Math.Abs(player.transform.position.x - transform.position.x);
            var playerInChaceRange = playerDistance <= ChaseRange;
            var playerInAttackRange = playerDistance <= AttackRange;
            if (playerInChaceRange)
            {
                CurrentState = MobileBehaviourStates.Escapeing;
            }
        }
    }

    private IEnumerator TryChangeState()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(MinStateDecisionFrequency, MaxStateDecisionFrequency));

            MakeDecision();
        }
    }

    public virtual void MakeDecision()
    {
        var tempCurrState = CurrentState;

        var playerInChaceRange = false;
        var playerInAttackRange = false;

        // if (!IsPeaceful)
        // {
        var playerDistance = Math.Abs(player.transform.position.x - transform.position.x);
        playerInChaceRange = playerDistance <= ChaseRange;
        playerInAttackRange = playerDistance <= AttackRange;
        // }

        switch (CurrentState)
        {
            case MobileBehaviourStates.Idling:
                if (!playerInChaceRange || IsPeaceful)
                {
                    CurrentState = UnityEngine.Random.value < StateChangingChance ? MobileBehaviourStates.Walking : MobileBehaviourStates.Idling;
                }
                else if (CurrentHealth > EscapeHealth && !IsPeaceful)
                {
                    CurrentState = MobileBehaviourStates.Chaseing;
                }
                break;
            case MobileBehaviourStates.Walking:
                if (!playerInChaceRange || IsPeaceful)
                {
                    CurrentState = UnityEngine.Random.value < StateChangingChance ? MobileBehaviourStates.Walking : MobileBehaviourStates.Idling;
                }
                else if (CurrentHealth > EscapeHealth && !IsPeaceful)
                {
                    CurrentState = MobileBehaviourStates.Chaseing;
                }
                break;
            case MobileBehaviourStates.Chaseing:
                if (!playerInChaceRange)
                {
                    CurrentState = MobileBehaviourStates.Idling;
                }
                else if (playerInAttackRange && (CurrentHealth > EscapeHealth) && !IsPeaceful)
                {
                    CurrentState = MobileBehaviourStates.Atacking;
                }
                break;

            case MobileBehaviourStates.Atacking:
                if (!playerInAttackRange && (CurrentHealth > EscapeHealth) && !IsPeaceful)
                {
                    CurrentState = MobileBehaviourStates.Chaseing;
                }
                break;
            case MobileBehaviourStates.Escapeing:
                if (!playerInChaceRange)
                {
                    CurrentState = MobileBehaviourStates.Walking;
                }
                break;
            default:
                // Change nothing
                //CurrentState = CurrentState;
                break;
        }

        if (tempCurrState != CurrentState)
        {
            if (OnStateChanged != null)
            {
                OnStateChanged(CurrentState);
            }
        }
    }
}
