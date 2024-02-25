using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCollectible : PoolableComponent
{
    [SerializeField, Range(0f, 1f)] private float _chanceRate;
    protected PoolManager _poolManager;
    protected abstract void Apply();
    private void Awake()
    {
        _poolManager = PoolManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out RunnerCollesionDetection player))
        {
            Apply();
            gameObject.SetActive(false);
        }
    }
}
