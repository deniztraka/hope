﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaysPastTextBehaviour : MonoBehaviour
{
    
    private bool isInitialized;

    public TimeOfTheDay TimeOfTheDay;
    public string Format;

    // Start is called before the first frame update
    void Start()
    {
        if (TimeOfTheDay != null)
        {
            TimeOfTheDay.OnAfterValueChangedEvent += new TimeOfTheDay.TimeOfTheDayHandler(UpdateText);
        }

        if(string.IsNullOrEmpty(Format)){
            Format = "{1}:{2} - {0} days";
        }
    }
    
    private void UpdateText()
    {
        if (TimeOfTheDay != null)
        {
            var currentGameTime = TimeOfTheDay.GetGameTime();
            var textComp = GetComponent<Text>();
            textComp.text = string.Format(Format,
            currentGameTime.Days,
            currentGameTime.Hours.ToString().Length == 1 ? ("0" + currentGameTime.Hours.ToString()) : currentGameTime.Hours.ToString(),
            currentGameTime.Minutes.ToString().Length == 1 ? ("0" + currentGameTime.Minutes.ToString()) : currentGameTime.Minutes.ToString(),            
            currentGameTime.Seconds);
        }
    }
}
