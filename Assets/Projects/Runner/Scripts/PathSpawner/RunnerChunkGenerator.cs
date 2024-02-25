using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RunnerChunkGenerator : MonoBehaviour
{
    protected PoolManager _poolManager;
    [SerializeField] private RunnerChunkBase[] _chunkPrefabs;
    [SerializeField] private bool _shouldGenerate;
    [SerializeField] private List<RunnerChunkBase> _activeChunks;
    [SerializeField] private VoidEventChannelSO _gameStarted;
    [SerializeField] private VoidEventChannelSO _gameReStarted;

    private SelectorWithWeight<RunnerChunkBase> _selector = new SelectorWithWeight<RunnerChunkBase>();
    private void Awake()
    {
        _gameReStarted.OnEventRaised += DeSpawnAllChunks;
        _gameStarted.OnEventRaised += StartChunks;
    }

    private void Start()
    {
        _poolManager = PoolManager.Instance;
    }
    private void StartChunks()
    {
        if (!_shouldGenerate) return;
        var chunk = _poolManager.Spawn(_chunkPrefabs[0], Vector3.zero, Quaternion.identity, null);
        RegisterChunk(chunk);

    }

    private void DeActivateChunk(RunnerChunkBase chunk)
    {
        UnRegisterChunk(chunk);
        _activeChunks.Remove(chunk);
        _poolManager.Despawn(chunk);
    }

    private void ActivateNewChunk(float zPosition)
    {
        var chunk = _poolManager.Spawn(_selector.GetRandomWithWeight(_chunkPrefabs), new Vector3(0, 0, zPosition), Quaternion.identity, null);
        RegisterChunk(chunk);
    }

    private void RegisterChunk(RunnerChunkBase chunk)
    {
        _activeChunks.Add(chunk);
        chunk.ActivateNextPath += ActivateNewChunk;
        chunk.DeActivateThisPath += DeActivateChunk;
    }
    private void UnRegisterChunk(RunnerChunkBase chunk)
    {
        chunk.ActivateNextPath -= ActivateNewChunk;
        chunk.DeActivateThisPath -= DeActivateChunk;
    }

    private void DeSpawnAllChunks()
    {
        foreach (var chunk in _activeChunks)
        {
            _poolManager.Despawn(chunk);
            chunk.ActivateNextPath -= ActivateNewChunk;
            chunk.DeActivateThisPath -= DeActivateChunk;
        }
        _activeChunks.Clear();
    }
    private void OnDestroy()
    {
        _gameStarted.OnEventRaised -= StartChunks;
        _gameReStarted.OnEventRaised -= DeSpawnAllChunks;
    }
}
