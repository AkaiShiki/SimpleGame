using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private float SprintSpeed = 30;
    private float MoveSpeed = 5;

    private bool _isSprinting;
    private float _stamina = 10; // 10 sec of running
    private float _staminaRegenCooldown = 2; // 2 secs of not running before regaining
    private float _staminaRegenCounter = 0;
    private float _staminaUseRate = 1; // 1/sec
    private float _staminaRegenRate = 0.5f; // 0.5/sec


    private bool _isGrounded;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private float speedChangeRate = 5;
    private float _maxStamina = 10;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _staminaRegenCounter += Time.deltaTime;
        MovePlayer();
        StaminaRegeneration();
    }
    
    private void MovePlayer()
    {
        float targetSpeed = _isSprinting ? SprintSpeed : MoveSpeed;

        if (_moveInput == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float _speed;
        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
             _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * speedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
             _speed = targetSpeed;
        }
        Vector3 inputDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;
        _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime)); //+ _verticalVelocity * Vector3.up * Time.deltaTime);
    }

    private void StaminaRegeneration()
    {
        print(_stamina);
        if(!_isSprinting && _staminaRegenCounter >= _staminaRegenCooldown && _stamina <= _maxStamina) // %% stamina regen cooldown < counter 
        {
            print("regenerating");
            _stamina += _staminaRegenRate * Time.deltaTime;
        }
    }
    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if(_isGrounded)
        {
            //jump
            _isGrounded = false;
        }
    }

    public void OnSprint(InputValue value)
    {
        if(_stamina >= 0 && value.isPressed)
        {
            _isSprinting = true;
            _stamina -= _staminaUseRate * Time.deltaTime;
            _staminaRegenCounter = 0;
        }
        else
        {
            _isSprinting = false;

        }
        
    }

    public void OnShoot(InputValue value)
    {

    }

}
