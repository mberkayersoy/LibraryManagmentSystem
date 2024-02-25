using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChunk : RunnerChunkBase
{
    [SerializeField] protected ObstacleDataSO[] _obstacleList;
    [SerializeField] protected BaseCollectible[] _collectibles;
    [SerializeField] protected int _maxObstacleCount;
    [SerializeField] protected float _obstaclePlacementChanceRate;
    [SerializeField, Range(0f,1f) ,Tooltip("Depending on whether collectibles will be created in the chunk or not.")]
    protected float _collectiblesChanceRate;
    protected const float X_RANGE = 2.5f;
    protected const float Y_POINT = 0f;
    protected List<PoolableComponent> _currentPoolObjects = new List<PoolableComponent>();
    protected Dictionary<float, List<(float, float)>> _safeAreaDictionary = new Dictionary<float, List<(float, float)>>();
    protected override void Awake()
    {
        base.Awake();
        _groundData = new GroundBounds(_collider);
    }

    protected void OnEnable()
    {
        _groundData.UpdateColliderData(_collider);
        PlaceObstacles();
    }

    protected void CalculateSafePath()
    {
        float[] lines = new float[3];
        for (int i = -1; i <= 1; i++)
        {
            float startX = i * X_RANGE;
            lines[i + 1] = startX;
            _safeAreaDictionary.Add(startX, new List<(float, float)>());
        }

        float start = _groundData.Min.z;
        float end = _groundData.Max.z;

        float currentLine = lines[Random.Range(0, lines.Length)]; // Keep track of the last line selected
        float[] nextLines = new float[2];
        while (start <= end)
        {
            float randomSpace = Random.Range(8f, 12f);
            if (start + randomSpace >= end)
            {
                _safeAreaDictionary[currentLine].Add((start, end));
                return;
            }

            _safeAreaDictionary[currentLine].Add((start, start + randomSpace));
            start += randomSpace;

            switch (currentLine)
            {
                case 0f:
                    nextLines[0] = -2.5f;
                    nextLines[1] = 2.5f;
                    currentLine = nextLines[Random.Range(0, nextLines.Length)];
                    break;
                case 2.5f:
                    nextLines[0] = -2.5f;
                    nextLines[1] = 0f;
                    currentLine = nextLines[Random.Range(0, nextLines.Length)];
                    break;
                case -2.5f:
                    nextLines[0] = 2.5f;
                    nextLines[1] = 0f;
                    currentLine = nextLines[Random.Range(0, nextLines.Length)];
                    break;
                default:
                    currentLine = lines[Random.Range(0, lines.Length)];
                    Debug.LogWarning("DEFAULT");
                    break;
            }
        }
    }

    protected void PlaceCollectibles()
    {
        if (Random.Range(0, 1f) <= _collectiblesChanceRate) 
        {
            foreach (var line in _safeAreaDictionary)
            {
                foreach (var range in line.Value)
                {
                    float start = range.Item1;
                    float end = range.Item2;
                    int numCollectibles = Mathf.FloorToInt((end - start) / 3f);
                    float step = (end - start) / numCollectibles;

                    for (int i = 1; i <= numCollectibles; i++)
                    {
                        Vector3 position = new Vector3(line.Key, Y_POINT, start + step * i);
                        var collectible = _poolManager.Spawn(_collectibles[0], position, default, transform);
                        _currentPoolObjects.Add(collectible);
                    }
                }
            }
        }
    }

    protected void PlaceObstacles()
    {
        CalculateSafePath();
        PlaceCollectibles();
        int placedObstacles = 0;

        List<(float, float)> currentLineSafeArea;

        for (int j = -1; j <= 1; j++)
        {
            float startX = j * X_RANGE;

            float minZ = _groundData.Min.z;
            float maxZ = _groundData.Max.z;
            currentLineSafeArea = _safeAreaDictionary[startX];

            while (minZ <= maxZ)
            {
                if (Random.Range(0f, 1f) <= _obstaclePlacementChanceRate)
                {
                    ObstacleDataSO _randomObstacle = _selector.GetRandomWithWeight(_obstacleList);
                    float obstacleSize = _randomObstacle.ColliderData.Size.z;

                    // Check if MinZ is within any of the safe areas
                    bool isInSafeArea = false;
                    foreach (var tuple in currentLineSafeArea)
                    {
                        if ((minZ >= tuple.Item1 || minZ + obstacleSize >= tuple.Item1) && minZ <= tuple.Item2)
                        {
                            isInSafeArea = true;
                            minZ = tuple.Item2;
                            break;
                        }
                    }
                    // If MinZ is within any safe area, skip placing obstacle
                    if (!isInSafeArea && minZ + obstacleSize <= _groundData.Max.z)
                    {
                        var newObstacle = _poolManager.Spawn(_randomObstacle.Obstacle, new Vector3(startX, Y_POINT, minZ), Quaternion.identity, transform);
                        minZ += obstacleSize + _randomObstacle.Space;
                        _currentPoolObjects.Add(newObstacle);
                        placedObstacles++;
                        if (placedObstacles >= _maxObstacleCount) return;
                    }
                }
                else
                {
                    // Can be customized here.
                    /*Such as increasing the chance of placing an obstacle for each obstacle not placed
                      and decreasing the chance of placing an obstacle for each obstacle placed...*/
                }
                minZ++;
            }
        }
    }

    public override void ResetPoolableObject()
    {
        base.ResetPoolableObject();
        for (int i = 0; i < _currentPoolObjects.Count; i++)
        {
            var item = _currentPoolObjects[i];
            _poolManager.Despawn(item);
        }
        _currentPoolObjects.Clear();
        _safeAreaDictionary.Clear();
    }
}