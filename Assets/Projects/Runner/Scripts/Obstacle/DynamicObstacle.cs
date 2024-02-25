using UnityEngine;

public class DynamicObstacle : Obstacle
{
    [SerializeField] private float _moveSpeed;
    private bool _isActive;

    private void Update()
    {
        if (!_isActive) return;

        transform.position += -transform.forward * _moveSpeed * Time.deltaTime;
    }
    public void Activate(bool isActive)
    {
        _isActive = isActive;
    }

    private void DeActivate()
    {
        _isActive = false;
    }

    public override void ResetPoolableObject()
    {
        base.ResetPoolableObject();
        DeActivate();
    }
}
