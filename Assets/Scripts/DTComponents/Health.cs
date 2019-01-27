using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTComponents
{
    public class Health : MonoBehaviour, IHealthComponent
    {
        public int MaxValue;
        public int CurrentValue;
        public bool IsModifyOverTimeEnabled;
        public float Frequency;
        public int ModifyValue;

        public delegate void DamageHandler();
        public event DamageHandler OnDeathEvent;
        public event DamageHandler OnAfterTookDamageEvent;
        public event DamageHandler OnBeforeTookDamageEvent;
        public event DamageHandler OnValueZeroOrBelowOnChangeEvent;

        private Toughness toughness;
        private Energy energy;

        private bool isDead;
        private bool lockFlag;

        private bool deadPossible;

        int IHealthComponent.MaxValue
        {
            get
            {
                return MaxValue;
            }
        }

        int IHealthComponent.CurrentValue
        {
            get
            {
                return CurrentValue;
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

            if (OnBeforeTookDamageEvent != null)
            {
                OnBeforeTookDamageEvent();
            }

            CurrentValue -= amount ?? ModifyValue;
            CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue);

            if (OnAfterTookDamageEvent != null)
            {
                OnAfterTookDamageEvent();
            }

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