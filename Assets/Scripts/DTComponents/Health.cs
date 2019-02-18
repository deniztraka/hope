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
        private float maxValue = 100;
        [SerializeField]
        private float currentValue = 100;
        public bool IsModifyOverTimeEnabled;
        public float Frequency;
        public float ModifyValue;
        public delegate void DamageHandler(float beforeValue, float afterValue);

        public delegate void DeathZeroHandler();
        public event DeathZeroHandler OnDeathEvent;
        public event DamageHandler OnAfterValueChangedEvent;
        public event DamageHandler OnBeforeValueChangedEvent;
        public event DeathZeroHandler OnValueZeroOrBelowOnChangeEvent;

        public float MaxValue
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

        public float CurrentValue
        {
            get
            {
                return currentValue;
            }

            set
            {
                var tempValue = Mathf.Clamp(value, 0, MaxValue);
                var tempCurrentValue = currentValue;

                if (OnBeforeValueChangedEvent != null)
                {
                    OnBeforeValueChangedEvent(currentValue, tempValue);
                }

                currentValue = tempValue;

                if (OnAfterValueChangedEvent != null)
                {
                    OnAfterValueChangedEvent(tempCurrentValue, currentValue);
                }
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
                toughness.OnValueZeroOrBelowOnChangeEvent += new Health.DeathZeroHandler(RelatedToughnessValueChangedToZeroOrBelow);
            }

            //health ise energy check et
            if (typeof(Health) == this.GetType() && energy && energy != this)
            {
                energy.OnValueZeroOrBelowOnChangeEvent += new Health.DeathZeroHandler(RelatedEnergyValueChangedToZeroOrBelow);
            }




        }

        public void RelatedToughnessValueChangedToZeroOrBelow()
        {
            Debug.Log("getting damage from toughness");
            Modify(toughness.DecreaseHealthValueAmount);
        }

        public void RelatedEnergyValueChangedToZeroOrBelow()
        {
            Debug.Log("getting damage from energy");
            Modify(energy.DecreaseHealthValueAmount);
        }

        void FixedUpdate()
        {
            if (IsModifyOverTimeEnabled && !lockFlag)
            {
                lockFlag = true;
                StartCoroutine(ModifyOverTime());
            }
        }

        private IEnumerator ModifyOverTime()
        {
            yield return new WaitForSeconds(Frequency);

            Modify(ModifyValue);

            lockFlag = false;
        }

        public void Modify(float? amount)
        {
            if (CurrentValue <= 0)
            {
                OnValueZeroOrBelowOnChange();
            }

            if (isDead)
            {
                return;
            }

            CurrentValue += amount ?? ModifyValue;

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
    }
}