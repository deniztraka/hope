using System.Collections;
using System.Collections.Generic;
using DTObjects.Statics;
using UnityEngine;

namespace DTObjects.Statics
{
    public class Shelter : GameStaticObject
    {
        void Start()
        {
            Type = GameObjectType.Usable;

            Init();

            gameObject.transform.position.Set(gameObject.transform.position.x,gameObject.transform.position.y,-2.9f);
        }
        protected override void OnClick(){
            Debug.Log("clicked on shelter.");
        }
    }
}