using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime
{
    public int Days;
    public int Hours;
    public int Minutes;
    public int Seconds;

    public GameTime(int days, int hours, int minutes, int seconds){
        Days = days;
        Hours = hours;
        Minutes = minutes;
        Seconds = seconds;
    }
}

public class TimeOfTheDay : MonoBehaviour
{
    public int DayLengthInSeconds;
    public bool isEnabled;
    public float TimeMultiplier;

    public delegate void TimeOfTheDayHandler();
    public event TimeOfTheDayHandler OnAfterValueChangedEvent;

    private int currentDay;
    private int currentHour;
    private int currentMinute;
    private int currentSecond;

    [SerializeField]
    private int processFrequencyInSeconds;

    [SerializeField]
    private int realGameSecondsPast;

    private float currentTimeOfDay;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        if (isEnabled)
        {
            StartCoroutine(Process());
        }
    }

    private void Init()
    {
        realGameSecondsPast = 0;
    }

    public GameTime GetGameTime(){
        return new GameTime(currentDay, currentHour, currentMinute, currentSecond);
    }

    // Update is called once per frame
    void Update()
    {
        // This makes currentTimeOfDay go from 0 to 1 in the number of seconds we've specified.
        // currentTimeOfDay += (Time.deltaTime / DayLengthInSeconds) * TimeMultiplier;
        // currentHour = Mathf.FloorToInt(currentTimeOfDay* 24);

        // // If currentTimeOfDay is 1 (midnight) set it to 0 again so we start a new day.
        // if (currentTimeOfDay >= 1)
        // {
        //     currentDay++;
        //     currentTimeOfDay = 0;
        // }
    }

    private IEnumerator Process()
    {
        while (isEnabled)
        {
            yield return new WaitForSeconds((float)processFrequencyInSeconds);
            CalculateTimeOfTheDay();
            realGameSecondsPast++;
        }
    }

    public void CalculateTimeOfTheDay()
    {
        //Debug.Log(currentTimeOfDay);
        if (realGameSecondsPast > 0)
        {

            var ratio = DayLengthInSeconds / 86400f;
            //how many seconds past according to game time.
            var secondsPastInGame = realGameSecondsPast / ratio;

            var dayx = secondsPastInGame / (24 * 3600);

            secondsPastInGame = secondsPastInGame % (24 * 3600);
            var hourx = secondsPastInGame / 3600;

            secondsPastInGame %= 3600;
            var minutesx = secondsPastInGame / 60;

            secondsPastInGame %= 60;
            var secondsx = secondsPastInGame;

            currentDay = (int)dayx;
            currentHour = (int)hourx;
            currentMinute = (int)minutesx;
            currentSecond = (int)secondsx;

            if (OnAfterValueChangedEvent != null)
            {
                OnAfterValueChangedEvent();
            }

        }

    }
}

