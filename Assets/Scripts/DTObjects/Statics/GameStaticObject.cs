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
        private Health healthBehaviour;

        void Start()
        {

        }

        protected void Init()
        {
            healthBehaviour = GetComponent<Health>();
            interactionBehaviour = GetComponent<Interactable>();
            if (healthBehaviour)
            {
                healthBehaviour.OnDeathEvent += new Health.DamageHandler(OnDeath);
            }

            if (interactionBehaviour)
            {
                interactionBehaviour.OnClickEvent += new Interactable.OnClickHandler(OnClick);
            }
        }

        protected virtual void OnClick()
        {            
            healthBehaviour.TakeDamage(20);
        }

        protected virtual void OnDeath()
        {            
        }
    }
}


