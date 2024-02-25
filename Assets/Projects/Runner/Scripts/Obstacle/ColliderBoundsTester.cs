using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBoundsTester : MonoBehaviour
{
    public ObstacleBounds ColliderData;

    [SerializeField] private Collider[] collider;

    private void Awake()
    {
        ColliderData = new ObstacleBounds(collider);
    }
}
public abstract class ColliderBounds
{
    public Vector3 Min; 
    public Vector3 Max;
    public Vector3 Center;
    public Vector3 Size;

    public void CalculateColliderBounds(Collider collider)
    {
        if (collider != null)
        {
            Size = collider.bounds.size;
            Size.x = Mathf.Round(Size.x);
            Size.y = Mathf.Round(Size.y);
            Size.z = Mathf.Round(Size.z);

            if (Size.z <= 0.5f) Size.z = 0.5f;

            Center = collider.bounds.center;
            Center.x = Mathf.Round(Center.x);
            Center.y = Mathf.Round(Center.y);
            Center.z = Mathf.Round(Center.z);

            Min = new Vector3(Center.x - Size.x / 2f, Center.y - Size.y / 2f, Center.z - Size.z / 2f);
            Max = new Vector3(Center.x + Size.x / 2f, Center.y + Size.y / 2f, Center.z + Size.z / 2f);
        }
    }
}
[System.Serializable]
public class GroundBounds : ColliderBounds
{
    private float AvoidAreaPer = 0.1f;
    public GroundBounds(Collider collider, float avoidAreaPer = 0.1f)
    {
        AvoidAreaPer = avoidAreaPer;
        CalculateColliderBounds(collider);
    }
    public void UpdateColliderData(Collider collider)
    {
        if (collider != null)
        {
            Size = collider.bounds.size;
            //Size.x = Mathf.Round(Size.x);
            //Size.y = Mathf.Round(Size.y);
            //Size.z = Mathf.Round(Size.z);

            if (AvoidAreaPer <= 0) AvoidAreaPer = 0.1f;
            Size.z -= Size.z * AvoidAreaPer;
            Center = collider.bounds.center;
            //Center.x = Mathf.Round(Center.x);
            //Center.y = Mathf.Round(Center.y);
            //Center.z = Mathf.Round(Center.z);

            Min = new Vector3(Center.x - Size.x / 2f, Center.y - Size.y / 2f, Center.z - Size.z / 2f);
            Max = new Vector3(Center.x + Size.x / 2f, Center.y + Size.y / 2f, Center.z + Size.z / 2f);
        }
    }
}
[System.Serializable]
public class ObstacleBounds : ColliderBounds
{
    public ObstacleBounds(Collider collider)
    {
        CalculateColliderBounds(collider);
    }
    public ObstacleBounds(Collider[] colliders)
    {
        CalculateCollidersBound(colliders);
    }

    public void CalculateCollidersBound(Collider[] colliders)
    {
        if (colliders != null && colliders.Length > 0)
        {
            Vector3 minPoint = colliders[0].bounds.min;
            Vector3 maxPoint = colliders[0].bounds.max;

            foreach (Collider collider in colliders)
            {
                minPoint = Vector3.Min(minPoint, collider.bounds.min);
                maxPoint = Vector3.Max(maxPoint, collider.bounds.max);
            }

            Min = minPoint;
            Max = maxPoint;

            Size = Max - Min;
            Center = (Min + Max) / 2f;

            Size.x = Mathf.Round(Size.x);
            Size.y = Mathf.Round(Size.y);
            Size.z = Mathf.Round(Size.z);

            if (Size.z <= 0.5f) Size.z = 0.5f;

            Center.x = Mathf.Round(Center.x);
            Center.y = Mathf.Round(Center.y);
            Center.z = Mathf.Round(Center.z);

            Min = new Vector3(Center.x - Size.x / 2f, Center.y - Size.y / 2f, Center.z - Size.z / 2f);
            Max = new Vector3(Center.x + Size.x / 2f, Center.y + Size.y / 2f, Center.z + Size.z / 2f);
        }
    }

}


