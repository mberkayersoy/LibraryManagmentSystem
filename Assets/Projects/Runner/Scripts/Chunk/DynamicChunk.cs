using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicChunk : RandomChunk
{
    [SerializeField] private PlayerTriggerDetectionBase _activated;

    protected override void Awake()
    {
        base.Awake();
        _activated.PlayerTriggered += ActivateObstacles;
    }

    private void ActivateObstacles()
    {
        foreach (var item in _currentPoolObjects)
        {
            if (item is DynamicObstacle dynamicObstacle)
            {
                dynamicObstacle.Activate(true);
            }
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _activated.PlayerTriggered -= ActivateObstacles;
    }
}
