using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleSO", menuName = "Runner/Obstacle Data")]
public class ObstacleDataSO : ScriptableObject, IRandomSelectedWithWeight
{
    public Obstacle Obstacle;
    public ObstacleBounds ColliderData;
    public float Space;
    [Range(0f,1f)] public float ChanceWeight;
    public float Weight => ChanceWeight;
}
