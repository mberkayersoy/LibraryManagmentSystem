using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : PoolableComponent
{
    [SerializeField] private ObstacleType _type;
    public ObstacleType Type { get => _type; }
}

[System.Serializable]   
public enum ObstacleType
{
    Passable,
    Impassable
}