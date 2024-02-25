using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField] private PoolableContainer _objectPoolPrefab;
    private readonly Dictionary<int, PoolableContainer> _prefabToPoolableContainerDictionary = new Dictionary<int, PoolableContainer>();
    private readonly Dictionary<int, int> _instanceToPrefabsDictionary = new Dictionary<int, int>();
    private void Awake()
    {
        Instance = this;
        InitializeDictionary();
    }

    protected void InitializeDictionary()
    {
        _prefabToPoolableContainerDictionary.Clear();
        _instanceToPrefabsDictionary.Clear();
    }

    public T Spawn<T>(T prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : PoolableComponent
    {
        var pool = GetOrCreatePoolableContainer(prefab);
        var obj = pool.Spawn(prefab, position, rotation, parent);
        _instanceToPrefabsDictionary.TryAdd(obj.gameObject.GetHashCode(), prefab.gameObject.GetHashCode());
        return obj;
    }

    public void Despawn<T>(T instance) where T : PoolableComponent
    {
        var pool = FindPoolableContainerFromInstance(instance);
        if (pool != null)
        {
            pool.Despawn(instance);
        }
    }

    public void AddNewPoolableContainer(PoolableComponent prefab, PoolableContainer pool)
    {
        if (pool == null || prefab == null)
            return;

        _prefabToPoolableContainerDictionary[prefab.gameObject.GetHashCode()] = pool;
    }

    private PoolableContainer GetOrCreatePoolableContainer<T>(T prefab) where T : PoolableComponent
    {
        if (!_prefabToPoolableContainerDictionary.TryGetValue(prefab.gameObject.GetHashCode(), out var pool))
        {
            pool = CreateNewPoolableContainer(prefab.name);
            AddNewPoolableContainer(prefab, pool);
        }
        return pool;
    }

    private PoolableContainer CreateNewPoolableContainer(string name)
    {
        var pool = Instantiate(_objectPoolPrefab);
        pool.name = "Pool_" + name;// $"Pool_{name}";
        return pool;
    }

    private PoolableContainer FindPoolableContainerFromInstance<T>(T instance) where T : PoolableComponent
    {
        _instanceToPrefabsDictionary.TryGetValue(instance.gameObject.GetHashCode(), out var prefabHash);
        if (prefabHash == 0)
        {
            Debug.LogError("Hash can not be found!");
            return null;
        }
        _prefabToPoolableContainerDictionary.TryGetValue(prefabHash, out var pool);
        if (pool != null)
            return pool;
        Debug.LogError("Pool can not be found!");
        return null;
    }
}
