using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTComponents
{
    public class Health : MonoBehaviour
    {
        public int MaxHealth;
        public int CurrentHealth;

        public delegate void DieHandler ();
        public event DieHandler OnDeathEvent;

        public delegate void DamageHandler ();
        public event DamageHandler OnAfterTookDamage;
        public event DamageHandler OnBeforeTookDamage;

        private bool isDead;

        void Start()
        {
            CurrentHealth = MaxHealth;            
        }

        public void TakeDamage()
        {
            if (isDead || CurrentHealth <= 0)
            {
                return;
            }

            

            if(OnBeforeTookDamage != null){
                OnBeforeTookDamage();
            }

            var playerObj = GameObject.FindGameObjectWithTag("Player");
            var player = playerObj.GetComponent<Player>();
            var damageAmount = player.GetCurrentDamage();

            CurrentHealth -= damageAmount;

            if(OnAfterTookDamage != null){
                OnAfterTookDamage();
            }

            if(CurrentHealth <= 0){
                OnDeath();
            }
        }
        private void OnDeath()
        {
            isDead = true;
            if(OnDeathEvent != null){
                OnDeathEvent();
            }
        }
    }
}