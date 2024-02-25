using System;
using UnityEngine;

[SelectionBase]
public abstract class RunnerChunkBase : PoolableComponent, IRandomSelectedWithWeight
{
    public Action<float> ActivateNextPath;
    public Action<RunnerChunkBase> DeActivateThisPath;

    [SerializeField] protected GroundBounds _groundData;
    [SerializeField] protected PlayerTriggerDetectionBase _entered;
    [SerializeField] protected PlayerTriggerDetectionBase _exit;
    [SerializeField] protected float _weight;
    protected BoxCollider _collider;
    protected PoolManager _poolManager;
    protected SelectorWithWeight<ObstacleDataSO> _selector = new SelectorWithWeight<ObstacleDataSO>();
    public float Weight => _weight;

    protected virtual void Awake()
    {
        _poolManager = PoolManager.Instance;
        _collider = GetComponent<BoxCollider>();
    }

    protected virtual void Start()
    {
        _entered.PlayerTriggered += OnActivateNextPath;
        _exit.PlayerTriggered += OnDeActivateThisPath;
    }
    protected virtual void OnActivateNextPath()
    {
        ActivateNextPath?.Invoke(transform.position.z + _collider.size.z);
    }
    protected virtual void OnDeActivateThisPath()
    {
        DeActivateThisPath?.Invoke(this);
    }
    protected virtual void OnDestroy()
    {
        _entered.PlayerTriggered -= OnActivateNextPath;
        _exit.PlayerTriggered -= OnDeActivateThisPath;
    }
}