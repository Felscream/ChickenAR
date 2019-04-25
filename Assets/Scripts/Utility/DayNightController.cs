using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DayNightSystem {
    public enum DayPhase
    {
        Night,
        Dawn,
        Day,
        Dusk
    }

    [RequireComponent(typeof(TimeScaleManager))]
    public class DayNightController : MonoBehaviour, ISubject
    {
        /// <summary>
        /// Virtual day length in real world seconds
        /// </summary>
        public float DayCycleLength = 120f;
        public DayPhase CurrentPhase;
        public float HoursPerDay = 24.0f;
        public float DawnTimeOffset = 3.0f;
        public int WorldTimeHour;
        public Color FullLight = new Color(253.0f / 255.0f, 248.0f / 255.0f, 223.0f / 255.0f);
        public Color FullDark = new Color(32.0f / 255.0f, 28.0f / 255.0f, 46.0f / 255.0f);
        public Color DawnDuskFog = new Color(133.0f / 255.0f, 124.0f / 255.0f, 102.0f / 255.0f);
        public Color DayFog = new Color(180.0f / 255.0f, 208.0f / 255.0f, 209.0f / 255.0f);
        public Color NightFog = new Color(12.0f / 255.0f, 15.0f / 255.0f, 91.0f / 255.0f);

        public string DawnAudio = "ChickenDawn";
        public string DuskAudio = "OwlDusk";

        private float _dawnTime;
        private float _dayTime;
        private float _duskTime;
        private float _nightTime;
        private float _quarterDay;
        private float _lightIntensity;
        private float _currentCycleTime;

        private Light _light;

        private List<IObserver> _observers;

        public float CurrentCycleTime
        {
            get { return _currentCycleTime; }
            set
            {
                _currentCycleTime = value % DayCycleLength;
            }
        }

        private void Awake()
        {
            _observers = new List<IObserver>();    
        }

        private void Start()
        {
            _light = GetComponent<Light>();
            Setup();
        }

        private void Update()
        {
            if (CurrentCycleTime > _nightTime && CurrentPhase == DayPhase.Dusk)
            {
                SetNight();
            }
            else if (CurrentCycleTime > _duskTime && CurrentPhase == DayPhase.Day)
            {
                SetDusk();
            }
            else if (CurrentCycleTime > _dayTime && CurrentPhase == DayPhase.Dawn)
            {
                SetDay();
            }
            else if (CurrentCycleTime > _dawnTime && CurrentCycleTime < _dayTime && CurrentPhase == DayPhase.Night)
            {
                SetDawn();
            }

            UpdateWorldTime();
            UpdateDayLight();
            UpdateFog();

            _currentCycleTime += TimeScaleManager.Delta;
            if(CurrentCycleTime >= DayCycleLength)
            {
                NotifyObservers();
            }
            _currentCycleTime %= DayCycleLength;
        }

        public void SetDawn()
        {
            if(_light != null)
            {
                _light.enabled = true;
            }
            CurrentPhase = DayPhase.Dawn;
            Audio.AudioManager.PlaySoundEffect(DawnAudio);
        }

        public void SetDay()
        {
            if(_light != null)
            {
                _light.intensity = _lightIntensity;
            }
            CurrentPhase = DayPhase.Day;
        }

        public void SetDusk()
        {
            CurrentPhase = DayPhase.Dusk;
            Audio.AudioManager.PlaySoundEffect(DuskAudio);
        }

        public void SetNight()
        {
            if (_light != null)
            {
                _light.enabled = false;
            }
            CurrentPhase = DayPhase.Night;
        }

        private void Setup()
        {
            _quarterDay = DayCycleLength * 0.25f;
            _dawnTime = 0.0f;
            _dayTime = _dawnTime + _quarterDay;
            _duskTime = _dayTime + _quarterDay;
            _nightTime = _duskTime + _quarterDay;

            if(_light != null)
            {
                _lightIntensity = _light.intensity;
            }
        }

        private void Reset()
        {
            DayCycleLength = 120.0f;
            HoursPerDay = 24.0f;
            DawnTimeOffset = 3.0f;
            FullDark = new Color(32.0f / 255.0f, 28.0f / 255.0f, 46.0f / 255.0f);
            FullLight = new Color(253.0f / 255.0f, 248.0f / 255.0f, 223.0f / 255.0f);
            DawnDuskFog = new Color(133.0f / 255.0f, 124.0f / 255.0f, 102.0f / 255.0f);
            DayFog = new Color(180.0f / 255.0f, 208.0f / 255.0f, 209.0f / 255.0f);
            NightFog = new Color(12.0f / 255.0f, 15.0f / 255.0f, 91.0f / 255.0f);
        }

        private void UpdateDayLight()
        {
            if(CurrentPhase == DayPhase.Dawn)
            {
                float relativeTime = CurrentCycleTime - _dawnTime;
                RenderSettings.ambientLight = Color.Lerp(FullDark, FullLight, relativeTime / _quarterDay);
                if(_light != null)
                {
                    _light.intensity = _lightIntensity * (relativeTime / _quarterDay);
                }
            }
            else if(CurrentPhase == DayPhase.Dusk)
            {
                float relativeTime = CurrentCycleTime - _duskTime;
                RenderSettings.ambientLight = Color.Lerp(FullLight, FullDark, relativeTime / _quarterDay);
                if (_light != null)
                {
                    _light.intensity = _lightIntensity * ((_quarterDay - relativeTime) / _quarterDay);
                }
            }

            transform.localEulerAngles = new Vector3((CurrentCycleTime / DayCycleLength) * 360f, -90f, -90f);
        }

        private void UpdateFog()
        {
            if (CurrentPhase == DayPhase.Dawn)
            {
                float relativeTime = CurrentCycleTime - _dawnTime;
                RenderSettings.fogColor = Color.Lerp(DawnDuskFog, DayFog, relativeTime / _quarterDay);
            }
            else if (CurrentPhase == DayPhase.Day)
            {
                float relativeTime = CurrentCycleTime - _dayTime;
                RenderSettings.fogColor = Color.Lerp(DayFog, DawnDuskFog, relativeTime / _quarterDay);
            }
            else if (CurrentPhase == DayPhase.Dusk)
            {
                float relativeTime = CurrentCycleTime - _duskTime;
                RenderSettings.fogColor = Color.Lerp(DawnDuskFog, NightFog, relativeTime / _quarterDay);
            }
            else if (CurrentPhase == DayPhase.Night)
            {
                float relativeTime = CurrentCycleTime - _nightTime;
                RenderSettings.fogColor = Color.Lerp(NightFog, DawnDuskFog, relativeTime / _quarterDay);
            }
        }

        private void UpdateWorldTime()
        {
            WorldTimeHour = (int)((Mathf.Ceil((CurrentCycleTime / DayCycleLength) * HoursPerDay) + DawnTimeOffset) % HoursPerDay) + 1;
        }

        public void AddObserver(IObserver o)
        {
            if (!_observers.Contains(o))
            {
                _observers.Add(o);
            }
        }

        public void RemoveObserver(IObserver o)
        {
            int id = _observers.FindIndex(x => x.Equals(o));
            Debug.Log(id);
            if (id != -1)
            {
                Debug.Log("observer found");
                _observers.RemoveAt(id);
            }
        }

        public void NotifyObservers()
        {
            for(int i = 0; i < _observers.Count; i++)
            {
                _observers[i].ObserverUpdate();
            }
        }
    }
}

