using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RunnerAnimationManager : MonoBehaviour
{
    public Action RollCompleted;
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private RunnerCollesionDetection _collesionDetection;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private int _animIDRoll;
    private int _animIDDead;
    private int _animIDActivated;

    private const string DIZZY_HEAD_LAYER = "DizzyHead";
    private const string DIZZY_ARMS_LAYER = "DizzyArms";
    private Tween headTween;
    private Tween armsTween;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _collesionDetection = GetComponentInParent<RunnerCollesionDetection>();
        _playerMovement.Activated += OnPlayerActivated;
        _playerMovement.Jumped += OnPlayerJumped;
        _playerMovement.SpeedChanged += OnPlayerSpeedChanged;
        _playerMovement.FreeFall += OnPlayerFreeFall;
        _playerMovement.GroundChanged += OnGroundStateChanged;
        _playerMovement.Rolled += OnRolled;
        _playerMovement.SideChanged += OnSideChanged;
        _playerMovement.PlayerRestarted += RestartAnimator;
        _collesionDetection.DizzyChanged += OnDizzyChanged;
        _collesionDetection.PlayerDied += OnPlayerDied;
    }
    private void Start()
    {
        AssignAnimationIDs();
    }
    private void OnRollCompleted(AnimationEvent animationEvent)
    {
        RollCompleted?.Invoke();
    }

    private void RestartAnimator()
    {
        _animator.Rebind();
    }

    private void OnPlayerActivated(bool isActivated)
    {
        _animator.SetBool(_animIDActivated, isActivated);
    }

    private void OnPlayerDied(bool isDead)
    {
        OnDizzyChanged(false);
        _animator.SetBool(_animIDDead, isDead);
    }

    private void OnDizzyChanged(bool isDizzy)
    {
        LayerWeightTimer(isDizzy);
    }

    private void LayerWeightTimer(bool isDizzy)
    {
        float targetWeight = isDizzy ? 1.0f : 0.0f;

        float currentHeadWeight = _animator.GetLayerWeight(_animator.GetLayerIndex(DIZZY_HEAD_LAYER));
        float currentArmsWeight = _animator.GetLayerWeight(_animator.GetLayerIndex(DIZZY_ARMS_LAYER));

        // Kill existing tweens if they are still running
        headTween?.Kill();
        armsTween?.Kill();

        headTween = DOVirtual.Float(currentHeadWeight, targetWeight, 0.5f, value =>
        {
            _animator.SetLayerWeight(_animator.GetLayerIndex(DIZZY_HEAD_LAYER), value);
        });

        armsTween = DOVirtual.Float(currentArmsWeight, targetWeight, 0.5f, value =>
        {
        _animator.SetLayerWeight(_animator.GetLayerIndex(DIZZY_ARMS_LAYER), value);
        });
    }

    private void OnSideChanged(float xInput)
    {
        _animator.SetTrigger("SideWayInput");
        _animator.SetFloat("XInput", xInput * Time.deltaTime);
    }

    private void OnRolled(bool isRolling)
    {
        _animator.SetBool(_animIDRoll, isRolling);
    }

    private void OnGroundStateChanged(bool isGrounded)
    {
        _animator.SetBool(_animIDGrounded, isGrounded);
    }

    private void OnPlayerFreeFall(bool isFreeFalling)
    {
        _animator.SetBool(_animIDFreeFall, isFreeFalling);
    }

    private void OnPlayerSpeedChanged(float speed)
    {
        _animator.SetFloat(_animIDMotionSpeed, speed * 0.1f);
    }
    private void OnPlayerJumped(bool isJumped)
    {
        _animator.SetBool(_animIDJump, isJumped);
    }

    private void AssignAnimationIDs()
    {
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDRoll = Animator.StringToHash("Roll");
        _animIDDead = Animator.StringToHash("Dead");
        _animIDActivated = Animator.StringToHash("Activate");
    }
    private void OnDestroy()
    {
        _playerMovement.Activated -= OnPlayerActivated;
        _playerMovement.Jumped -= OnPlayerJumped;
        _playerMovement.SpeedChanged -= OnPlayerSpeedChanged;
        _playerMovement.FreeFall -= OnPlayerFreeFall;
        _playerMovement.GroundChanged -= OnGroundStateChanged;
        _playerMovement.Rolled -= OnRolled;
        _playerMovement.SideChanged -= OnSideChanged;
        _collesionDetection.DizzyChanged -= OnDizzyChanged;
        _collesionDetection.PlayerDied -= OnPlayerDied;
    }
}
