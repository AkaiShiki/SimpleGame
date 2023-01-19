using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolerProjectile : Pooler
{
    [SerializeField] private float _offset = 0.5f;
    [SerializeField] private Transform _player;

    override protected Vector3 SetPosition()
    {
        Vector3 positionObjectToSpawn = new Vector3(_player.position.x + _offset, _player.position.y, _player.position.z);

        return positionObjectToSpawn;
    }
}
