using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private float SprintSpeed = 30;
    private float MoveSpeed = 5;
    private float JumpHeight = 2;
    private float Gravity = -15;

    private bool _isSprinting;
    private float _stamina = 5; // 10 sec of running
    private float _staminaRegenCooldown = 2; // 2 secs of not running before regaining
    private float _staminaRegenCounter = 0;
    private float _staminaUseRate = 1; // 1/sec
    private float _staminaRegenRate = 0.5f; // 0.5/sec


    private bool _isGrounded;
    private float _verticalVelocity;

    private Vector2 _moveInput;
    private bool _jumpInput;

    private CharacterController _controller;
    private float speedChangeRate = 5;
    private float _maxStamina = 10;

    private float GroundedOffset = 0.1f;
    private float GroundedRadius = 1f;
    public LayerMask GroundLayers;


    private bool _isDead = false;
    private int _playerHP = 1; // over 0 by default



    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDead)
        {
            _staminaRegenCounter += Time.deltaTime;
            MovePlayer();
            StaminaRegeneration();
            CheckGrounded();
            JumpAndGravity();
            CheckLife();
            //print(_stamina);
        }
        else
        {
            transform.eulerAngles = new Vector3(0,0,90);
        }
    }

    private void CheckLife()
    {
        if(_playerHP<= 0 )
        {
            _isDead = true;
        }
    }

    private void MovePlayer()
    {
        float targetSpeed;
        if (_stamina >= 0 && _isSprinting)
        {
             targetSpeed = SprintSpeed;
            _stamina -= _staminaUseRate * Time.deltaTime;
            _staminaRegenCounter = 0;
        }
        else
        {
            targetSpeed = MoveSpeed;
        }

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
        _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + _verticalVelocity * Vector3.up * Time.deltaTime);
    }

    private void StaminaRegeneration()
    {

        if(!_isSprinting && _staminaRegenCounter >= _staminaRegenCooldown && _stamina < _maxStamina)
        {
            
            _stamina += _staminaRegenRate * Time.deltaTime;
        }
    }
    private void CheckGrounded()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        _isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        print(_isGrounded);
    }
    private void JumpAndGravity()
    {
        if (_isGrounded)
        {
            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = 0;
            }

            // Jump
            if (_jumpInput)
            {
                print("change velocity");
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity); // Gravity has to be negative for this to work
                _jumpInput = false; 
            }

        }
        else
        {
            _verticalVelocity += Gravity * Time.deltaTime; // Gravity has to be negative for this to work
        }
       
    }
        public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        _jumpInput = value.isPressed;
        print("jump");
    }

    public void OnSprint(InputValue value)
    {
        if(value.isPressed)
        {
            _isSprinting = true;
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
