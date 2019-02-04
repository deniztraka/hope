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
                HealthBehaviour.OnDeathEvent += new Health.DamageHandler(OnDeath);
            }

            if (interactionBehaviour)
            {
                interactionBehaviour.OnClickEvent += new Interactable.OnClickHandler(OnClick);
            }
        }

        protected virtual void OnClick()
        {            
            HealthBehaviour.Modify(-20);
        }

        protected virtual void OnDeath()
        {            
        }
    }
}


