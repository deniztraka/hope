using System;
using System.Collections;
using System.Collections.Generic;
using DTComponents;
using UnityEngine;

namespace DTMobiles
{
    [RequireComponent(typeof(MobileStateDecisionMaker), typeof(CharacterController2D))]
    public class BaseMobileBehaviour : MonoBehaviour
    {
        private MobileStateDecisionMaker mobileStateDecisionMaker;
        private float finalMovement;
        private CharacterController2D controller2D;
        private int currentDirection;
        private Player player;

        public float MovementSpeed = 5;
        public float RunningSpeed = 10;
        public Animator animator;

        public MobileBehaviourStates currentBehaviour;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            controller2D = GetComponent<CharacterController2D>();
            mobileStateDecisionMaker = GetComponent<MobileStateDecisionMaker>();

            mobileStateDecisionMaker.OnStateChanged += new MobileStateDecisionMaker.MobileStateDecisionHandler(OnStateChanged);

            animator = GetComponent<Animator>();
        }

        private void OnStateChanged(MobileBehaviourStates newState)
        {
            if (newState == MobileBehaviourStates.Walking)
            {
                currentDirection = UnityEngine.Random.value < 0.5 ? -1 : 1;
            }
        }

        void Update()
        {
            currentBehaviour = mobileStateDecisionMaker.CurrentState;
            SetMovementSpeed();
        }

        private void SetMovementSpeed()
        {
            var playerDistance = player.transform.position.x - transform.position.x;

            switch (currentBehaviour)
            {
                case MobileBehaviourStates.Idling:
                    finalMovement = 0;
                    break;
                case MobileBehaviourStates.Walking:
                    finalMovement = MovementSpeed;
                    break;
                case MobileBehaviourStates.Chaseing:
                    finalMovement = RunningSpeed;
                    currentDirection = playerDistance < 0 ? -1 : 1;
                    break;
                case MobileBehaviourStates.Atacking:
                    finalMovement = 0;
                    break;
                case MobileBehaviourStates.Escapeing:
                    finalMovement = RunningSpeed;
                    currentDirection = playerDistance < 0 ? 1 : -1;
                    break;
            }

            animator.SetFloat("speed", finalMovement);
        }

        void FixedUpdate()
        {
            // Move our character
            controller2D.Move(finalMovement * currentDirection * Time.fixedDeltaTime, false, false);
        }
    }
}