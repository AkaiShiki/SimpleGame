using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _intervalEnemies = 10.0f;
    [SerializeField] private float _intervalMinEnemies = 5.0f;
    [SerializeField] private float _intervalItems = 15.0f;
    [SerializeField] private float _intervalMaxItems = 30.0f;
    [SerializeField] private PoolerProjectile _poolerProjectile;
    [SerializeField] private Pooler _poolerItems;
    [SerializeField] private Pooler _poolerEnemies;
    private float _timerEnemies = 0;
    private float _timerItems = 0;

    private void Start()
    {
        _poolerItems.GetPooledObject();
        _poolerEnemies.GetPooledObject();
    }

    private void Update()
    {
        _timerEnemies += Time.deltaTime;
        _timerItems += Time.deltaTime;
        
        if (_timerItems >= _intervalItems)
        {
            SpawnItems();
        }

        if (_timerEnemies >= _intervalEnemies)
        {
            SpawnEnemies();
        }
    }

    public void SpawnBullet()
    {
        _poolerProjectile.GetPooledObject();
    }

    public void SpawnItems()
    {
        _poolerItems.GetPooledObject();
        if (_intervalItems < _intervalMaxItems)
        {
            _intervalItems++;
        }
        _timerItems = 0;
    }

    public void SpawnEnemies()
    {
        _poolerEnemies.GetPooledObject();
        if (_intervalEnemies > _intervalMinEnemies)
        {
            _intervalEnemies--;
        }
        _timerEnemies = 0;
    }
}