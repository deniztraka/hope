using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTComponents
{
    public class Interactable : MonoBehaviour
    {
        public bool IsInteractable;

        public delegate void OnClickHandler();
        public event OnClickHandler OnClickEvent;

        public float distance = 1.5f;

        private bool IsCloseEnough()
        {
            var player = GameObject.FindWithTag("Player");
            return Math.Abs(player.transform.position.x - transform.position.x) < distance;
        }

        public void OnClick()
        {
            if (IsInteractable && IsCloseEnough())
            {
                if (OnClickEvent == null)
                {
                    Debug.Log("OnClickEvent is empty for this object:" + gameObject.name);
                    return;
                }

                OnClickEvent();

            }

        }
    }
}