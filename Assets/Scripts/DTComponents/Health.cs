using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTComponents
{
    public class Health : MonoBehaviour, IHealthComponent
    {
        private Toughness toughness;
        private Energy energy;
        private bool isDead;
        private bool lockFlag;
        private bool deadPossible;

        [SerializeField]
        private int maxValue;
        [SerializeField]
        private int currentValue;
        public bool IsModifyOverTimeEnabled;
        public float Frequency;
        public int ModifyValue;
        public delegate void DamageHandler();
        public event DamageHandler OnDeathEvent;
        public event DamageHandler OnAfterValueChangedEvent;
        public event DamageHandler OnBeforeValueChangedEvent;
        public event DamageHandler OnValueZeroOrBelowOnChangeEvent;

        public int MaxValue
        {
            get
            {
                return maxValue;
            }

            set
            {
                maxValue = value;
            }
        }

        public int CurrentValue
        {
            get
            {
                return currentValue;
            }

            set
            {
                if (OnBeforeValueChangedEvent != null)
                {
                    OnBeforeValueChangedEvent();
                }

                currentValue = value;
                currentValue = Mathf.Clamp(currentValue, 0, MaxValue);

                if (OnAfterValueChangedEvent != null)
                {
                    OnAfterValueChangedEvent();
                }
            }
        }

        public virtual void LoadValues()
        {
            if (this.GetType() == typeof(Health))
            {
                var player = GetComponent<Player>();
                CurrentValue = player.PlayerDataModel.Health;
            }
        }

        void Start()
        {
            CurrentValue = MaxValue;
            toughness = GetComponent<Toughness>();
            energy = GetComponent<Energy>();
            deadPossible = !IsModifyOverTimeEnabled;

            //health ise toughness check et
            if (typeof(Health) == this.GetType() && toughness && toughness != this)
            {
                toughness.OnValueZeroOrBelowOnChangeEvent += new Health.DamageHandler(RelatedToughnessValueChangedToZeroOrBelow);
            }

            //health ise energy check et
            if (typeof(Health) == this.GetType() && energy && energy != this)
            {
                energy.OnValueZeroOrBelowOnChangeEvent += new Health.DamageHandler(RelatedEnergyValueChangedToZeroOrBelow);
            }


            var player = GetComponent<Player>();
            if (player != null){
                player.OnFirstUpdate += new Player.PlayerEventHandler(LoadValues);
            }

        }

        public void RelatedToughnessValueChangedToZeroOrBelow()
        {
            Debug.Log("getting damage from toughness");
            TakeDamage(toughness.DecreaseHealthValueAmount);
        }

        public void RelatedEnergyValueChangedToZeroOrBelow()
        {
            Debug.Log("getting damage from energy");
            TakeDamage(energy.DecreaseHealthValueAmount);
        }

        void FixedUpdate()
        {
            if (IsModifyOverTimeEnabled && !lockFlag)
            {
                lockFlag = true;
                StartCoroutine(Modify());
            }
        }

        private IEnumerator Modify()
        {
            yield return new WaitForSeconds(Frequency);

            TakeDamage(-ModifyValue);

            lockFlag = false;
        }

        public void TakeDamage(int? amount)
        {
            if (CurrentValue <= 0)
            {
                OnValueZeroOrBelowOnChange();
            }

            if (isDead)
            {
                return;
            }

            CurrentValue -= amount ?? ModifyValue;

            if (CurrentValue <= 0)
            {
                OnDeath();
            }
        }
        private void OnValueZeroOrBelowOnChange()
        {
            if (OnValueZeroOrBelowOnChangeEvent != null)
            {
                OnValueZeroOrBelowOnChangeEvent();
            }
        }

        private void OnDeath()
        {
            isDead = deadPossible;
            if (isDead && OnDeathEvent != null)
            {
                OnDeathEvent();
            }
        }

        public void Modify(int value)
        {
            TakeDamage(-value);
        }
    }
}