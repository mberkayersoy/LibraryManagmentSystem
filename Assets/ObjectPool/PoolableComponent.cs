using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PoolableComponent : MonoBehaviour
{
    public event Action<PoolableComponent> ResettedPoolObject;
    public virtual void ResetPoolableObject()
    {
        ResettedPoolObject?.Invoke(this);
    }
}
