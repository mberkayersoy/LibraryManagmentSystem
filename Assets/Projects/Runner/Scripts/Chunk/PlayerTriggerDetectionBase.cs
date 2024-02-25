using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerTriggerDetectionBase : MonoBehaviour
{
    public event Action PlayerTriggered;

    protected void OnPlayerTriggered()
    {
        PlayerTriggered?.Invoke();
    }
}
