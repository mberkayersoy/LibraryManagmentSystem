using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerCollesionDetection : MonoBehaviour
{
    public Action ObstacleHit;
    public Action<bool> DizzyChanged;
    public Action<bool> PlayerDied;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private CameraShakeSourceSO _obstacleHitShake;
    [SerializeField] private float _dizzyTime;
    [SerializeField] private ParticleSystem _injuredEffect;
    private float _dizzyTimeoutDelta;
    private bool _isDizzy;
    private bool _isHitObstacle;
    public bool IsHitObstacle { get => _isHitObstacle; set => _isHitObstacle = value; }
    private void Update()
    {
        if (_isDizzy)
        {
            _dizzyTimeoutDelta -= Time.deltaTime;

            if (_dizzyTimeoutDelta <= 0)
            {
                _isDizzy = false;
                DizzyChanged?.Invoke(_isDizzy);
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_isHitObstacle) return;
        
        if (((1 << hit.gameObject.layer) & _obstacleLayer) != 0)
        {
            if (_isDizzy)
            {
                _obstacleHitShake.Shake();
                PlayerDied?.Invoke(true);
                return;
            }
            _obstacleHitShake.Shake();
            _isHitObstacle = true;
            _isDizzy = true;
            _injuredEffect.Play();
            ObstacleHit?.Invoke();
            DizzyChanged?.Invoke(_isDizzy);
            _dizzyTimeoutDelta = _dizzyTime;
        }
    }
}
