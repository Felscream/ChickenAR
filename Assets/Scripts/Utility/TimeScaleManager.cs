using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    public float Scale = 1f;

    private float _fixedDelta;
    private float _delta;
    
    private static TimeScaleManager _instance;
    public static TimeScaleManager Instance { get { return _instance; } }
    public static float Delta { get { return _instance._delta; } }
    public static float FixedDelta { get { return _instance._fixedDelta; } }
    public static float TimeScale { get { return _instance.Scale; } }

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
    }

    private void FixedUpdate()
    {
        _fixedDelta = Time.fixedDeltaTime * TimeScale;
    }

    private void Update()
    {
        _delta = Time.deltaTime * TimeScale;
    }
}
