using System;
using System.Collections;
using System.Collections.Generic;
using DTComponents;
using UnityEngine;

namespace DTMobiles
{
    [RequireComponent(typeof(Health), typeof(CharacterController2D))]
    public class BaseMobileBehaviour : MonoBehaviour
    {
        private Health healthComponent;

        private CharacterController2D controller2D;

        private int currentDirection;
        private float movementSpeed = 5;
        private float runningSpeed = 10;
        private float finalMovement;

        private MobileBehaviourStates currentBehaviour;

        public float CurrentHealth
        {
            get { return healthComponent.CurrentValue; }
        }

        void Start()
        {
            healthComponent = GetComponent<Health>();
            healthComponent.OnAfterValueChangedEvent += new Health.DamageHandler(OnHealthChanged);
            healthComponent.OnDeathEvent += new Health.DamageHandler(OnDeath);

            controller2D = GetComponent<CharacterController2D>();
            currentBehaviour = MobileBehaviourStates.Idling;
            StartCoroutine(TryChangeState());
        }

        private IEnumerator TryChangeState()
        {
            while (true)
            {



                yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 3f));
                Debug.Log("trychange");
                if (currentBehaviour != MobileBehaviourStates.Atacking || currentBehaviour != MobileBehaviourStates.Running)
                {
                    currentBehaviour = UnityEngine.Random.value < 0.5f ? MobileBehaviourStates.Walking : MobileBehaviourStates.Idling;
                    Debug.Log("changed to:" + currentBehaviour.ToString());
                    if (currentBehaviour == MobileBehaviourStates.Idling)
                    {
                        currentDirection = UnityEngine.Random.value < 0.5f ? 1 : -1;
                    }
                }
            }
        }

        void Update()
        {
            SetMovementSpeed();
        }

        private void SetMovementSpeed()
        {
            switch (currentBehaviour)
            {
                case MobileBehaviourStates.Idling:
                    finalMovement = 0;
                    break;
                case MobileBehaviourStates.Walking:
                    finalMovement = movementSpeed;
                    break;
                case MobileBehaviourStates.Running:
                    finalMovement = runningSpeed;
                    break;
                case MobileBehaviourStates.Atacking:
                    finalMovement = 0;
                    break;
            }
        }

        void FixedUpdate()
        {
            // Move our character
            controller2D.Move(finalMovement * currentDirection * Time.fixedDeltaTime, false, false);
        }

        protected virtual void OnHealthChanged()
        {
            Debug.Log(gameObject.name + " health is changed.");
        }

        protected virtual void OnDeath()
        {
            Debug.Log(gameObject.name + " is dead.");
        }


    }
}