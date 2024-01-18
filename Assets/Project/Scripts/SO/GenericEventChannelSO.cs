using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericEventChannelSO<T> : ScriptableObject
{
    public Action<T> OnEventRaised;

    public void RaiseEvent(T parameter)
    {
        if (OnEventRaised == null)
            return;

        OnEventRaised.Invoke(parameter);
    }
}