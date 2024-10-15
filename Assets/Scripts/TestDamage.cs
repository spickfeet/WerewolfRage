using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamage : MonoBehaviour
{
    [SerializeField] private Player _player;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _player.ApplyDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            _player.ApplyHeal(1);
        }
    }

}
