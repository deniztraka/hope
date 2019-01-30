using System;
using System.Collections;
using System.Collections.Generic;
using DTComponents;
using UnityEngine;
using UnityEngine.UI;

namespace DTUI.Components
{
    public class DHealthbar : MonoBehaviour
    {
        private Slider healthbarDisplay;

        [Header("Healthbar Values:")]

        private IHealthComponent Health;

        public string BarType;
        // If the character has this health or less, consider them having low health:
        [Tooltip("Low health is less than or equal to this:")] public int lowHealth = 33;

        // If the character has between this health and "low health", consider them having medium health:
        // If they have more than this health, consider them having highHealth:
        [Tooltip("High health is greater than or equal to this:")] public int highHealth = 66;

        [Space]

        [Header("Healthbar Colors:")]
        public Color highHealthColor = new Color(0.35f, 1f, 0.35f);
        public Color mediumHealthColor = new Color(0.9450285f, 1f, 0.4481132f);
        public Color lowHealthColor = new Color(1f, 0.259434f, 0.259434f);

        private float healthPercentage = 100;
        public float CurrentHealth;

        // Start is called before the first frame update
        void Start()
        {
            // If the healthbar hasn't already been assigned, then automatically assign it.
            if (healthbarDisplay == null)
            {
                healthbarDisplay = GetComponent<Slider>();
            }

            var playerObj = GameObject.Find("Player");
            Health = (IHealthComponent)playerObj.GetComponent(BarType);
            Health.OnAfterValueChangedEvent += new Health.DamageHandler(OnAfterValueChanged);

            // Set the minimum and maximum health on the healthbar to be equal to the 'minimumHealth' and 'maximumHealth' variables:
            healthbarDisplay.minValue = 0;
            healthbarDisplay.maxValue = Health.MaxValue;
            healthPercentage = Health.MaxValue;
            CurrentHealth = Health.CurrentValue;
            // Change the starting visible health to be equal to the variable:
            UpdateUI();
        }

        private void OnAfterValueChanged()
        {
            CurrentHealth = Health.CurrentValue;
            UpdateUI();
        }

        // Update is called once per frame
        void Update()
        {
            healthPercentage = int.Parse((Mathf.Round(Health.MaxValue * (CurrentHealth / 100f))).ToString());
        }

        public void UpdateUI()
        {
            // Change the health bar color acording to how much health the player has:
            if (healthPercentage <= lowHealth && CurrentHealth >= 0 && transform.Find("Bar").GetComponent<Image>().color != lowHealthColor)
            {
                ChangeHealthbarColor(lowHealthColor);
            }
            else if (healthPercentage <= highHealth && CurrentHealth > lowHealth)
            {
                float lerpedColorValue = (float.Parse(healthPercentage.ToString()) - 25) / 41;
                ChangeHealthbarColor(Color.Lerp(lowHealthColor, mediumHealthColor, lerpedColorValue));
            }
            else if (healthPercentage > highHealth && CurrentHealth <= Health.MaxValue)
            {
                float lerpedColorValue = (float.Parse(healthPercentage.ToString()) - 67) / 33;
                ChangeHealthbarColor(Color.Lerp(mediumHealthColor, highHealthColor, lerpedColorValue));
            }

            healthbarDisplay.value = CurrentHealth;
        }

        public void ChangeHealthbarColor(Color colorToChangeTo)
        {
            transform.Find("Bar").GetComponent<Image>().color = colorToChangeTo;
        }
    }
}