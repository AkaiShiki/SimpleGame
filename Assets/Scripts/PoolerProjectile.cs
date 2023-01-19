using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolerProjectile : Pooler
{
    [SerializeField] private float _offset = 0.5f;
    //    [SerializeField] private Transform _player;

    protected Vector3 SetPosition(Vector3 playerPosition, Vector3 bulletDirection)
    {
        Vector3 positionObjectToSpawn = new Vector3(playerPosition.x + bulletDirection.x * _offset,
                                                    playerPosition.y,
                                                    playerPosition.z + bulletDirection.z * _offset);

        return positionObjectToSpawn;
    }

    public GameObject GetPooledObject(Vector3 playerPosition, Vector3 bulletDirection)
    {
        GameObject objectSpawn = new GameObject();

        if (disabledObjects.Count > 0)
        {
            disabledObjects[0].transform.position = SetPosition(playerPosition, bulletDirection);
            objectSpawn = disabledObjects[0];
            Vector3 directionOfBullet = objectSpawn.transform.position + bulletDirection;

            objectSpawn.transform.rotation = Quaternion.LookRotation(directionOfBullet, Vector3.up);
            disabledObjects[0].SetActive(true);
            disabledObjects.RemoveAt(0);
        }
        else Debug.Log(transform.name + " n'a plus d'objets inactifs à faire spawner.");

        return objectSpawn;
    }
}
