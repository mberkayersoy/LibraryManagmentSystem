using System.Collections.Generic;
using UnityEngine;

public class PoolableContainer : PoolableComponent
{
    private readonly Queue<PoolableComponent> _poolObjects = new Queue<PoolableComponent>();

    public T Spawn<T>(T obj, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : PoolableComponent
    {
        var poolObject = GetObjectFromContainer();
        if (poolObject == null)
            poolObject = Instantiate(obj, position, rotation, parent);
        else
            Activate(poolObject, position, rotation, parent);
        return poolObject.GetComponent<T>();
    }

    public void Despawn(PoolableComponent objectToDespawn)
    {
        DeActivate(objectToDespawn);
        _poolObjects.Enqueue(objectToDespawn);
    }

    private PoolableComponent GetObjectFromContainer()
    {
        return _poolObjects.Count > 0 ? _poolObjects.Dequeue() : null;
    }

    private void DeActivate(PoolableComponent obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        obj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        obj.ResetPoolableObject();
    }

    private void Activate(PoolableComponent obj, Vector3 position, Quaternion rotation, Transform parent)
    {
        obj.transform.SetParent(null);

        if (parent != null) obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(true);
    }
}
