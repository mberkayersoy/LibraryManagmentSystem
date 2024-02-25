using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkExitDetection : PlayerTriggerDetectionBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RunnerCollesionDetection player))
        {
            StartCoroutine(OnPlayerExit());
        }
    }

    private IEnumerator OnPlayerExit()
    {
        yield return new WaitForSeconds(0.2f);
        OnPlayerTriggered();
    }
}
