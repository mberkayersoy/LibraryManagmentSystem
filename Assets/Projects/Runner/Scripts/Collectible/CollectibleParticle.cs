using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleParticle : PoolableComponent
{

    public void OnParticleSystemStopped()
    {
        PoolManager.Instance.Despawn(this);
    }
}
