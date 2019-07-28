using System.Collections;
using System.Collections.Generic;
using DTComponents;
using DTInterfaces;
using UnityEngine;

namespace DTObjects.Statics
{
    [RequireComponent(typeof(Interactable), typeof(Health))]
    public abstract class GameStaticObject : DObject
    {
        public GameObjectType Type;
        private Interactable interactionBehaviour;
        public Health HealthBehaviour;

        void Start()
        {

        }

        protected void Init()
        {
            HealthBehaviour = GetComponent<Health>();
            interactionBehaviour = GetComponent<Interactable>();
            if (HealthBehaviour)
            {
                HealthBehaviour.OnDeathEvent += new Health.DeathZeroHandler(OnDeath);
            }

            if (interactionBehaviour)
            {
                interactionBehaviour.OnClickEvent += new Interactable.OnClickHandler(OnClick);
            }
        }

        protected virtual void OnClick()
        {       
            var player = GameObject.Find("Player");  
            var animator = player.GetComponent<Animator>();             
            animator.Play("Sit");            
            HealthBehaviour.Modify(-20);
        }

        protected virtual void OnDeath()
        {            
        }
    }
}


