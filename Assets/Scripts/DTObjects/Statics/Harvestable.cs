using System.Collections;
using System.Collections.Generic;
using DTComponents;
using UnityEngine;

namespace DTObjects.Statics
{
    [RequireComponent(typeof(Drop))]
    public class Harvestable : GameStaticObject
    {
        protected Drop DropBehaviour;

        void Start()
        {
            Type = GameObjectType.Harvestable;

            Init();
            DropBehaviour = GetComponent<Drop>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void OnClick(){
            base.OnClick();            
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            DropBehaviour.TryDropItems();
            Destroy(gameObject);
        }
    }
}