using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _health;
    [SerializeField] private Player _player;
    private void Start()
    {
        _player.HealthChanged += OnHealthChanged;
    }
    
    private void OnHealthChanged(float currentHealthPercent)
    {
        _health.fillAmount = currentHealthPercent;
    }

}
