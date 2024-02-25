using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Action<bool> GroundStateChanged;
    public Action<Vector3> GroundSlopeChanged;
    private bool _isGrounded;
    [SerializeField] private float _threshold;
    [SerializeField] private float _yOffSet;
    [SerializeField] private float _raycastDistance;
    [SerializeField] private LayerMask _groundLayer;
    private Vector3 _slopeVector;

    public LayerMask GroundLayer { get => _groundLayer; set => _groundLayer = value; }

    private void Update()
    {
        CheckGround();
        CheckSlope();
    }
    private void CheckSlope()
    {
        Debug.DrawRay(transform.position + transform.forward / 6 + new Vector3(0, _yOffSet, 0), -transform.up * _raycastDistance, Color.red);
        if (Physics.Raycast(transform.position /*+ transform.forward / 6*/ + new Vector3(0, _yOffSet, 0), -transform.up, out RaycastHit hit, _raycastDistance, _groundLayer))
        {
            _slopeVector = Vector3.Cross(hit.normal, Vector3.Cross(Vector3.up, hit.normal)).normalized;
        }
        else
        {
            _slopeVector = Vector3.forward;
        }

        GroundSlopeChanged?.Invoke(_slopeVector);
    }

    private void CheckGround()
    {
        if (Physics.Raycast(transform.position, -transform.up + new Vector3(0, _yOffSet, 0), out RaycastHit hit, _raycastDistance, _groundLayer))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);
            if (hit.distance <= _threshold)
            {
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            } 
        }
        else
        {
            _isGrounded = false;
        }

        GroundStateChanged?.Invoke(_isGrounded);
    }
}
