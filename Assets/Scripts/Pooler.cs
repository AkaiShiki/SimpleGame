using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pooler : MonoBehaviour
{
    protected List<GameObject> disabledObjects;

    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int _amountToPool = 20;
    [SerializeField] private float _distanceMin;
    [SerializeField] private float _distanceMax;
    [SerializeField] private float _distanceY;

    void Start()
    {
        disabledObjects = new List<GameObject>();

        if (_amountToPool < 1) _amountToPool = 1;

        for (int i = 0; i < _amountToPool; i++)
        {
            InstantiateGameObject();
        }
    }

    private void InstantiateGameObject()
    {
        GameObject objectToAdd = Instantiate(objectToPool);
        //objectToAdd.transform.parent = transform;
        objectToAdd.AddComponent<PoolerReference>();
        objectToAdd.GetComponent<PoolerReference>()._pooler = this;

        objectToAdd.transform.position = SetPosition();

        objectToAdd.SetActive(false);
    }

    virtual protected Vector3 SetPosition()
    {
        float randomDistanceX = Random.Range(_distanceMin, _distanceMax);
        float randomDistanceZ = Random.Range(_distanceMin, _distanceMax);
        Vector3 positionObjectToSpawn = new Vector3(randomDistanceX, _distanceY, randomDistanceZ);

        return positionObjectToSpawn;
    }


    public void AddObjectToDisabledList(GameObject objectToAdd)
    {
        disabledObjects.Add(objectToAdd);
    }


    virtual public void GetPooledObject()
    {
        
        if (disabledObjects.Count > 0)
        {
            disabledObjects[0].transform.position = SetPosition();
            disabledObjects[0].SetActive(true);
            disabledObjects.RemoveAt(0);
        }
        else Debug.Log(transform.name + " n'a plus d'objets inactifs à faire spawner.");
    }
}