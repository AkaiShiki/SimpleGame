using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private FloatVariable Stamina;
    [SerializeField] private FloatVariable PlayerHP;
    [SerializeField] private LayerMask GroundLayers;
    [SerializeField] private Spawner Spawner;

    [SerializeField] private float SprintSpeed = 30;
    [SerializeField] private float MoveSpeed = 5;
    [SerializeField] private float JumpHeight = 2;
    [SerializeField] private float Gravity = -15;

    private bool _isSprinting;
    [SerializeField] private float _staminaRegenCooldown = 2; // 2 secs of not running before regaining
    private float _staminaRegenCounter = 0;
    [SerializeField] private float _staminaUseRate = 1; // 1/sec
    [SerializeField] private float _staminaRegenRate = 0.5f; // 0.5/sec


    private bool _isGrounded;
    private float _verticalVelocity;

    private Vector2 _moveInput;
    private bool _jumpInput;

    private CharacterController _controller;
    [SerializeField] private float speedChangeRate = 5;
    [SerializeField] int _maxHP = 100;
    [SerializeField] float _maxStamina = 10f;

    private float GroundedOffset = 0.1f;
    private float GroundedRadius = 1f;


    private bool _isDead = false;



    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Stamina.Value = _maxStamina;
        PlayerHP.Value = _maxHP;
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
        if(PlayerHP.Value<= 0 )
        {
            _isDead = true;
        }
    }

    private void MovePlayer()
    {
        float targetSpeed;
        if (Stamina.Value >= 0 && _isSprinting)
        {
             targetSpeed = SprintSpeed;
            Stamina.Value -= _staminaUseRate * Time.deltaTime;
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

        if(!_isSprinting && _staminaRegenCounter >= _staminaRegenCooldown && Stamina.Value < _maxStamina)
        {
            
            Stamina.Value += _staminaRegenRate * Time.deltaTime;
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
        Vector3 clickPostion = Input.mousePosition;
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = (clickPostion - playerPosition); // direction with 0 in the y axis
        Vector3 worldDir = new Vector3(dir.x, 0f, dir.y);
        print(worldDir);
        Spawner.SpawnBullet(transform.position, dir.normalized);
    }

}
