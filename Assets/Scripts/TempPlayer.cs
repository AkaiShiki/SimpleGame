using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempPlayer : MonoBehaviour
{
    [SerializeField] private InputActionReference _click;
    [SerializeField] private Spawner _spawner;

    // Start is called before the first frame update
    void Start()
    {
        _click.action.Enable();
        _click.action.performed += Shoot;
    }

    private void Shoot(InputAction.CallbackContext obj)
    {
        _spawner.SpawnBullet();
    }
}
