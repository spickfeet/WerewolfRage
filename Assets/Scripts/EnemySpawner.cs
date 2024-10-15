using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private StageController _stageController;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private int _countEnemies = 5;
    [SerializeField] private float _coolDown = 2;
    [SerializeField] private Player _enemyTarget;
    private float _nextSpawn;

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= _nextSpawn)
        {
            _nextSpawn = Time.time + _coolDown;
            _countEnemies -= 1;
            GameObject enemyGameObject = Instantiate(_enemy.gameObject, transform.position, Quaternion.identity);
            Enemy enemy = enemyGameObject.GetComponent<Enemy>();
            enemy.Player = _enemyTarget;
            _stageController.AddEnemy(enemy);
        }
        if(_countEnemies <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}

