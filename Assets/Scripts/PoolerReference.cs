using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerReference : MonoBehaviour
{
    [HideInInspector] public Pooler _pooler;

    private void OnDisable()
    {
        _pooler.AddObjectToDisabledList(gameObject);
    }
}
