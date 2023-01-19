using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] GameObject[] _poolers;
        
    private Pooler[] _instantiatedPoolers;
         
    [SerializeField] private float _spawnInterval;

    private float _time = 0;


    private void Start()
    {        
        _instantiatedPoolers = new Pooler[_poolers.Length];

        for (int i = 0; i < _poolers.Length; i++)
        {
            GameObject pooler = Instantiate(_poolers[i], transform.position, Quaternion.identity);
            pooler.transform.parent = transform;

            _instantiatedPoolers[i] = pooler.GetComponent<Pooler>();
        }
    }


    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _spawnInterval)
        {
            int randomSpooler = Random.Range(0, _poolers.Length);
            _instantiatedPoolers[randomSpooler].GetPooledObject();
            _time = 0;
        }
    }
}