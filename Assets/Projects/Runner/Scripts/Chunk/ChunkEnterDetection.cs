using System;
using UnityEngine;

public class ChunkEnterDetection : PlayerTriggerDetectionBase
{
    public Action PlayerEntered;
    [SerializeField] private VoidEventChannelSO _increasePlayerSpeed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RunnerCollesionDetection player))
        {
            OnPlayerTriggered();
            _increasePlayerSpeed.RaiseEvent();
        }
    }
}
