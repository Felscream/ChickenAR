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
    public class DayNightController : MonoBehaviour
    {
        /// <summary>
        /// Virtual day length in real world seconds
        /// </summary>
        public float DayCycleLength = 120f;
        public float CurrentCycleTime ;
        public DayPhase CurrentPhase;
        public float HoursPerDay = 24.0f;
        public float DawnTimeOffset = 3.0f;
        public int WorldTimeHour;
        public Color FullLight = new Color(253.0f / 255.0f, 248.0f / 255.0f, 223.0f / 255.0f);
        public Color FullDark = new Color(32.0f / 255.0f, 28.0f / 255.0f, 46.0f / 255.0f);
        public Color DawnDuskFog = new Color(133.0f / 255.0f, 124.0f / 255.0f, 102.0f / 255.0f);
        public Color DayFog = new Color(180.0f / 255.0f, 208.0f / 255.0f, 209.0f / 255.0f);
        public Color NightFog = new Color(12.0f / 255.0f, 15.0f / 255.0f, 91.0f / 255.0f);

        private float _dawnTime;
        private float _dayTime;
        private float _duskTime;
        private float _nightTime;
        private float _quarterDay;
        private float _lightIntensity;

        private Light _light;

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

            CurrentCycleTime += TimeScaleManager.Delta;
            CurrentCycleTime = CurrentCycleTime % DayCycleLength;
        }

        public void SetDawn()
        {
            if(_light != null)
            {
                _light.enabled = true;
            }
            CurrentPhase = DayPhase.Dawn;
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

            CurrentCycleTime = _dayTime;

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

            transform.Rotate(Vector3.up * ((TimeScaleManager.Delta / DayCycleLength) * 360.0f), Space.Self);
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
    }
}

