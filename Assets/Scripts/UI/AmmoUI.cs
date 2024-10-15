using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private Image[] _ammo;
    [SerializeField] private Gun _gun;

    private void Start()
    {
        _gun.AmmoChanged += OnAmmoChanged;
    }
    private void OnAmmoChanged(int ammo)
    {
        foreach (Image item in _ammo)
        {
            item.enabled = false;
        }
        for (int i = 0; i < ammo; i++)
        {
            _ammo[i].enabled = true;
        }
    }
    
    private void Update()
    {
        
    }
}
