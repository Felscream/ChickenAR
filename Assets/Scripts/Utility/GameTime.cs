using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DayNightSystem;

[RequireComponent(typeof(DayNightController))]
public class GameTime : MonoBehaviour, IObserver
{
    public struct TimeData
    {
        public int Hours;
        public string Period;

        public TimeData(int h, string p)
        {
            Hours = h;
            Period = p;
        }
    }

    const string AM = "AM";
    const string PM = "PM";

    private int _dayCounter = 1;
    private DayNightController _dayNightController;
    
    private static GameTime _instance;
    public static GameTime Instance { get { return _instance; } }
    public int Day { get { return _dayCounter; } }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }

        _dayNightController = GetComponent<DayNightController>();
    }

    private void Start()
    {
        _dayNightController.AddObserver(this);
    }

    public TimeData CurrentTime()
    {
        int gameTime = _dayNightController.WorldTimeHour;
        return new TimeData(gameTime < 13 ? gameTime % 13 : gameTime % 12, gameTime < 12 ? AM : gameTime == 24 ? AM : PM);
    }

    public void SetTimeofDay(float time)
    {
        _dayNightController.CurrentCycleTime = time;
    }

    public void ObserverUpdate()
    {
        _dayCounter++;
    }
}
