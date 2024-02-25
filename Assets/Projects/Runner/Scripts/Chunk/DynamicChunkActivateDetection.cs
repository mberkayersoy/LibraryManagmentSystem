using UnityEngine;

public class DynamicChunkActivateDetection : PlayerTriggerDetectionBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RunnerCollesionDetection player))
        {
            OnPlayerTriggered();
        }
    }
}
