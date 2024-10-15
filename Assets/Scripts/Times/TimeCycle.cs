using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class TimeCycle : MonoBehaviour
{
    [SerializeField] private Gradient _directionalLightGradient;

    [SerializeField, Range(1f, 3600f)] private float _timeDayInSeconds = 60f;
    [SerializeField, Range(0f, 1f)] private float _timeProgress;

    [SerializeField] private Light2D _directionalLight;

    private TimeOfDay _timeOfDay;
    public TimeOfDay TimeOfDay
    {
        get
        {
            return _timeOfDay;
        }
        private set
        {
            if (TimeOfDay != value)
            {
                _timeOfDay = value;
                TimeOfDayChanged?.Invoke(_timeOfDay);
            }
        }
    }

    public UnityAction<TimeOfDay> TimeOfDayChanged;

    private void Update()
    {
        _timeProgress += Time.deltaTime / _timeDayInSeconds;

        if (_timeProgress > 0.25f && _timeProgress < 0.75f)
        {
            TimeOfDay = TimeOfDay.Day;
        }
        else
        {
            TimeOfDay = TimeOfDay.Night;
        }

        if (_timeProgress > 1f)
            _timeProgress = 0f;

        _directionalLight.color = _directionalLightGradient.Evaluate(_timeProgress);
    }
}
