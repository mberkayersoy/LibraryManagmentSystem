using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Action<bool> Activated;
    public Action<bool> Jumped;
    public Action<float> SpeedChanged;
    public Action<bool> FreeFall;
    public Action<bool> GroundChanged;
    public Action<bool> Rolled;
    public Action<float> SideChanged;
    public Action PlayerRestarted;
    public Action ObstacleHit;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _sidewaysSpeed;
    [SerializeField] private float _sideWaySmoothTime;
    [SerializeField] private float _rotationSpeed;
    [Tooltip("The character uses its own gravity value")]
    [SerializeField] private float _gravityMultiplier;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    [SerializeField] private float _fallTimeout = 0.15f;
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    [SerializeField] private float _jumpTimeout = 0.5f;
    [SerializeField] private float _sideMoveRange = 2.5f;
    [SerializeField] private Transform _playerModel;

    [Header("Broadcast Events")]
    [SerializeField] private IntEventChannelSO _playerScore;
    [SerializeField] private VoidEventChannelSO _playerDied;
    [Header("Listening Events")]
    [SerializeField] private VoidEventChannelSO _gameStarted;
    [SerializeField] private VoidEventChannelSO _gameReStarted;
    [SerializeField] private VoidEventChannelSO _increaseSpeed;
    private RunnerPlayerInput _input;
    private GroundCheck _groundCheck;
    private CharacterController _cc;
    private RunnerAnimationManager _animationManager;
    private RunnerCollesionDetection _collesionDetection;
    private bool _isGameActive;
    private float _sidewayInput;
    private bool _isGronuded;
    private bool _isJumping;
    private bool _isRolling;
    private bool _isDead;
    private const float DEFAULT_SPEED = 12f;
    private float _airToGronudMultiplier = -5f;
    private float _verticalVelocity = 0f;
    private float _terminalVelocity = 53.0f;
    private float _fallTimeoutDelta;
    private float _jumpTimeoutDelta;
    private float _targetXPos;
    private float _lastXPos;

    private Vector3 _startPosition;
    private void Awake()
    {
        _input = GetComponent<RunnerPlayerInput>();
        _groundCheck = GetComponent<GroundCheck>();
        _cc = GetComponent<CharacterController>();
        _animationManager = GetComponentInChildren<RunnerAnimationManager>();
        _collesionDetection = GetComponent<RunnerCollesionDetection>();
        _input.InputChanged += InputManager_InputChanged;
        _groundCheck.GroundStateChanged += GronudCheck_GroundStateChanged;
        _groundCheck.GroundSlopeChanged += SetRotation;
        _collesionDetection.ObstacleHit += ChangeTargetPosition;
        _collesionDetection.PlayerDied += StopMovement;
        _animationManager.RollCompleted += AnimationManager_OnRollCompleted;
        _gameStarted.OnEventRaised += ActivatePlayer;
        _gameReStarted.OnEventRaised += RestartPlayer;
        _increaseSpeed.OnEventRaised += IncreaseSpeed;
        _startPosition = transform.position;
    }

    private void Start()
    {
        SpeedChanged?.Invoke(_moveSpeed);
    }
    private void Update()
    {
        if (!_isGameActive) return;
        JumpAndGravity();
        Move();
    }

    private void Move()
    {
        Vector3 moveDirection = transform.forward * _moveSpeed * Time.deltaTime +
                            Vector3.up * _verticalVelocity * Time.deltaTime;

        moveDirection += CalculateHorizontalMovement();
        _cc.Move(moveDirection);
        _playerScore.RaiseEvent((int)transform.position.z);
    }

    private void IncreaseSpeed()
    {
        _moveSpeed += 0.5f;
        SpeedChanged?.Invoke(_moveSpeed);

    }
    private void StopMovement(bool isDead)
    {
        _isDead = isDead;
        _isGameActive = !isDead;
        _playerDied.RaiseEvent();

    }
    private Vector3 CalculateHorizontalMovement()
    {
        float _currentXPos = transform.position.x;

        if (Mathf.Approximately(_currentXPos, _targetXPos))
        {
            _lastXPos = _currentXPos;
            _targetXPos = Mathf.Clamp(_currentXPos + _sidewayInput * _sideMoveRange,
                                        -_sideMoveRange, _sideMoveRange);
        }
        float newXPos = Mathf.MoveTowards(_currentXPos, _targetXPos, _sidewaysSpeed * Time.deltaTime);

        return new Vector3(newXPos - _currentXPos, 0, 0);
    }

    private void ChangeTargetPosition()
    {
        if (_targetXPos == _lastXPos)
        {
            StartCoroutine(SetCollesionDetecstion());
        }
        else
        {
            _targetXPos = _lastXPos;
            _collesionDetection.IsHitObstacle = false;
        }
    }

    private IEnumerator SetCollesionDetecstion()
    {
        yield return new WaitForSeconds(0.15f);
        _collesionDetection.IsHitObstacle = false;
    }

    private void JumpAndGravity()
    {
        if (_isGronuded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = _fallTimeout;

            Jumped?.Invoke(false);
            FreeFall?.Invoke(false);

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            if (_isJumping & _jumpTimeoutDelta <= 0.0f)
            {      // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(_jumpSpeed * -2f * _gravityMultiplier);

                Jumped?.Invoke(true);
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = _jumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                FreeFall?.Invoke(true);

            }
            Jumped?.Invoke(false);
            _isJumping = false;

        }
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += _gravityMultiplier * Time.deltaTime;
        }
    }
    private void InputManager_InputChanged(Vector2 input)
    {
        if (!_isGameActive) return;

        if (_collesionDetection.IsHitObstacle || _isDead) return;

        if (_isGronuded)
        {
            if (input.y >= 1)
            {
                _isJumping = true;
                SetColliderHeight(false);
            }
        }

        if (input.y <= -1)
        {
            SetColliderHeight(true);
            CalculateDownSpeed();
        }


        _sidewayInput = input.x;

        SideChanged?.Invoke(_sidewayInput);
    }

    private void SetColliderHeight(bool isRolling)
    {
        _isRolling = isRolling;
        Rolled?.Invoke(_isRolling);
        if (_isRolling)
        {
            _cc.height = 0.8f;
            _cc.center = Vector3.up * 0.4f;
        }
        else
        {
            _cc.height = 1.6f;
            _cc.center = Vector3.up * 0.8f;
        }
    }

    private void SetRotation(Vector3 slopeVector)
    {
        if (slopeVector.z >= 0f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(slopeVector);
            _playerModel.rotation = Quaternion.Slerp(_playerModel.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
    private void ActivatePlayer()
    {
        _isGameActive = true;
        Activated?.Invoke(_isGameActive);
    }
    private void RestartPlayer()
    {
        transform.position = _startPosition;
        _targetXPos = 0f;
        _lastXPos = 0f;
        _sidewayInput = 0f;
        _moveSpeed = DEFAULT_SPEED;
        _isDead = false;
        PlayerRestarted?.Invoke();
        SpeedChanged?.Invoke(_moveSpeed);
    }

    private void CalculateDownSpeed()
    {
        if (!_isGronuded && Physics.Raycast(transform.position + Vector3.up * 0.05f, Vector3.down, out RaycastHit hit, _groundCheck.GroundLayer))
        {
            _verticalVelocity = hit.distance * _airToGronudMultiplier;
        }
    }
    private void AnimationManager_OnRollCompleted()
    {
        SetColliderHeight(false);
    }
    private void GronudCheck_GroundStateChanged(bool isGronuded)
    {
        _isGronuded = isGronuded;
        GroundChanged?.Invoke(isGronuded);
    }

    private void OnDestroy()
    {
        _input.InputChanged -= InputManager_InputChanged;
        _groundCheck.GroundStateChanged -= GronudCheck_GroundStateChanged;
        _groundCheck.GroundSlopeChanged -= SetRotation;
        _collesionDetection.ObstacleHit -= ChangeTargetPosition;
        _collesionDetection.PlayerDied -= StopMovement;
        _animationManager.RollCompleted -= AnimationManager_OnRollCompleted;
        _gameStarted.OnEventRaised -= ActivatePlayer;
        _gameReStarted.OnEventRaised -= RestartPlayer;
    }
}